using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Connection;
[Route("api/[controller]")]
[ApiController]
public class GetUserMessageController : ControllerBase
{
    private readonly UserMessageService _entityService;

    public GetUserMessageController(UserMessageService entityService)
    {
        _entityService = entityService;
    }

    // GET: api/<ValuesController>
    [HttpGet]
    public IActionResult GetAll(string? search, Guid userSendId, Guid userReceiveId, string? sortBy, int page = 1, int pageSize = 5)
    {
        return Ok(_entityService.GetAllForTwo(search, userSendId, userReceiveId, sortBy, page, pageSize));

    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_entityService.GetById(id));
    }
}


