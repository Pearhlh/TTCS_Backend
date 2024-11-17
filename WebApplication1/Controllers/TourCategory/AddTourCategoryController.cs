using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddTourCategoryController : ControllerBase
    {
        private readonly TourishCategoryService _entityService;

        public AddTourCategoryController(TourishCategoryService entityService)
        {
            _entityService = entityService;
        }

        [HttpPost]
        [Authorize(Policy = "CreateTourishCategoryAccess")]
        public IActionResult CreateNew(TourishCategoryModel entityModel)
        {
            return Ok(_entityService.CreateNew(entityModel));
        }
    }
}
