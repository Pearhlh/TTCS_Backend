using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteNotificationController : ControllerBase
    {
        private readonly NotificationService _entityService;

        public DeleteNotificationController(NotificationService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteNotificationAccess")]
        public IActionResult DeleteById(Guid id)
        {
            {
                return Ok(_entityService.DeleteById(id));
            }
        }
    }
}
