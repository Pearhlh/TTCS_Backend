using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourRating
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTourRatingController : ControllerBase
    {
        private readonly TourishRatingService _entityService;

        public GetTourRatingController(TourishRatingService entityService)
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
        public IActionResult GetAllByTourishPlanId(Guid tourishPlanId)
        {
            return Ok(_entityService.GetAllByTourishPlanId(tourishPlanId));
        }

        // GET: api/<ValuesController>
        [HttpGet("user")]
        public IActionResult GetByUserIdAndTourId(Guid userId, Guid tourishPlanId)
        {
            return Ok(_entityService.getByUserIdAndTourId(userId, tourishPlanId));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_entityService.GetById(id));
        }
    }
}

