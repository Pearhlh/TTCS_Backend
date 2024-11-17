using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;

        public GetFileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult GetAll(Guid resourceId, ResourceTypeEnum resourceType)
        {
            try
            {
                var fileList = _fileRepository.getByProductId(resourceId, resourceType);
                return Ok(fileList);
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C910",
                    Error = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }
    }
}

