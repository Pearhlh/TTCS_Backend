using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.TourRating
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendTourRatingController : ControllerBase
    {
        private readonly TourishRatingService _entityService;

        public SendTourRatingController(TourishRatingService entityService)
        {
            _entityService = entityService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult SendNotify(TourishRatingModel entityModel)
        {
            return Ok(_entityService.SendRating(entityModel));
        }
    }
}
