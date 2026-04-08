using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
