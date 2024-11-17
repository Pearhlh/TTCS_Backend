using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.Restaurant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.EatingPlace.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateRestaurantController : ControllerBase
    {
        private readonly RestaurantService _entityService;

        public UpdateRestaurantController(RestaurantService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateRestaurantAccess")]
        public IActionResult UpdateRestaurantById(Guid id, RestaurantModel RestaurantModel)
        {
            return Ok(_entityService.UpdateEntityById(id, RestaurantModel));
        }
    }
}
