using Microsoft.AspNetCore.Mvc;

namespace DinocoStore.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
