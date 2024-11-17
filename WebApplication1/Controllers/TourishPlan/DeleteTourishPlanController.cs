using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourishPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteTourishPlanController : ControllerBase
    {
        private readonly TourishPlanService _entityService;

        public DeleteTourishPlanController(TourishPlanService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteTourishPlanAccess")]
        public IActionResult DeleteById(Guid id)
        {
            return Ok(_entityService.DeleteById(id));
        }
    }
}
