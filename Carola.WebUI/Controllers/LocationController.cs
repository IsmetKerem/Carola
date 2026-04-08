using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers;

public class LocationController : Controller
{
    private readonly ILocationService _locationService;
    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }
    // GET
    public async  Task<IActionResult> LocationList()
    {
        var values = await _locationService.TGetAllAsync();
        return View(values);
    }
}