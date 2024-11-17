using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.RestHouse.RestHouseContact
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteRestHouseContactController : ControllerBase
    {
        private readonly RestHouseContactService _entityService;

        public DeleteRestHouseContactController(RestHouseContactService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteRestHouseContactAccess")]
        public IActionResult DeleteById(Guid id)
        {
            return Ok(_entityService.DeleteById(id));
        }
    }
}
