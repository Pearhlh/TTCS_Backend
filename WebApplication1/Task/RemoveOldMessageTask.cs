namespace TourishApi.Task;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data.DbContextFile;

public class RemoveOldMessageTask
{
    private readonly MyDbContext _context;

    public RemoveOldMessageTask(MyDbContext _context)
    {
        this._context = _context;
    }

    public async Task RemoveOldMessage()
    {
        var thresholdDate = DateTime.UtcNow.AddDays(-30);

        var guestMessageList = _context
            .GuestMessages
            .Where(entity => (entity.CreateDate ?? DateTime.UtcNow) <= thresholdDate)
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var message in guestMessageList)
        {
            _context.Remove(message);
        }

        await _context.SaveChangesAsync();
    }
}
