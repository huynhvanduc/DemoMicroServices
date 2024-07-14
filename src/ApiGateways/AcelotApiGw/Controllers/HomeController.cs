using Microsoft.AspNetCore.Mvc;

namespace AcelotApiGw.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}