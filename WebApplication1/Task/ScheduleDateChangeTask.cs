namespace TourishApi.Task;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Data.Schedule;

public class ScheduleDateChangeTask
{
    private readonly MyDbContext _context;

    public ScheduleDateChangeTask(MyDbContext _context)
    {
        this._context = _context;
    }

    public async Task ScheduleDateDueTask()
    {
        var outOfTicketTourScheduleList = _context
            .TourishScheduleList.Where(entity =>
                entity.RemainTicket <= 0 && (int) entity.PlanStatus <= 1
            )
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in outOfTicketTourScheduleList)
        {
            item.PlanStatus = WebApplication1.Data.PlanStatus.OnGoing;
        }

        var onGoingTourScheduleList = _context
            .TourishScheduleList.Where(entity =>
                entity.StartDate < DateTime.UtcNow
                && entity.EndDate > DateTime.UtcNow
                && (int)entity.PlanStatus < 3
            )
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in onGoingTourScheduleList)
        {
            item.PlanStatus = WebApplication1.Data.PlanStatus.OnGoing;
        }

        var completeTourScheduleList = _context
            .TourishScheduleList.Where(entity =>
                entity.EndDate < DateTime.UtcNow && (int)entity.PlanStatus < 3
            )
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in completeTourScheduleList)
        {
            item.PlanStatus = WebApplication1.Data.PlanStatus.Complete;
        }

        var outOfTicketServiceScheduleList = _context
            .ServiceSchedule.Where(entity =>
                entity.RemainTicket <= 0 && (int)entity.Status <= 1
            )
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in outOfTicketServiceScheduleList)
        {
            item.Status = ScheduleStatus.OnGoing;
        }

        var onGoingScheduleScheduleList = _context
            .ServiceSchedule.Where(entity =>
                entity.StartDate < DateTime.UtcNow
                && entity.EndDate > DateTime.UtcNow
                && (int)entity.Status < 3
            )
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in onGoingScheduleScheduleList)
        {
            item.Status = ScheduleStatus.OnGoing;
        }

        var completeServiceScheduleList = _context
            .ServiceSchedule.Where(entity =>
                entity.EndDate < DateTime.UtcNow && (int)entity.Status < 3
            )
            .OrderBy(entity => entity.CreateDate)
            .AsSplitQuery()
            .ToList();

        foreach (var item in completeServiceScheduleList)
        {
            item.Status = ScheduleStatus.Completed;
        }

        await _context.SaveChangesAsync();
    }
}
