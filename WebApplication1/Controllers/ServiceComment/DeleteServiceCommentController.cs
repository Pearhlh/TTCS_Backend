using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.ServiceComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteServiceCommentController : ControllerBase
    {
        private readonly ServiceCommentService _entityService;

        public DeleteServiceCommentController(ServiceCommentService entityService)
        {
            _entityService = entityService;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteById(Guid id)
        {
            {
                string userId = User.FindFirstValue("Id");
                return Ok(_entityService.UserDeleteById(id, userId));
            }
        }
    }
}
