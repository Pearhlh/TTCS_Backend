using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TourishApi.Extension;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Data.Schedule;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;
using WebApplication1.Service;

namespace WebApplication1.Repository.InheritanceRepo;

public class TourishPlanRepository : ITourishPlanRepository
{
    private readonly MyDbContext _context;
    private readonly IBlobService blobService;
    private readonly ILogger<TourishPlanRepository> logger;
    public static int PAGE_SIZE { get; set; } = 5;
    private readonly char[] delimiter = new char[] { ';' };

    private readonly string[] priceInstructionLines = new string[]
    {
        "Hộ chiếu phải còn thời hạn sử dụng trên 6 tháng, Tính từ ngày khởi hành đi và về.",
        "Hành lý quá cước qui định. Xe vận chuyển ngoài chương trình + Các show về đêm.",
        "Điện thoại, giặt ủi, nước uống trong phòng khách sạn và các chi phí cá nhân khác.",
        "Phí bồi dưỡng cho hướng dẫn viên 125.000VND/Ngày/Khách."
    };

    private readonly string[] cautionInstructionLines = new string[]
    {
        "Tour du lịch thuần túy, Quý khách vui lòng không rời đoàn.",
        "Du khách Việt Kiều hoặc nước ngoài phải có visa tái nhập nhiều lần hoặc miễn thị thực 5 năm và mang theo khi tham gia tour.",
        "Trẻ em dưới 16 tuổi phải có bố mẹ đi cùng hoặc người được uỷ quyền có giấy uỷ quyền từ bố mẹ.",
        "Hộ chiếu phải mang theo bản gốc hợp lệ không bị rạn, phai mờ, và còn thời hạn sử dụng trên 6 tháng (tính từ ngày khởi hành).",
        "Không sử dụng thẻ xanh. Nếu sử dụng Sổ Du lịch (yêu cầu visa nước nhập cảnh), vui lòng thông báo cho nhân viên nhận tour nếu Quý khách sử dụng các hồ sơ khác ngoài hộ chiếu.",
        "Công ty du lịch không chịu trách nhiệm nếu Quý khách bị từ chối nhập cảnh với bất kỳ lý do nào từ hải quan nước ngoài.",
        "Công ty được phép thay đổi lịch trình chuyến đi và sử dụng các hãng hàng không thay thế, nhưng vẫn đảm bảo tham quan đầy đủ các điểm theo chương trình.",
        "Thứ tự điểm tham quan và lộ trình có thể thay đổi tùy theo tình hình thực tế, nhưng vẫn đảm bảo đầy đủ các điểm tham quan như ban đầu.",
        "Trong trường hợp bất khả kháng như khủng bố, thiên tai hoặc thay đổi lịch trình của phương tiện công cộng (máy bay, tàu hỏa...), Công ty du lịch giữ quyền điều chỉnh lộ trình tour cho phù hợp và an toàn cho khách hàng, mà không phải chịu trách nhiệm bồi thường thiệt hại phát sinh."
    };

    public TourishPlanRepository(
        MyDbContext _context,
        IBlobService blobService,
        ILogger<TourishPlanRepository> _logger
    )
    {
        this._context = _context;
        this.blobService = blobService;
        this.logger = _logger;
    }

    public async Task<Response> Add(TourishPlanInsertModel entityModel, String id)
    {
        var tourishPlan = new TourishPlan
        {
            TourName = entityModel.TourName,
            StartingPoint = entityModel.StartingPoint,
            EndPoint = entityModel.EndPoint,
            SupportNumber = entityModel.SupportNumber,

            // Description = entityModel.Description,

            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
        };

        #region Interest
        var tourishInterestList = new List<TourishInterest>();
        var tourishInterest = new TourishInterest();
        if (id != null)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id.ToString() == id);

            tourishInterest = new TourishInterest
            {
                InterestStatus = InterestStatus.Creator,
                User = user,
                TourishPlan = tourishPlan,
                UpdateDate = DateTime.UtcNow
            };

            tourishInterestList.Add(tourishInterest);

