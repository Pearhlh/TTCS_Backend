using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;
using WebApplication1.Service;

namespace WebApplication1.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDeleteController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IFileRepository _fileRepository;

        public FileDeleteController(IBlobService blobService, IFileRepository fileRepository)
        {
            this._blobService = blobService;
            this._fileRepository = fileRepository;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile(FileDeleteModel fileDeleteModel)
        {
            try
            {
                var fileIdList = fileDeleteModel.FileIdListString.Split(';');

                if (fileIdList.Length <= 0)
                {
                    return BadRequest();
                }

                foreach (var fileId in fileIdList)
                {

                    var fileInDb = await this._fileRepository.getById(new Guid(fileId));

                    var ext = fileInDb.type;

                    this._fileRepository.Delete(new Guid(fileId));

                    var fileName = fileDeleteModel.ProductType + "_" + fileId + ext;

                    var result = await this._blobService.DeleteFileBlobAsync(
                       fileDeleteModel.ProductType + "-container",
                       fileName);
                }

                var response = new Response
                {
                    resultCd = 0,
                    MessageCode = "I913",
                };

                return Ok(response);


            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C914",
                    Data = ex.Message
                };
                return StatusCode(500, response);
            }
        }
    }
}
