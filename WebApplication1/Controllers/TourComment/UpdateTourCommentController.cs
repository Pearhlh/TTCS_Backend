using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateTourCommentController : ControllerBase
    {
        private readonly TourishCommentService _entityService;

        public UpdateTourCommentController(TourishCommentService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateTourishCommentById(Guid id, TourishCommentModel TourishCommentModel)
        {
            return Ok(_entityService.UpdateEntityById(id, TourishCommentModel));
        }
    }
}
