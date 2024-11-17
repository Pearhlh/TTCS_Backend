using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Receipt
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteReceiptController : ControllerBase
    {
        private readonly ReceiptService _receiptService;

        private readonly char[] delimiter = new char[] { ';' };

        public DeleteReceiptController(ReceiptService receiptService
            )
        {
            _receiptService = receiptService;
        }

        [HttpDelete("tour/{id}")]
        [Authorize(Policy = "DeleteReceiptAccess")]
        public IActionResult DeleteTourReceiptById(int id)
        {
            return Ok(_receiptService.DeleteTourReceiptById(id));
        }

        [HttpDelete("schedule/{id}")]
        [Authorize(Policy = "DeleteReceiptAccess")]
        public IActionResult DeleteScheduleReceiptById(int id)
        {
            return Ok(_receiptService.DeleteScheduleReceiptById(id));
        }
    }
}
