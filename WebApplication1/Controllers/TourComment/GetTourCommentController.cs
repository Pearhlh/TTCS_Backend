using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTourCommentController : ControllerBase
    {
        private readonly TourishCommentService _entityService;

        public GetTourCommentController(TourishCommentService entityService)
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
        [HttpGet("tourishplan")]
        public IActionResult GetAllByTourishPlanId(Guid tourishPlanId, string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            return Ok(_entityService.GetAllByTourishPlanId(tourishPlanId, search, type, sortBy, sortDirection, page, pageSize));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_entityService.GetById(id));
        }
    }
}

