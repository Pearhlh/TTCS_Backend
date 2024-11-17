using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourCategory
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteTourCategoryController : ControllerBase
    {
        private readonly TourishCategoryService _entityService;

        public DeleteTourCategoryController(TourishCategoryService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteTourishCategoryAccess")]
        public IActionResult DeleteById(Guid id)
        {
            {
                return Ok(_entityService.DeleteById(id));
            }
        }
    }
}
