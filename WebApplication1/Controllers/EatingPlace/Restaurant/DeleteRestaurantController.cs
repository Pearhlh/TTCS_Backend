using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.EatingPlace.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteRestaurantController : ControllerBase
    {
        private readonly RestaurantService _entityService;

        public DeleteRestaurantController(RestaurantService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteRestaurantAccess")]
        public IActionResult DeleteById(Guid id)
        {
            return Ok(_entityService.DeleteById(id));
        }
    }
}
