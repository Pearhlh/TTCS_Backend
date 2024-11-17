namespace TourishApi.Task;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TourishApi.Service.InheritanceService;
using TourishApi.Service.InheritanceService.Schedule;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Data.Receipt;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Service.InheritanceService;

public class ReceiptStatusChangeTask
{
    private readonly MyDbContext _context;
    private NotificationService _notificationService;
    private UserService _userService;
    private readonly TourishPlanService _tourishPlanService;

    private readonly MovingScheduleService _movingScheduleService;
    private readonly StayingScheduleService _stayingScheduleService;

    public ReceiptStatusChangeTask(
        MyDbContext _context,
        NotificationService notificationService,
        UserService userService,
        TourishPlanService tourishPlanService,
        MovingScheduleService movingScheduleService,
        StayingScheduleService stayingScheduleService
    )
    {
        this._context = _context;
        _notificationService = notificationService;
        _userService = userService;
        _tourishPlanService = tourishPlanService;
        _movingScheduleService = movingScheduleService;
        _stayingScheduleService = stayingScheduleService;
    }

    public async Task ReceiptStatusTask()
    {
        var thresholdDate = DateTime.UtcNow.AddHours(-2);

        var createTourReceipt = _context
            .FullReceiptList.Include(entity => entity.TotalReceipt)
            .Where(entity =>
                entity.CreatedDate < DateTime.UtcNow
                && entity.Status == FullReceiptStatus.Created
                && entity.CreatedDate <= thresholdDate
            )
            .OrderBy(entity => entity.CreatedDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in createTourReceipt)
        {
            item.Status = FullReceiptStatus.AwaitPayment;
            item.CompleteDate = DateTime.UtcNow;

            await sendTourPaymentNotifyToUser(
                item.Email,
                item.TotalReceipt.TourishPlanId.Value,
                "I511-user-await"
            );
        }

        var createServiceReceipt = _context
            .FullScheduleReceiptList
            .Include(entity => entity.TotalReceipt)
            .Where(entity =>
                entity.CreatedDate < DateTime.UtcNow
                && entity.Status == FullReceiptStatus.Created
                && entity.CreatedDate <= thresholdDate
            )
            .OrderBy(entity => entity.CreatedDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in createServiceReceipt)
        {
            item.Status = FullReceiptStatus.AwaitPayment;
            item.CompleteDate = DateTime.UtcNow;

            await sendServicePaymentNotifyToUser(
                item.Email,
                item.TotalReceipt.MovingScheduleId,
                item.TotalReceipt.StayingScheduleId,
                "I511-user-await"
            );
        }

        await _context.SaveChangesAsync();
    }

    public async Task ReceiptCancelStatusTask()
    {
        var thresholdDate = DateTime.UtcNow.AddDays(-8);

        var onWaitTourReceipt = _context
            .FullReceiptList.Include(entity => entity.TotalReceipt)
            .Where(entity =>
                entity.CreatedDate < DateTime.UtcNow
                && entity.Status == FullReceiptStatus.AwaitPayment
                && entity.CreatedDate <= thresholdDate
            )
            .OrderBy(entity => entity.CreatedDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in onWaitTourReceipt)
        {
            item.Status = FullReceiptStatus.Cancelled;
            item.CompleteDate = DateTime.UtcNow;

            await sendTourPaymentNotifyToUser(
                item.Email,
                item.TotalReceipt.TourishPlanId.Value,
                "I511-user-cancel"
            );

            await _tourishPlanService.sendTourPaymentNotifyToAdmin(
                item.Email,
                item.TotalReceipt.TourishPlanId.Value,
                "I511-user-cancel"
            );
        }

        var onWaitServiceReceipt = _context
            .FullScheduleReceiptList.Include(entity => entity.TotalReceipt)
            .Where(entity =>
                entity.CreatedDate < DateTime.UtcNow
                && entity.Status == FullReceiptStatus.AwaitPayment
                 && entity.CreatedDate <= thresholdDate
            )
            .OrderBy(entity => entity.CreatedDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in onWaitServiceReceipt)
        {
            item.Status = FullReceiptStatus.Cancelled;
            item.CompleteDate = DateTime.UtcNow;

            await sendServicePaymentNotifyToUser(
                item.Email,
                item.TotalReceipt.MovingScheduleId,
                item.TotalReceipt.StayingScheduleId,
                "I511-user-cancel"
            );

            if (item.TotalReceipt.StayingScheduleId.HasValue)
                await _stayingScheduleService.sendTourPaymentNotifyToAdmin(
                    item.Email,
                    item.TotalReceipt.StayingScheduleId.Value,
                    "I511-user-cancel"
                );
            if (item.TotalReceipt.MovingScheduleId.HasValue)
                await _movingScheduleService.sendTourPaymentNotifyToAdmin(
                    item.Email,
                    item.TotalReceipt.MovingScheduleId.Value,
                    "I511-user-cancel"
                );
        }

        await _context.SaveChangesAsync();
    }


    private async Task<Response> sendTourPaymentNotifyToUser(
        string email,
        Guid tourishPlanId,
        string contentCode
    )
    {
        var systemUser = (User)_userService.getByName("admin", 4).Data;
        var user = (User)_userService.getUserByEmail(email).Data;

        if (user != null && systemUser != null)
        {
            var notification = new NotificationModel
            {
                UserCreateId = systemUser.Id,
                UserReceiveId = user.Id,
                TourishPlanId = tourishPlanId,
                IsGenerate = true,
                Content = "",
                ContentCode = contentCode,
                IsRead = false,
                IsDeleted = false,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            // _notificationService.CreateNew(notification);

            return await _notificationService.CreateNewAsync(user.Id, notification);
        }

        return new Response();
    }

    private async Task<Response> sendServicePaymentNotifyToUser(
        string email,
        Guid? movingServiceId,
        Guid? stayingServiceId,
        string contentCode
    )
    {
        var systemUser = (User)_userService.getByName("admin", 4).Data;
        var user = (User)_userService.getUserByEmail(email).Data;

        if (user != null && systemUser != null)
        {
            var notification = new NotificationModel
            {
                UserCreateId = systemUser.Id,
                UserReceiveId = user.Id,
                MovingScheduleId = movingServiceId,
                StayingScheduleId = stayingServiceId,
                IsGenerate = true,
                Content = "",
                ContentCode = contentCode,
                IsRead = false,
                IsDeleted = false,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            return await _notificationService.CreateNewAsync(user.Id, notification);
        }

        return new Response();
    }
}
