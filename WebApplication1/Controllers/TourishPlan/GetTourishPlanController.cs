using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourishPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTourishPlanController : ControllerBase
    {
        private readonly TourishPlanService _entityService;
        private readonly IDatabase _redisDatabase;

        public GetTourishPlanController(
            TourishPlanService entityService,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _entityService = entityService;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? search,
            string? category,
            string? categoryString,
            string? startingPoint,
            string? endPoint,
            string? startingDate,
            double? priceFrom,
            double? priceTo,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            var result = await _entityService.GetAll(
                search,
                category,
                categoryString,
                startingPoint,
                endPoint,
                startingDate,
                priceFrom,
                priceTo,
                sortBy,
                sortDirection,
                "",
                page,
                pageSize
            );

            return Ok(result);
        }

        [HttpGet("with-authority")]
        [Authorize]
        public async Task<IActionResult> GetAllWithAuthority(
            string? search,
            string? category,
            string? categoryString,
            string? startingPoint,
            string? endPoint,
            string? startingDate,
            double? priceFrom,
            double? priceTo,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            string userId = User.FindFirstValue("Id");
            return Ok(
                await _entityService.GetAll(
                    search,
                    category,
                    categoryString,
                    startingPoint,
                    endPoint,
                    startingDate,
                    priceFrom,
                    priceTo,
                    sortBy,
                    sortDirection,
                    userId,
                    page,
                    pageSize
                )
            );
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_entityService.GetById(id));
        }

        [HttpGet("client/{id}")]
        public async Task<IActionResult> ClientGetById(Guid id)
        {
            var result = await _entityService.clientGetById(id);

            return Ok(result);

            // return Ok(_entityService.clientGetById(id));
        }

        [HttpGet("interest")]
        public IActionResult GetById(Guid tourishPlanId, Guid userId)
        {
            return Ok(_entityService.getTourInterest(tourishPlanId, userId));
        }

        [HttpGet("top-rating")]
        public IActionResult GetTopTourRating()
        {
            return Ok(_entityService.getTopTourRating());
        }
    }
}