            tourishPlan.TourishInterestList = tourishInterestList;
        }

        tourishPlan.TourishInterestList = tourishInterestList;
        #endregion

        #region Interest
        var instructionList = new List<Instruction>();
        var instruction = new Instruction();
        if (id != null)
        {
            instructionList.Add(instruction);

            tourishPlan.InstructionList = instructionList;
        }
        #endregion



        if (entityModel.TourishCategoryRelations != null)
        {
            tourishPlan.TourishCategoryRelations = entityModel
                .TourishCategoryRelations.Select(entity => new TourishCategoryRelation
                {
                    TourishCategoryId = entity.TourishCategory.Id
                })
                .ToList();
        }

        if (entityModel.TourishScheduleList != null)
        {
            var tourishDataScheduleList = new List<TourishSchedule>();

            tourishPlan.TourishScheduleList = tourishDataScheduleList;
        }

        tourishPlan.InstructionList = initiateInstructionList();

        await _context.AddAsync(tourishPlan);
        await _context.SaveChangesAsync();
        await blobService.UploadStringBlobAsync(
            "tourish-content-container",
            entityModel.Description ?? "",
            "text/plain",
            tourishPlan.Id.ToString() ?? "" + ".txt"
        );

        if (!String.IsNullOrEmpty(entityModel.EatingScheduleString))
        {
            await AddEatSchedule(tourishPlan.Id, entityModel.EatingScheduleString);
        }

        if (!String.IsNullOrEmpty(entityModel.MovingScheduleString))
        {
            await AddMovingSchedule(tourishPlan.Id, entityModel.MovingScheduleString);
        }

        if (!String.IsNullOrEmpty(entityModel.StayingScheduleString))
        {
            await AddStayingSchedule(tourishPlan.Id, entityModel.StayingScheduleString);
        }

        return new Response
        {
            resultCd = 0,
            MessageCode = "I411",
            returnId = tourishPlan.Id,
            // Create type success
        };
    }

    private List<Instruction> initiateInstructionList()
    {
        var instructionList = new List<Instruction>();

        foreach (var instruction in priceInstructionLines)
        {
            instructionList.Add(
                new Instruction
                {
                    InstructionType = InstructionType.Price,
                    Description = instruction,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                }
            );
        }

        foreach (var instruction in cautionInstructionLines)
        {
            instructionList.Add(
                new Instruction
                {
                    InstructionType = InstructionType.Caution,
                    Description = instruction,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                }
            );
        }

        return instructionList;
    }

    public Response Delete(Guid id)
    {
        var entity = _context.TourishPlan.FirstOrDefault((entity => entity.Id == id));
        if (entity != null)
        {
            var movingScheduleList = _context.MovingSchedules.Where(entity => entity.TourishPlanId == id).ToList();
            _context.RemoveRange(movingScheduleList);
            var stayingScheduleList = _context.StayingSchedules.Where(entity => entity.TourishPlanId == id).ToList();
            _context.RemoveRange(stayingScheduleList);
            var eatingScheduleList = _context.EatSchedules.Where(entity => entity.TourishPlanId == id).ToList();
            _context.RemoveRange(eatingScheduleList);
            _context.SaveChanges();

            blobService.DeleteFileBlobAsync("tourish-content-container", entity.Id.ToString());
            _context.Remove(entity);
            _context.SaveChanges();
        }

        return new Response
        {
            resultCd = 0,
            MessageCode = "I413",
            // Delete type success
        };
    }

    public Response GetAll(
        string? search,
        string? category,
        string? categoryString,
        string? startingPoint,
        string? endPoint,
        string? startingDate,
        double? priceFrom,
        double? priceTo,
        string? sortBy,
        string? sortDirection,
        int page = 1,
        int pageSize = 5
    )
    {
        var entityQuery = _context
            .TourishPlan.Include(entity => entity.MovingSchedules)
            .Include(entity => entity.EatSchedules)
            .Include(entity => entity.StayingSchedules)
            .Include(entity => entity.TourishScheduleList)
            .Include(entity => entity.TourishCategoryRelations)
            .ThenInclude(entity => entity.TourishCategory)
            .AsSplitQuery();

        #region Filtering
        entityQuery = entityQuery.Where(entity => entity.TourishScheduleList.Count(entity1 => entity1.PlanStatus == PlanStatus.ConfirmInfo) >= 1);

        if (!string.IsNullOrEmpty(search))
        {
            entityQuery = entityQuery.Where(entity => entity.TourName.Contains(search));
        }

        if (!string.IsNullOrEmpty(category))
        {
            entityQuery = entityQuery.Where(entity =>
                entity.TourishCategoryRelations.Count(categoryRelation =>
                    (categoryRelation.TourishCategory != null)
                        ? categoryRelation.TourishCategory.Name.Contains(category)
                        : false
                ) >= 1
            );
        }

        if (!string.IsNullOrEmpty(categoryString))
        {
            string[] categoryArray = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(
                categoryString
            );

            if (categoryArray != null)
            {
                entityQuery = entityQuery.Where(entity =>
                    entity.TourishCategoryRelations.Count(categoryRelation =>
                        (categoryRelation.TourishCategory != null)
                            ? categoryArray.Contains(categoryRelation.TourishCategory.Name)
                            : false
                    ) >= 1
                );
            }
        }

        if (!string.IsNullOrEmpty(startingPoint))
        {
            entityQuery = entityQuery.Where(entity => entity.StartingPoint.Contains(startingPoint));
        }

        if (!string.IsNullOrEmpty(endPoint))
        {
            entityQuery = entityQuery.Where(entity => entity.EndPoint.Contains(endPoint));
        }

        if (priceFrom != null)
        {
            entityQuery = entityQuery.Where(entity =>
                (
                    entity.StayingSchedules.Sum(schedule => schedule.SinglePrice)
                    + entity.EatSchedules.Sum(schedule => schedule.SinglePrice)
                    + entity.MovingSchedules.Sum(schedule => schedule.SinglePrice)
                ) >= priceFrom
            );

            if (priceTo != null)
            {
                entityQuery = entityQuery.Where(entity =>
                    (
                        entity.StayingSchedules.Sum(schedule => schedule.SinglePrice)
                        + entity.EatSchedules.Sum(schedule => schedule.SinglePrice)
                        + entity.MovingSchedules.Sum(schedule => schedule.SinglePrice)
                    ) <= priceTo
                );
            }
        }

        if (!string.IsNullOrEmpty(startingDate))
        {
            // Mảng chứa các mẫu định dạng mà bạn cho phép
            string[] formats = { "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", "yyyy-MM-ddTHH:mm:sszzz" }; // Thêm các định dạng khác nếu cần
            DateTime dateTime;
            if (
                DateTime.TryParseExact(
                    startingDate,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateTime
                )
            )
            {
                logger.LogInformation(dateTime.ToString());
                entityQuery = entityQuery.Where(entity =>
                    entity.TourishScheduleList.Count(schedule =>
                        schedule.StartDate.Day == dateTime.Day
                        && schedule.StartDate.Month == dateTime.Month
                        && schedule.StartDate.Year == dateTime.Year
                    ) >= 1
                );
            }
        }

        #endregion

        #region Sorting
        entityQuery = entityQuery.OrderByColumnDescending("createDate");
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.Equals("totalTicketInMonth"))
            {
                var receiptList = _context
                .FullReceiptList.Include(entity => entity.TotalReceipt)
                .Include(entity => entity.TotalReceipt)
                .Include(entity => entity.TourishSchedule)
                .ThenInclude(entity => entity.TourishPlan)
                .Where(entity => (int)entity.Status < 3)
                .Where(entity =>
                    (
                        (entity.CreatedDate.Month == DateTime.UtcNow.Month || entity.CreatedDate.Month == DateTime.UtcNow.Month - 1)
                        && entity.CreatedDate.Year == DateTime.UtcNow.Year
                    )
                    ||
                    (
                        (entity.CompleteDate.Value.Month == DateTime.UtcNow.Month || entity.CompleteDate.Value.Month == DateTime.UtcNow.Month - 1)
                        && entity.CompleteDate.Value.Year == DateTime.UtcNow.Year
                    )
                )
                .GroupBy(entity => entity.TotalReceipt.TourishPlan.Id)
                .Select(group => new
                {
                    id = group.Key,
                    totalTicket = group.Sum(entity => entity.TotalTicket + entity.TotalChildTicket)
                })
                .OrderByDescending(group => group.totalTicket)
                .Take(pageSize)
                .AsSplitQuery();

                entityQuery = entityQuery.Where(entity => receiptList.Count(entity1 => entity1.id == entity.Id) >= 1);
            }
            else
            {
                entityQuery = entityQuery.OrderByColumn(sortBy);
                if (sortDirection == "desc")
                {
                    entityQuery = entityQuery.OrderByColumnDescending(sortBy);
                }
            }

        }
        #endregion

        #region Paging
        var result = PaginatorModel<TourishPlan>.Create(entityQuery, page, pageSize);
        #endregion

        var entityVM = new Response
        {
            resultCd = 0,
            Data = result.ToList(),
            count = result.TotalCount,
        };

        return entityVM;
    }

    public Response GetAllWithAuthority(
        string? search,
        string? category,
        string? categoryString,
        string? startingPoint,
        string? endPoint,
        string? startingDate,
        double? priceFrom,
        double? priceTo,
        string? sortBy,
        string? sortDirection,
        string? userId,
        int page = 1,
        int pageSize = 5
    )
    {
        var entityQuery = _context
            .TourishPlan
            .Include(entity => entity.TourishScheduleList)
            .Include(entity => entity.TourishInterestList)
            .Include(entity => entity.TourishCategoryRelations)
            .ThenInclude(entity => entity.TourishCategory)
            .AsSplitQuery();

        #region Filtering
        if (!string.IsNullOrEmpty(search))
        {
            entityQuery = entityQuery.Where(entity => entity.TourName.Contains(search));
        }

        if (!string.IsNullOrEmpty(userId))
        {
            // Lọc trên danh sách TourishInterestList của từng đối tượng TourishPlan
            foreach (var tourishPlan in entityQuery)
            {
                tourishPlan.TourishInterestList = tourishPlan
                    .TourishInterestList.Where(interest => interest.UserId.ToString() == userId)
                    .ToList();
            }
        }

        if (!string.IsNullOrEmpty(category))
        {
            entityQuery = entityQuery.Where(entity =>
                entity.TourishCategoryRelations.Count(categoryRelation =>
                    (categoryRelation.TourishCategory != null)
                        ? categoryRelation.TourishCategory.Name.Contains(category)
                        : false
                ) >= 1
            );
        }

        if (!string.IsNullOrEmpty(categoryString))
        {
            string[] categoryArray = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(
                categoryString
            );

            if (categoryArray != null)
            {
                entityQuery = entityQuery.Where(entity =>
                    entity.TourishCategoryRelations.Count(categoryRelation =>
                        (categoryRelation.TourishCategory != null)
                            ? categoryArray.Contains(categoryRelation.TourishCategory.Name)
                            : false
                    ) >= 1
                );
            }
        }

        if (!string.IsNullOrEmpty(startingPoint))
        {
            entityQuery = entityQuery.Where(entity => entity.StartingPoint.Contains(startingPoint));
        }

        if (!string.IsNullOrEmpty(endPoint))
        {
            entityQuery = entityQuery.Where(entity => entity.EndPoint.Contains(endPoint));
        }

        if (priceFrom != null)
        {
            entityQuery = entityQuery.Where(entity =>
                (
                    entity.StayingSchedules.Sum(schedule => schedule.SinglePrice)
                    + entity.EatSchedules.Sum(schedule => schedule.SinglePrice)
                    + entity.MovingSchedules.Sum(schedule => schedule.SinglePrice)
                ) >= priceFrom
            );

            if (priceTo != null)
            {
                entityQuery = entityQuery.Where(entity =>
                    (
                        entity.StayingSchedules.Sum(schedule => schedule.SinglePrice)
                        + entity.EatSchedules.Sum(schedule => schedule.SinglePrice)
                        + entity.MovingSchedules.Sum(schedule => schedule.SinglePrice)
                    ) <= priceTo
                );
            }
        }

        if (!string.IsNullOrEmpty(startingDate))
        {
            // Mảng chứa các mẫu định dạng mà bạn cho phép
            string[] formats = { "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", "yyyy-MM-ddTHH:mm:sszzz" }; // Thêm các định dạng khác nếu cần
            DateTime dateTime;
            if (
                DateTime.TryParseExact(
                    startingDate,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateTime
                )
            )
            {
                logger.LogInformation(dateTime.ToString());
                entityQuery = entityQuery.Where(entity =>
                    entity.TourishScheduleList.Count(schedule =>
                        schedule.StartDate.Day == dateTime.Day
                        && schedule.StartDate.Month == dateTime.Month
                        && schedule.StartDate.Year == dateTime.Year
                    ) >= 1
                );
            }
        }

        #endregion

        #region Sorting
        entityQuery = entityQuery.OrderByColumnDescending("createDate");

        if (!string.IsNullOrEmpty(sortBy))
        {
            entityQuery = entityQuery.OrderByColumn(sortBy);
            if (sortDirection == "desc")
            {
                entityQuery = entityQuery.OrderByColumnDescending(sortBy);
            }
        }
        #endregion

        #region Paging
        var result = PaginatorModel<TourishPlan>.Create(entityQuery, page, pageSize);
        #endregion

        var entityVM = new Response
        {
            resultCd = 0,
            Data = result.ToList(),
            count = result.TotalCount,
        };

        return entityVM;
    }

    public Response getById(Guid id)
    {
        var entity = _context
            .TourishPlan.Where(entity => entity.Id == id)
            .Include(entity => entity.EatSchedules)
            .Include(entity => entity.StayingSchedules)
            .Include(entity => entity.MovingSchedules)
            .Include(entity => entity.InstructionList)
            .Include(entity => entity.TourishScheduleList)
            .Include(entity => entity.TourishCategoryRelations)
            .ThenInclude(entity => entity.TourishCategory)
            .Include(entity => entity.TotalReceipt)
            .ThenInclude(entity => entity.FullReceiptList)
            .AsSplitQuery()
            .FirstOrDefault();
        if (entity == null)
        {
            return null;
        }

        return new Response { resultCd = 0, Data = entity };
    }

    public Response clientGetById(Guid id)
    {
        var entity = _context
            .TourishPlan.Where(entity => entity.Id == id)
            .Include(entity => entity.EatSchedules)
            .Include(entity => entity.StayingSchedules)
            .Include(entity => entity.MovingSchedules)
            .Include(entity => entity.InstructionList)
            .Include(entity => entity.TourishScheduleList)
            .Include(entity => entity.TourishCategoryRelations)
            .ThenInclude(entity => entity.TourishCategory)
            .AsSplitQuery()
            .FirstOrDefault();

        if (entity == null)
        {
            return null;
        }

        entity.TourishScheduleList = entity
            .TourishScheduleList.Where(entity => entity.PlanStatus == PlanStatus.ConfirmInfo)
            .ToList();

        return new Response { resultCd = 0, Data = entity };
    }

    public Response getByName(String TourName)
    {
        var entity = _context.TourishPlan.FirstOrDefault((entity => entity.TourName == TourName));

        return new Response { resultCd = 0, Data = entity };
    }

    public async Task<Response> Update(TourishPlanUpdateModel entityModel, string id)
    {
        List<string> propertyChangeList = new List<string>();
        List<string> scheduleChangeList = new List<string>();
        bool isNewScheduleAdded = false;

        var entity = await _context
            .TourishPlan
            .Include(e => e.EatSchedules)
            .Include(e => e.StayingSchedules)
            .Include(e => e.MovingSchedules)
            .Include(e => e.TourishInterestList)
            .FirstOrDefaultAsync(e => e.Id == entityModel.Id);

        if (entity == null)
        {
            return new Response { resultCd = 0, MessageCode = "C412" };
        }

        if (!string.IsNullOrEmpty(id))
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
            if (user != null)
            {
                if (entity.TourishInterestList == null)
                {
                    entity.TourishInterestList = new List<TourishInterest>();
                }

                if (!entity.TourishInterestList.Any(interest => interest.UserId.ToString() == id))
                {
                    entity.TourishInterestList.Add(new TourishInterest
                    {
                        InterestStatus = InterestStatus.Modifier,
                        User = user,
                        TourishPlan = entity,
                        UpdateDate = DateTime.UtcNow
                    });
                }
            }
        }

        if (entityModel.TourishCategoryRelations != null)
        {
            await _context
                .TourishCategoryRelations
                .Where(a => a.TourishPlanId == entityModel.Id)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
            entity.TourishCategoryRelations = entityModel.TourishCategoryRelations;
        }

        entity.TourName = entityModel.TourName ?? entity.TourName;
        entity.StartingPoint = entityModel.StartingPoint;
        entity.EndPoint = entityModel.EndPoint;
        entity.SupportNumber = entityModel.SupportNumber;

        if (entityModel.TourishScheduleList != null)
        {
            var tourishDataScheduleList = new List<TourishSchedule>();
            foreach (var item in entityModel.TourishScheduleList)
            {
                if (item.Id.HasValue)
                {
                    var existSchedule = await _context.TourishScheduleList.FirstOrDefaultAsync(e => e.Id == item.Id.Value);
                    if (existSchedule != null)
                    {
                        existSchedule.PlanStatus = item.PlanStatus;
                        existSchedule.StartDate = item.StartDate;
                        existSchedule.RemainTicket = item.RemainTicket;
                        existSchedule.TotalTicket = item.TotalTicket;
                        existSchedule.EndDate = item.EndDate;

                        var changedSchedule = GetChangedProperties(existSchedule);
                        if (changedSchedule.Any()) scheduleChangeList.Add(existSchedule.Id.ToString());

                        existSchedule.UpdateDate = DateTime.UtcNow;
                        tourishDataScheduleList.Add(existSchedule);
                    }
                }
                else
                {
                    isNewScheduleAdded = true;
                    tourishDataScheduleList.Add(new TourishSchedule
                    {
                        TourishPlanId = item.TourishPlanId,
                        PlanStatus = item.PlanStatus,
                        StartDate = item.StartDate,
                        RemainTicket = item.RemainTicket,
                        TotalTicket = item.TotalTicket,
                        EndDate = item.EndDate,
                        CreateDate = item.CreateDate ?? DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                    });
                }
            }

            entity.TourishScheduleList = tourishDataScheduleList;
        }

        if (entityModel.InstructionList != null)
        {
            var instructionList = new List<Instruction>();
            foreach (var item in entityModel.InstructionList)
            {
                instructionList.Add(new Instruction
                {
                    Id = item.Id.Value,
                    TourishPlanId = item.TourishPlanId,
                    Description = item.Description,
                    InstructionType = item.InstructionType,
                    CreateDate = item.CreateDate,
                    UpdateDate = DateTime.UtcNow,
                });
            }
            entity.InstructionList = instructionList;
        }

        var changedProperties = GetChangedProperties(entity);
        logger.LogInformation($"Change: {System.Text.Json.JsonSerializer.Serialize(changedProperties)}");
        foreach (var prop in changedProperties)
        {
            logger.LogInformation($"Property {prop} has been modified.");
        }

        propertyChangeList = changedProperties;

        entity.UpdateDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(entityModel.Description))
        {
            await blobService.UploadStringBlobAsync(
                "tourish-content-container",
                entityModel.Description,
                "text/plain",
                $"{entityModel.Id}.txt"
            );
        }

        List<string> updateService = new List<string>();

        if (!string.IsNullOrEmpty(entityModel.StayingScheduleString))
        {
            updateService.Add("staying");
            await _context.StayingSchedules.Where(a => a.TourishPlanId == entityModel.Id).ExecuteDeleteAsync();
            await AddStayingSchedule(entity.Id, entityModel.StayingScheduleString);
        }

        if (!string.IsNullOrEmpty(entityModel.MovingScheduleString))
        {
            updateService.Add("moving");
            await _context.MovingSchedules.Where(a => a.TourishPlanId == entityModel.Id).ExecuteDeleteAsync();
            await AddMovingSchedule(entity.Id, entityModel.MovingScheduleString);
        }

        if (!string.IsNullOrEmpty(entityModel.EatingScheduleString))
        {
            updateService.Add("eating");
            await _context.EatSchedules.Where(a => a.TourishPlanId == entityModel.Id).ExecuteDeleteAsync();
            await AddEatSchedule(entity.Id, entityModel.EatingScheduleString);
        }

        return new Response
        {
            resultCd = 0,
            MessageCode = "I412",
            Change = new Change
            {
                serviceChangeList = updateService,
                scheduleChangeList = scheduleChangeList,
                propertyChangeList = propertyChangeList,
                isNewScheduleAdded = isNewScheduleAdded
            }
        };
    }
    
    public async Task<string> getDescription(string containerName, string blobName)
    {
        logger.LogInformation("Description info: " + await blobService.GetBlobContentAsync(containerName, blobName + ".txt"));
        var content = await blobService.GetBlobContentAsync(containerName, blobName + ".txt");
        return content.Length <= 0 ?  await blobService.GetBlobContentAsync(containerName, blobName): content;
    }

    public async Task<bool> deleteDescription(string containerName, string blobName)
    {
        return await blobService.DeleteFileBlobAsync(containerName, blobName);
    }

    public async Task<List<TourishInterest>> getTourInterest(Guid id)
    {
        var entity = await _context
            .TourishPlan.Where(entity => entity.Id == id)
            .Include(tour => tour.TourishInterestList)
            .ThenInclude(entity => entity.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        if (entity == null)
        {
            return null;
        }

        return entity.TourishInterestList.ToList();
    }

    private async Task AddEatSchedule(Guid tourishPlanId, string FullScheduleString)
    {
        if (!String.IsNullOrEmpty(FullScheduleString))
        {
            dynamic scheduleArray = Newtonsoft.Json.JsonConvert.DeserializeObject(
                FullScheduleString
            );

            if (scheduleArray != null)
            {
                foreach (var schedule in scheduleArray)
                {
                    var eatSchedule = new EatSchedule
                    {
                        PlaceName = schedule.placeName,
                        SinglePrice = schedule.singlePrice,
                        Address = schedule.address,
                        SupportNumber = schedule.supportNumber,
                        // Description = schedule.description,
                        RestaurantId = schedule.restaurantId,
                        TourishPlanId = tourishPlanId,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                    };
                    await _context.AddAsync(eatSchedule);
                    await _context.SaveChangesAsync();

                    var insertString = (String)schedule.description ?? "";
                    ;
                    var oldId = (String)schedule.id;

                    if (oldId != null)
                    {
                        await blobService.RenameFileBlobAsync(
                            "eatschedule-content-container",
                            oldId,
                            eatSchedule.Id.ToString()
                        );

                        if (insertString.Length > 0)
                        {
                            await blobService.UploadStringBlobAsync(
                                "eatschedule-content-container",
                                (String)schedule.description ?? "Không có thông tin",
                                "text/plain",
                                eatSchedule.Id.ToString() ?? "" + ".txt"
                            );
                        }
                    }
                    else
                    {
                        await blobService.UploadStringBlobAsync(
                            "eatschedule-content-container",
                            (String)schedule.description ?? "Không có thông tin",
                            "text/plain",
                            eatSchedule.Id.ToString() ?? "" + ".txt"
                        );
                    }
                }
            }
        }
    }

    private async Task AddMovingSchedule(Guid tourishPlanId, string FullScheduleString)
    {
        if (!String.IsNullOrEmpty(FullScheduleString))
        {
            dynamic scheduleArray = Newtonsoft.Json.JsonConvert.DeserializeObject(
                FullScheduleString
            );

            if (scheduleArray != null)
            {
                foreach (var schedule in scheduleArray)
                {
                    var movingSchedule = new MovingSchedule
                    {
                        DriverName = schedule.driverName ?? "",
                        VehiclePlate = schedule.vehiclePlate,
                        BranchName = schedule.branchName,
                        VehicleType = schedule.vehicleType,
                        TransportId = schedule.transportId,
                        SinglePrice = schedule.singlePrice,
                        PhoneNumber = schedule.phoneNumber,
                        StartingPlace = schedule.startingPlace,
                        HeadingPlace = schedule.headingPlace,
                        TourishPlanId = tourishPlanId,
                        // Description = schedule.description,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                    };

                    await _context.AddAsync(movingSchedule);
                    await _context.SaveChangesAsync();

                    var insertString = (String)schedule.description ?? "";
                    var oldId = (String)schedule.id;

                    if (oldId != null)
                    {
                        await blobService.RenameFileBlobAsync(
                            "movingschedule-content-container",
                            oldId,
                            movingSchedule.Id.ToString()
                        );

                        if (insertString.Length > 0)
                        {
                            await blobService.UploadStringBlobAsync(
                                "movingschedule-content-container",
                                (String)schedule.description ?? "Không có thông tin",
                                "text/plain",
                                movingSchedule.Id.ToString() ?? "" + ".txt"
                            );
                        }
                    }
                    else
                    {
                        await blobService.UploadStringBlobAsync(
                            "movingschedule-content-container",
                            (String)schedule.description ?? "Không có thông tin",
                            "text/plain",
                            movingSchedule.Id.ToString() ?? "" + ".txt"
                        );
                    }
                }
            }
        }
    }

    private async Task AddStayingSchedule(Guid tourishPlanId, string FullScheduleString)
    {
        if (!String.IsNullOrEmpty(FullScheduleString))
        {
            dynamic scheduleArray = Newtonsoft.Json.JsonConvert.DeserializeObject(
                FullScheduleString
            );

            if (scheduleArray != null)
            {
                foreach (var schedule in scheduleArray)
                {
                    var stayingSchedule = new StayingSchedule
                    {
                        PlaceName = schedule.placeName,
                        Address = schedule.address,
                        SupportNumber = schedule.supportNumber,
                        SinglePrice = schedule.singlePrice,
                        RestHouseBranchId = schedule.restHouseBranchId,
                        RestHouseType = schedule.restHouseType,
                        // Description = schedule.description,
                        TourishPlanId = tourishPlanId,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                    };

                    await _context.AddAsync(stayingSchedule);
                    await _context.SaveChangesAsync();

                    var insertString = (String)schedule.description ?? "";
                    ;
                    var oldId = (String)schedule.id;

                    if (oldId != null)
                    {
                        await blobService.RenameFileBlobAsync(
                            "stayingschedule-content-container",
                            oldId,
                            stayingSchedule.Id.ToString()
                        );

                        if (insertString.Length > 0)
                        {
                            await blobService.UploadStringBlobAsync(
                                "stayingschedule-content-container",
                                (String)schedule.description ?? "Không có thông tin",
                                "text/plain",
                                stayingSchedule.Id.ToString() ?? "" + ".txt"
                            );
                        }
                    }
                    else
                    {
                        await blobService.UploadStringBlobAsync(
                            "stayingschedule-content-container",
                            (String)schedule.description ?? "Không có thông tin",
                            "text/plain",
                            stayingSchedule.Id.ToString() ?? "" + ".txt"
                        );
                    }
                }
            }
        }
    }

    private double GetTotalPrice(TourishPlan tourishPlan)
    {
        double totalPrice = 0;

        if (tourishPlan.StayingSchedules != null)
        {
            foreach (var entity in tourishPlan.StayingSchedules)
            {
                totalPrice += entity.SinglePrice ?? 0;
            }
        }

        if (tourishPlan.EatSchedules != null)
        {
            foreach (var entity in tourishPlan.EatSchedules)
            {
                totalPrice += entity.SinglePrice ?? 0;
            }
        }

        if (tourishPlan.MovingSchedules != null)
        {
            foreach (var entity in tourishPlan.MovingSchedules)
            {
                totalPrice += entity.SinglePrice ?? 0;
            }
        }

        return totalPrice;
    }

    public Response getTourInterest(Guid tourId, Guid userId)
    {
        var data = _context.TourishInterests.FirstOrDefault(entity =>
            entity.TourishPlanId == tourId && entity.UserId == userId
        );
        return new Response
        {
            resultCd = 0,
            Data = data,
            // Update type success
        };
    }

    public async Task<Response> setTourInterest(
        Guid tourId,
        Guid userId,
        InterestStatus interestStatus
    )
    {
        var existInterest = _context.TourishInterests.FirstOrDefault(entity =>
            entity.TourishPlanId == tourId && entity.UserId == userId
        );
        if (existInterest != null)
        {
            if (
                existInterest.InterestStatus == InterestStatus.Creator
                || existInterest.InterestStatus == InterestStatus.User
            )
                return new Response { resultCd = 1, MessageCode = "C416", };

            if (
                existInterest.InterestStatus == InterestStatus.Interest
                || existInterest.InterestStatus == InterestStatus.Modifier
            )
            {
                existInterest.InterestStatus = InterestStatus.NotInterested;
            }
            else if (existInterest.InterestStatus == InterestStatus.NotInterested)
            {
                existInterest.InterestStatus = InterestStatus.Interest;
            }

            existInterest.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            if (existInterest.InterestStatus == InterestStatus.NotInterested)
            {
                return new Response { resultCd = 0, MessageCode = "I416", };
            }
            return new Response { resultCd = 0, MessageCode = "I415", };
        }
        else
        {
            var insertValue = new TourishInterest
            {
                TourishPlanId = tourId,
                UserId = userId,
                InterestStatus = InterestStatus.Interest,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            _context.Add(insertValue);
            await _context.SaveChangesAsync();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I415",
                // Update type success
            };
        }
    }

    public Response getTopTourRating()
    {
        var entityList = _context
            .TourishPlan.Include(tour => tour.TourishRatingList)
            .Where(e => e.TourishRatingList.Count() > 10)
            .OrderByDescending(entity =>
                entity.TourishRatingList.Sum(e => e.Rating) / entity.TourishRatingList.Count()
            )
            .ThenByDescending(entity => entity.TourishRatingList.Count())
            .ToList();

        return new Response { resultCd = 0, Data = entityList };
    }

    public async Task<Boolean> checkArrangeScheduleFromUser(String email, Guid tourishPlanId, List<string> scheduleList)
    {
        if (scheduleList.Count > 0)
        {
            var count = await _context
                       .FullReceiptList.Include(entity => entity.TourishSchedule)
                       .Include(entity => entity.TotalReceipt)
                       .Where(entity =>
                           entity.Email == email
                           && entity.TotalReceipt.TourishPlanId == tourishPlanId
                           && entity.TourishSchedule.EndDate >= DateTime.UtcNow
                           && scheduleList.Any(entity1 => entity1 == entity.TourishSchedule.Id.ToString())
                       )
                       .CountAsync();

            if (count >= 1)
                return true;
        }
        else
        {
            var count = await _context
           .FullReceiptList.Include(entity => entity.TourishSchedule)
           .Include(entity => entity.TotalReceipt)
           .Where(entity =>
               entity.Email == email
               && entity.TotalReceipt.TourishPlanId == tourishPlanId
               && entity.TourishSchedule.EndDate >= DateTime.UtcNow
           )
           .CountAsync();

            if (count >= 1)
                return true;
        }

        return false;
    }

    private List<string> GetChangedProperties(object entity)
    {
        var entry = _context.Entry(entity);
        var changedProperties = new List<string>();

        foreach (var property in entry.Properties)
        {
            if (property.IsModified)
            {
                changedProperties.Add(property.Metadata.Name);
            }
        }

        return changedProperties;
    }
}
