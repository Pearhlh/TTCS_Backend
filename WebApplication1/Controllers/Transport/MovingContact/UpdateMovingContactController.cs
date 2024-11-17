using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.Transport;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Transport.MovingContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateMovingContactController : ControllerBase
    {
        private readonly MovingContactService _entityService;

        public UpdateMovingContactController(MovingContactService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMovingContactAccess")]
        public IActionResult UpdateMovingContactById(Guid id, MovingContactModel MovingContactModel)
        {
            return Ok(_entityService.UpdateEntityById(id, MovingContactModel));
        }
    }
}
