namespace TourishApi.Task;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data.DbContextFile;

public class RemoveOldMessageConTask
{
    private readonly MyDbContext _context;

    public RemoveOldMessageConTask(MyDbContext _context)
    {
        this._context = _context;
    }

    public async Task RemoveOldConn()
    {
        var thresholdDate = DateTime.UtcNow.AddDays(-30);
        var guestHisConList = _context
            .GuestMessageConHisList.Include(entity => entity.AdminCon)
            .Include(entity => entity.GuestCon)
            .Where(entity => entity.CreateDate <= thresholdDate)
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var guestCon in guestHisConList)
        {
            _context.Remove(guestCon.AdminCon);
            _context.Remove(guestCon.GuestCon);
            _context.Remove(guestCon);
        }

        await _context.SaveChangesAsync();
    }
}
