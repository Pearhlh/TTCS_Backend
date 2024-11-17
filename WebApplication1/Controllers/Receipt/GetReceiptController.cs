using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;
using WebApplication1.Data.Receipt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Receipt
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetReceiptController : ControllerBase
    {
        private readonly ReceiptService _receiptService;

        private readonly char[] delimiter = new char[] { ';' };

        public GetReceiptController(ReceiptService receiptService
            )
        {
            _receiptService = receiptService;
        }


        // GET: api/<ValuesController>
        [HttpGet("tour")]
        public IActionResult GetAll(string? tourishPlanId, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5, FullReceiptStatus status = FullReceiptStatus.Created)
        {
            return Ok(_receiptService.GetAllTourReceipt(tourishPlanId, sortBy, sortDirection, page, pageSize, status));

        }

        // GET: api/<ValuesController>
        [HttpGet("schedule")]
        public IActionResult GetAll(string? movingScheduleId,
            string? stayingScheduleId,
            ScheduleType? scheduleType, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5, FullReceiptStatus status = FullReceiptStatus.Created)
        {
            return Ok(_receiptService.GetAllScheduleReceipt(movingScheduleId, stayingScheduleId, scheduleType, sortBy, sortDirection, page, pageSize, status));

        }

        // GET: api/<ValuesController>
        [HttpGet("user/tour")]
        public IActionResult GetAllTourReceiptForUser(string? email, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5, FullReceiptStatus status = FullReceiptStatus.Created)
        {
            return Ok(_receiptService.GetAllTourReceiptForUser(email, sortBy, sortDirection, page, pageSize, status));

        }

        [HttpGet("user/schedule")]
        public IActionResult GetAllScheduleReceiptForUser(string? email, ScheduleType? scheduleType, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5, FullReceiptStatus status = FullReceiptStatus.Created)
        {
            return Ok(_receiptService.GetAllScheduleReceiptForUser(email, scheduleType, sortBy, sortDirection, page, pageSize, status));

        }

        [HttpGet("tour/{id}")]
        public IActionResult GetTotalTourReceiptById(Guid id)
        {
            return Ok(_receiptService.GetTotalTourReceiptById(id));
        }

        [HttpGet("schedule/{id}")]
        public IActionResult GetTotalScheduleReceiptById(Guid id)
        {
            return Ok(_receiptService.GetTotalScheduleReceiptById(id));
        }
    }
}