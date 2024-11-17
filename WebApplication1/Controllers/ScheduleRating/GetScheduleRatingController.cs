using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.ScheduleRating
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetScheduleRatingController : ControllerBase
    {
        private readonly ScheduleRatingService _entityService;

        public GetScheduleRatingController(ScheduleRatingService entityService)
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
        [HttpGet("schedule")]
        public IActionResult GetAllByTourishPlanId(Guid tourishPlanId, ScheduleType scheduleType)
        {
            return Ok(_entityService.GetAllByScheduleId(tourishPlanId, scheduleType));
        }

        // GET: api/<ValuesController>
        [HttpGet("user")]
        public IActionResult GetByUserIdAndTourId(Guid userId, Guid scheduleId, ScheduleType scheduleType)
        {
            return Ok(_entityService.getByUserIdAndScheduleId(userId, scheduleId, scheduleType));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_entityService.GetById(id));
        }
    }
}

