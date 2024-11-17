using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourishPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateTourishPlanController : ControllerBase
    {
        private readonly TourishPlanService _entityService;

        public UpdateTourishPlanController(TourishPlanService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateTourishPlanAccess")]
        public async Task<IActionResult> UpdateTourishPlanById(TourishPlanUpdateModel entityModel)
        {
            string userId = User.FindFirstValue("Id");
            var response = await _entityService.UpdateEntityById(userId, entityModel);
            return Ok(response);
        }

        [HttpPut("interest")]
        [Authorize]
        public async Task<IActionResult> SetInterest(TourishInterestModel tourishInterestModel)
        {
            string userId = User.FindFirstValue("Id");
            var response = await _entityService.setTourInterest(tourishInterestModel.TourishPlanId,
                new Guid(userId), tourishInterestModel.InterestStatus);
            return Ok(response);
        }
    }
}
