using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
