using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateNotificationController : ControllerBase
    {
        private readonly NotificationService _entityService;

        public UpdateNotificationController(NotificationService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateNotificationAccess")]
        public async Task<IActionResult> UpdateNotificationById(Guid id, NotificationModel NotificationModel)
        {
            var response = await _entityService.UpdateEntityByIdAsync(id, NotificationModel);
            return Ok(response);
        }
    }
}
