using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // ?status=1 (Pending), 2 (Approved), 3 (Rejected) — null ise tümü
        public async Task<IActionResult> Index(int? status)
        {
            var list = status.HasValue
                ? await _reservationService.TGetReservationsByStatusAsync((ReservationStatus)status.Value)
                : await _reservationService.TGetReservationsWithDetailsAsync();

            ViewBag.CurrentStatus = status;

            // Sayaçları üst kısımda tab'larda göstermek için
            var allReservations = await _reservationService.TGetReservationsWithDetailsAsync();
            ViewBag.PendingCount   = allReservations.Count(r => r.Status == ReservationStatus.Pending);
            ViewBag.ApprovedCount  = allReservations.Count(r => r.Status == ReservationStatus.Approved);
            ViewBag.RejectedCount  = allReservations.Count(r => r.Status == ReservationStatus.Rejected);
            ViewBag.TotalCount     = allReservations.Count;

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
            await _reservationService.TApproveReservationAsync(id);
            // TODO Sprint 10: Mail gönder (MailService)
            TempData["Success"] = "Rezervasyon onaylandı. (Mail bildirimi Sprint 10'da bağlanacak)";
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