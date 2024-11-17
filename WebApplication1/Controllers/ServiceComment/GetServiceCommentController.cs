using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.ServiceComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetServiceCommentController : ControllerBase
    {
        private readonly ServiceCommentService _entityService;

        public GetServiceCommentController(ServiceCommentService entityService)
        {
            _entityService = entityService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            return Ok(_entityService.GetAll(search, type, sortBy, sortDirection, page, pageSize));

        }

        // GET: api/<ValuesController>
        [HttpGet("service")]
        public IActionResult GetAllByServiceId(Guid ServiceId, ScheduleType serviceType, string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            return Ok(_entityService.GetAllByServiceId(ServiceId, serviceType, search, type, sortBy, sortDirection, page, pageSize));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_entityService.GetById(id));
        }
    }
}

