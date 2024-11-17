using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.ServiceComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddServiceCommentController : ControllerBase
    {
        private readonly ServiceCommentService _entityService;

        public AddServiceCommentController(ServiceCommentService entityService)
        {
            _entityService = entityService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNew(ServiceCommentModel entityModel)
        {
            return Ok(await _entityService.CreateNewAsync(entityModel));
        }
    }
}
