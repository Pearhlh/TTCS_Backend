using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.RestHouse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.RestHouse.RestHouseContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateRestHouseContactController : ControllerBase
    {
        private readonly RestHouseContactService _entityService;

        public UpdateRestHouseContactController(RestHouseContactService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateRestHouseContactAccess")]
        public IActionResult UpdateRestHouseContactById(Guid id, RestHouseContactModel RestHouseContactModel)
        {
            return Ok(_entityService.UpdateEntityById(id, RestHouseContactModel));
        }
    }
}
