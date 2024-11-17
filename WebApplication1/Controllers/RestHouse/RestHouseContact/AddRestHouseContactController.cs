using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.RestHouse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.RestHouse.RestHouseContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddRestHouseContactController : ControllerBase
    {
        private readonly RestHouseContactService _entityService;

        public AddRestHouseContactController(RestHouseContactService airPlaneService)
        {
            _entityService = airPlaneService;
        }

        [HttpPost]
        [Authorize(Policy = "CreateRestHouseContactAccess")]
        public IActionResult CreateNew(RestHouseContactModel entityModel)
        {
            return Ok(_entityService.CreateNew(entityModel));
        }
    }
}
