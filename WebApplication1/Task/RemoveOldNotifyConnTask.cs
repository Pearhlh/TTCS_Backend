namespace TourishApi.Task;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data.DbContextFile;

public class RemoveOldNotifyConnTask
{
    private readonly MyDbContext _context;

    public RemoveOldNotifyConnTask(MyDbContext _context)
    {
        this._context = _context;
    }

    public async Task RemoveOldConn()
    {
        var userIds = _context
            .NotificationConList.Where(entity => !entity.Connected)
            .Select(n => n.UserId)
            .Distinct()
            .ToList();

        foreach (var userId in userIds)
        {
            var notificationCount = _context
                .NotificationConList.Where(entity => !entity.Connected)
                .Count(n => n.UserId == userId);

            if (notificationCount > 5)
            {
                var notificationsToDelete = _context
                    .NotificationConList.Where(n => n.UserId == userId && !n.Connected)
                    .OrderBy(n => n.CreateDate)
                    .Take(notificationCount - 5)
                    .OrderBy(entity => entity.CreateDate)
                    .AsSplitQuery()
                    .ToList();

                // Delete the records
                _context.NotificationConList.RemoveRange(notificationsToDelete);
            }
        }

        await _context.SaveChangesAsync();
    }
}
