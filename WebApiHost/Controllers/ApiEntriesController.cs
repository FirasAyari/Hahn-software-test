using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApiHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiEntriesController : ControllerBase
{
    private readonly IApiEntryService _apiEntryService;

    public ApiEntriesController(IApiEntryService apiEntryService)
    {
        _apiEntryService = apiEntryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiEntry>>> GetApiEntries()
    {
        var apiEntries = await _apiEntryService.GetAllApiEntries();
        return Ok(apiEntries);
    }
}