using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.Transport;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Transport.MovingContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddMovingContactController : ControllerBase
    {
        private readonly MovingContactService _entityService;

        public AddMovingContactController(MovingContactService airPlaneService)
        {
            _entityService = airPlaneService;
        }

        [HttpPost]
        [Authorize(Policy = "CreateMovingContactAccess")]
        public IActionResult CreateNew(MovingContactModel entityModel)
        {
            return Ok(_entityService.CreateNew(entityModel));
        }
    }
}
