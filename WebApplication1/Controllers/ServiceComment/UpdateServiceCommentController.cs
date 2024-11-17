using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.ServiceComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateServiceCommentController : ControllerBase
    {
        private readonly ServiceCommentService _entityService;

        public UpdateServiceCommentController(ServiceCommentService entityService)
        {
            _entityService = entityService;
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateServiceCommentById(Guid id, ServiceCommentModel ServiceCommentModel)
        {
            return Ok(_entityService.UpdateEntityById(id, ServiceCommentModel));
        }
    }
}
