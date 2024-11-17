using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourishApi.Service.InheritanceService;
using WebApplication1.Model.Receipt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Receipt
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateReceiptController : ControllerBase
    {
        private readonly ReceiptService _receiptService;
        private readonly ILogger<UpdateReceiptController> _logger;

        private readonly char[] delimiter = new char[] { ';' };

        public UpdateReceiptController(ReceiptService receiptService,
            ILogger<UpdateReceiptController> logger
            )
        {
            _receiptService = receiptService;
            _logger = logger;
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateReceiptAccess")]
        public async Task<IActionResult> UpdateReceiptById(Guid fullReceiptId, FullReceiptUpdateModel receiptModel)
        {
            var emailClaim = User.FindFirstValue("Email");
            _logger.LogInformation("email-here: " + emailClaim);
            return Ok(await _receiptService.UpdateReceiptById(emailClaim, receiptModel));
        }

        [HttpPut("user/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReceiptForUserById(Guid fullReceiptId, FullReceiptUpdateModel receiptModel)
        {
            return Ok(await _receiptService.UpdateReceiptForUserById(fullReceiptId, receiptModel));
        }
    }
}
