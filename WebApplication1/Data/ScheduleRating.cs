using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data;

public enum ScheduleType
{
    EatSchedule = 0,
    MovingSchedule = 1,
    StayingSchedule = 2,
}

[Table("ScheduleRating")]
public class ScheduleRating
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScheduleId { get; set; }
    public ScheduleType ScheduleType { get; set; }
    public int Rating { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public User User { get; set; }

    public TourishPlan TourishPlan { get; set; }
}
