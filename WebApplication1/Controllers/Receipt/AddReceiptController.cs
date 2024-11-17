using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.Receipt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Receipt
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddReceiptController : ControllerBase
    {
        private readonly ReceiptService _receiptService;

        private readonly char[] delimiter = new char[] { ';' };

        public AddReceiptController(ReceiptService receiptService
            )
        {
            _receiptService = receiptService;
        }

        [HttpPost]
        [Authorize(Policy = "CreateReceiptAccess")]
        public async Task<IActionResult> CreateNew(FullReceiptInsertModel receiptInsertModel)
        {
            return Ok(await _receiptService.CreateNew(receiptInsertModel));
        }

        [HttpPost("client")]
        public async Task<IActionResult> CreateNewForClient(FullReceiptClientInsertModel receiptInsertModel)
        {
            return Ok(await _receiptService.CreateNewForClient(receiptInsertModel));
        }
    }
}
