namespace TourishApi.Task;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data.DbContextFile;

public class RemoveOldNotifyTask
{
    private readonly MyDbContext _context;

    public RemoveOldNotifyTask(MyDbContext _context)
    {
        this._context = _context;
    }

    public async Task RemoveOldNotify()
    {
        var thresholdDate = DateTime.UtcNow.AddDays(-7);

        var notificationList = _context
            .Notifications.Where(entity => (entity.CreateDate ?? DateTime.UtcNow) <= thresholdDate)
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var notification in notificationList)
        {
            _context.Remove(notification);
        }

        await _context.SaveChangesAsync();
    }
}