using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;
using WebApplication1.Service;

namespace WebApplication1.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IFileRepository _fileRepository;

        public FileUploadController(IBlobService blobService, IFileRepository fileRepository)
        {
            this._blobService = blobService;
            this._fileRepository = fileRepository;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromForm] Guid productId, [FromForm] string productType)
        {
            try
            {
                var files = Request.Form.Files;

                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }

                foreach (var file in files)
                {
                    var fileNameOld = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    FileInfo fi = new FileInfo(fileNameOld);
                    string ext = fi.Extension;

                    var productTypeValue = ResourceTypeEnum.Tour;

                    if (productType == "1")
                    {
                        productTypeValue = ResourceTypeEnum.Tour;
                    }
                    else if (productType == "0")
                    {
                        productTypeValue = ResourceTypeEnum.Avatar;
                    }
                    else if (productType == "2")
                    {
                        productTypeValue = ResourceTypeEnum.EatContact;
                    }
                    else if (productType == "3")
                    {
                        productTypeValue = ResourceTypeEnum.MovingContact;
                    }
                    else if (productType == "4")
                    {
                        productTypeValue = ResourceTypeEnum.RestHouseContact;
                    }

                    var fileSaveModel = new FileModel
                    {
                        FileType = ext,
                        ResourceType = productTypeValue,
                        AccessSourceId = productId,
                        CreatedDate = DateTime.UtcNow
                    };

                    var returnResponse = await this._fileRepository.Add(fileSaveModel);

                    var fileName = productType + "_" + (returnResponse.returnId) + ext;

                    var result = await this._blobService.UploadFileBlobAsync(
                       productType + "-container",
                       file.OpenReadStream(),
                       file.ContentType,
                       fileName);
                }

                var response = new Response
                {
                    resultCd = 0,
                    MessageCode = "I912",
                };

                return Ok(response);


            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C914",
                    Data = ex
                };
                return StatusCode(500, response);
            }
        }
    }
}
