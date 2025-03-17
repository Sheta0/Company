using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Company.PL.Models;
using Company.PL.Services;
using System.Text;

namespace Company.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IScopedService _scopedService01;
    private readonly IScopedService _scopedService02;
    private readonly ITransientService _transientService01;
    private readonly ITransientService _transientService02;
    private readonly ISingletonService _singletonService01;
    private readonly ISingletonService _singletonService02;

    public HomeController(
        ILogger<HomeController> logger,
        IScopedService scopedService01,
        IScopedService scopedService02,
        ITransientService transientService01,
        ITransientService transientService02,
        ISingletonService singletonService01,
        ISingletonService singletonService02)
    {
        _scopedService01 = scopedService01;
        _scopedService02 = scopedService02;
        _transientService01 = transientService01;
        _transientService02 = transientService02;
        _singletonService01 = singletonService01;
        _singletonService02 = singletonService02;
        _logger = logger;
    }

    public IActionResult TestLifeTime()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"Scoped 01: {_scopedService01.GetGuid()}\n");
        builder.AppendLine($"Scoped 02: {_scopedService02.GetGuid()}\n\n");
        builder.AppendLine($"Transient 01: {_transientService01.GetGuid()}\n");
        builder.AppendLine($"Transient 02: {_transientService02.GetGuid()}\n\n");
        builder.AppendLine($"Singleton 01: {_singletonService01.GetGuid()}\n");
        builder.AppendLine($"Singleton 02: {_singletonService02.GetGuid()}\n\n");

        return Content(builder.ToString());
    }


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
