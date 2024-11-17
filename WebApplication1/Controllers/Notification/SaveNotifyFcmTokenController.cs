using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveNotifyFcmTokenController : ControllerBase
    {
        private readonly NotificationService _entityService;

        public SaveNotifyFcmTokenController(NotificationService entityService)
        {
            _entityService = entityService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult SaveNotifyFcmToken(NotificationFcmTokenModel entityModel)
        {
            return Ok(_entityService.saveFcmToken(entityModel));
        }
    }
}
