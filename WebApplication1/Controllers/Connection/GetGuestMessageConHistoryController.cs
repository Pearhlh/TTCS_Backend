using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Connection;
[Route("api/[controller]")]
[ApiController]
public class GetGuestMessageConHistoryController : ControllerBase
{
    private readonly GuestMessageConHistoryService _entityService;

    public GetGuestMessageConHistoryController(GuestMessageConHistoryService entityService)
    {
        _entityService = entityService;
    }

    // GET: api/<ValuesController>
    [HttpGet]
    [Authorize]
    public IActionResult GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
    {
        return Ok(_entityService.GetAll(search, type, sortBy, sortDirection, page, pageSize));

    }

    // GET: api/<ValuesController>
    [HttpGet("admin")]
    [Authorize]
    public IActionResult GetAllForAdmin(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
    {
        string userId = User.FindFirstValue("Id");
        return Ok(_entityService.GetAllForAdmin(search, type, sortBy, sortDirection, userId, page, pageSize));

    }

    // GET: api/<ValuesController>
    [HttpGet("guest-connection")]
    [Authorize]
    public async Task<IActionResult> GetByGuestConId(string connectionId)
    {
        return Ok(await _entityService.getByGuestConId(connectionId));

    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_entityService.GetById(id));
    }
}


