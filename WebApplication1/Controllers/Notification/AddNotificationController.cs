using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddNotificationController : ControllerBase
    {
        private readonly NotificationService _entityService;

        public AddNotificationController(NotificationService entityService)
        {
            _entityService = entityService;
        }

        [HttpPost]
        [Authorize(Policy = "CreateNotificationAccess")]
        public async Task<IActionResult> CreateNew(NotificationModel entityModel)
        {
            NotificationModel entityInsertModel = entityModel;
            entityInsertModel.IsGenerate = false;
            return Ok(await _entityService.CreateNewAsync(entityModel.UserReceiveId.Value, entityInsertModel));
        }
    }
}
