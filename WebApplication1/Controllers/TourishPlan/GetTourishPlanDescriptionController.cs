using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourishPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTourishPlanDescriptionController : ControllerBase
    {
        private readonly TourishPlanService _entityService;
        private readonly IDatabase _redisDatabase;

        public GetTourishPlanDescriptionController(
            TourishPlanService entityService,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _entityService = entityService;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetDescription(string containerName, string blobName)
        {
            var result = await _entityService.getDescription(containerName, blobName);

            return Ok(result);

            // return Ok(await _entityService.getDescription(containerName, blobName));
        }

        [HttpGet("client")]
        public async Task<IActionResult> clientGetDescription(string containerName, string blobName)
        {
            var result = await _entityService.clientGetDescription(containerName, blobName);

            return Ok(result);

            // return Ok(await _entityService.getDescription(containerName, blobName));
        }
    }
}
