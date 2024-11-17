using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService.Schedule;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Schedule
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteStayingScheduleController : ControllerBase
    {
        private readonly StayingScheduleService _entityService;

        public DeleteStayingScheduleController(StayingScheduleService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteTourishPlanAccess")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            return Ok(await _entityService.DeleteById(id));
        }
    }
}
