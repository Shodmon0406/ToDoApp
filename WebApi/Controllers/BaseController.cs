using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
public class BaseController : ControllerBase
{
    protected List<string> ModelStateErrors() =>
        ModelState.SelectMany(e => e.Value!.Errors.Select(er => er.ErrorMessage)).ToList();
}