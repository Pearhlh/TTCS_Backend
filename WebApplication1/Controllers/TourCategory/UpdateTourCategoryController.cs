using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateTourCategoryController : ControllerBase
    {
        private readonly TourishCategoryService _entityService;

        public UpdateTourCategoryController(TourishCategoryService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateTourishCategoryAccess")]
        public IActionResult UpdateTourishCategoryById(Guid id, TourishCategoryModel TourishCategoryModel)
        {
            return Ok(_entityService.UpdateEntityById(id, TourishCategoryModel));
        }
    }
}
