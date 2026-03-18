using Microsoft.AspNetCore.Mvc;

namespace ContactManagerApp.Web.Controllers;

public class EmployeeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }
}