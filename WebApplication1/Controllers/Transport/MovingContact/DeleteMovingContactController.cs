using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Transport.MovingContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteMovingContactController : ControllerBase
    {
        private readonly MovingContactService _entityService;

        public DeleteMovingContactController(MovingContactService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMovingContactAccess")]
        public IActionResult DeleteById(Guid id)
        {
            return Ok(_entityService.DeleteById(id));
        }
    }
}
