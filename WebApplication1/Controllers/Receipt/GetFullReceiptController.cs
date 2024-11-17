using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourishApi.Service.InheritanceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Receipt
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFullReceiptController : ControllerBase
    {
        private readonly ReceiptService _receiptService;

        private readonly char[] delimiter = new char[] { ';' };

        public GetFullReceiptController(ReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet("tour/{id}")]
        public IActionResult GetFullTourReceiptById(int id)
        {
            return Ok(_receiptService.GetFullTourReceiptById(id));
        }

        [HttpGet("schedule/{id}")]
        public IActionResult GetFullScheduleReceiptById(int id)
        {
            return Ok(_receiptService.GetFullScheduleReceiptById(id));
        }

        [HttpGet("tour/unpaid-client")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getUnpaidTourClient()
        {
            return Ok(_receiptService.getUnpaidTourClient());
        }

        [HttpGet("moving-schedule/unpaid-client")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getUnpaidMovingScheduleClient()
        {
            return Ok(_receiptService.getUnpaidMovingScheduleClient());
        }

        [HttpGet("staying-schedule/unpaid-client")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getUnpaidStayingScheduleClient()
        {
            return Ok(_receiptService.getUnpaidStayingScheduleClient());
        }

        [HttpGet("gross-tour")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getTopGrossTourInMonth()
        {
            return Ok(_receiptService.getTopGrossTourInMonth());
        }

        [HttpGet("total-ticket-tour")]
        [Authorize]
        public IActionResult getTopTicketTourInMonth()
        {
            return Ok(_receiptService.getTopTicketTourInMonth());
        }

        [HttpGet("total-ticket-of-tour")]
        public async Task<IActionResult> getTicketOfTourInMonth(Guid tourishPlanId)
        {
            return Ok(await _receiptService.getTicketOfTourInMonth(tourishPlanId));
        }

        [HttpGet("gross-moving-service")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getTopGrossMovingScheduleInMonth()
        {
            return Ok(_receiptService.getTopGrossMovingScheduleInMonth());
        }

        [HttpGet("gross-staying-service")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getTopGrossStayingScheduleInMonth()
        {
            return Ok(_receiptService.getTopGrossStayingScheduleInMonth());
        }

        [HttpGet("total-ticket-moving-service")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getTopTicketMovingScheduleInMonth()
        {
            return Ok(_receiptService.getTopTicketMovingScheduleInMonth());
        }

        [HttpGet("total-ticket-staying-service")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getTopTicketStayingScheduleInMonth()
        {
            return Ok(_receiptService.getTopTicketStayingScheduleInMonth());
        }

        [HttpGet("tourish-plan/total-month-gross")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getGrossTourishPlanInYear()
        {
            return Ok(_receiptService.getGrossTourishPlanInYear());
        }

        [HttpGet("moving-schedule/total-month-gross")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getGrossMovingScheduleInYear()
        {
            return Ok(_receiptService.getGrossMovingScheduleInYear());
        }

        [HttpGet("staying-schedule/total-month-gross")]
        [Authorize(Roles = "Admin, AdminManager")]
        public IActionResult getGrossStayingScheduleInYear()
        {
            return Ok(_receiptService.getGrossStayingScheduleInYear());
        }
    }
}
