using Microsoft.AspNetCore.Mvc;

namespace MediCare_MVC_Project.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
