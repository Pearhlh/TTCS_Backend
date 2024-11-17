using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.Restaurant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.EatingPlace.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddRestaurantController : ControllerBase
    {
        private readonly RestaurantService _entityService;

        public AddRestaurantController(RestaurantService airPlaneService)
        {
            _entityService = airPlaneService;
        }

        [HttpPost]
        [Authorize(Policy = "CreateRestaurantAccess")]
        public IActionResult CreateNew(RestaurantModel entityModel)
        {
            return Ok(_entityService.CreateNew(entityModel));
        }
    }
}
