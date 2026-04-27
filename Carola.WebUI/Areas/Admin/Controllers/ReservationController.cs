using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.MailDtos;
using Carola.EntityLayer.Enums;
using Carola.WebUI.Models;
using Carola.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IMailService _mailService;
        private readonly IRazorViewToStringRenderer _viewRenderer;
        private readonly IWebHostEnvironment _env;

        public ReservationController(
            IReservationService reservationService,
            IMailService mailService,
            IRazorViewToStringRenderer viewRenderer,
            IWebHostEnvironment env)
        {
            _reservationService = reservationService;
            _mailService = mailService;
            _viewRenderer = viewRenderer;
            _env = env;
        }

        public async Task<IActionResult> Index(int? status)
        {
            var list = status.HasValue
                ? await _reservationService.TGetReservationsByStatusAsync((ReservationStatus)status.Value)
                : await _reservationService.TGetReservationsWithDetailsAsync();

            ViewBag.CurrentStatus = status;

            var allReservations = await _reservationService.TGetReservationsWithDetailsAsync();
            ViewBag.PendingCount  = allReservations.Count(r => r.Status == ReservationStatus.Pending);
            ViewBag.ApprovedCount = allReservations.Count(r => r.Status == ReservationStatus.Approved);
            ViewBag.RejectedCount = allReservations.Count(r => r.Status == ReservationStatus.Rejected);
            ViewBag.TotalCount    = allReservations.Count;

            return View(list);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var reservation = await _reservationService.TGetByIdDtoAsync(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var allReservations = await _reservationService.TGetReservationsWithDetailsAsync();
            var reservation = allReservations.FirstOrDefault(r => r.ReservationId == id);

            if (reservation == null)
            {
                TempData["Error"] = "Rezervasyon bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            await _reservationService.TApproveReservationAsync(id);

            try
            {
                if (string.IsNullOrWhiteSpace(reservation.CustomerEmail))
                {
                    TempData["Warning"] = "Rezervasyon onaylandı ancak müşterinin email adresi eksik, mail gönderilemedi.";
                    return RedirectToAction(nameof(Index));
                }

                var mailModel = new ReservationApprovedMailModel
                {
                    CustomerFullName = reservation.CustomerFullName,
                    CarBrandModel    = $"{reservation.BrandName} {reservation.CarModel}",
                    PickupDate       = reservation.PickupDate,
                    ReturnDate       = reservation.ReturnDate,
                    PickupLocation   = reservation.PickupLocationName ?? "",
                    ReturnLocation   = reservation.ReturnLocationName ?? "",
                    TotalPrice       = reservation.TotalPrice,
                    CouponCode       = $"CAROLA{DateTime.Now:yyyyMMdd}{id}"
                };

                var htmlBody = await _viewRenderer.RenderViewToStringAsync(
                    "/Views/EmailTemplates/ReservationApproved.cshtml", mailModel);

                var couponPath = Path.Combine(_env.WebRootPath, "email-assets", "coupon-30.png");

                var mail = new MailMessageDto
                {
                    ToEmail  = reservation.CustomerEmail,
                    ToName   = reservation.CustomerFullName,
                    Subject  = $"🎉 Rezervasyonunuz Onaylandı — {reservation.BrandName} {reservation.CarModel}",
                    HtmlBody = htmlBody,
                    EmbeddedImages = new Dictionary<string, string>
                    {
                        { "coupon-image", couponPath }
                    }
                };

                var sent = await _mailService.SendAsync(mail);
                if (sent)
                    TempData["Success"] = $"Rezervasyon onaylandı ve müşteriye mail gönderildi ({reservation.CustomerEmail}).";
                else
                    TempData["Warning"] = "Rezervasyon onaylandı ancak mail gönderilemedi. Logları kontrol edin.";
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Rezervasyon onaylandı ancak mail gönderiminde hata: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            await _reservationService.TRejectReservationAsync(id);
            TempData["Success"] = "Rezervasyon reddedildi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _reservationService.TDeleteAsync(id);
            TempData["Success"] = "Rezervasyon silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}