using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TourishApi.Extension;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Data.Schedule;
using WebApplication1.Model;
using WebApplication1.Model.Schedule;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Service;

namespace WebApplication1.Repository.InheritanceRepo
{
    public class TourishOuterScheduleRepository
    {
        private readonly MyDbContext _context;
        private readonly IBlobService _blobService;
        public static int PAGE_SIZE { get; set; } = 5;

        private readonly string[] travelTicketPriceInstructionLines = new string[]
        {
            "Hộ chiếu phải còn thời hạn sử dụng trên 6 tháng, tính từ ngày khởi hành đi và về.",
            "Hành lý quá cước quy định sẽ bị tính thêm phí.",
            "Xe vận chuyển ngoài chương trình và các show về đêm không bao gồm trong giá vé."
        };

        private readonly string[] travelTicketCautionInstructionLines = new string[]
        {
            "Du khách Việt Kiều hoặc nước ngoài phải có visa tái nhập nhiều lần hoặc miễn thị thực 5 năm.",
            "Với du lịch nước ngoài, hộ chiếu phải mang theo bản gốc hợp lệ, không bị rạn, phai mờ, và còn thời hạn sử dụng trên 6 tháng (tính từ ngày khởi hành).",
            "Công ty du lịch không chịu trách nhiệm nếu Quý khách bị từ chối nhập cảnh với bất kỳ lý do nào từ hải quan nước ngoài.",
            "Công ty được phép thay đổi lịch trình chuyến đi và sử dụng các hãng hàng không thay thế, nhưng vẫn đảm bảo lộ trình.",
            "Trong trường hợp bất khả kháng như khủng bố, thiên tai hoặc thay đổi lịch trình của phương tiện công cộng (máy bay, tàu hỏa...), Công ty du lịch giữ quyền điều chỉnh lộ trình sao cho phù hợp và an toàn cho khách hàng, mà không phải chịu trách nhiệm bồi thường thiệt hại phát sinh."
        };

        private readonly string[] bookingPriceInstructionLines = new string[]
        {
            "Điện thoại, giặt ủi, nước uống trong phòng khách sạn và các chi phí cá nhân khác không bao gồm trong giá phòng.",
            "Hành lý quá cước quy định có thể bị tính thêm phí."
        };

        private readonly string[] bookingCautionInstructionLines = new string[]
        {
            "Trẻ em dưới 16 tuổi phải có bố mẹ đi cùng hoặc người được ủy quyền có giấy ủy quyền từ bố mẹ.",
            "Với du lịch nước ngoài: Không sử dụng thẻ xanh. Nếu sử dụng Sổ Du lịch (yêu cầu visa nước nhập cảnh), vui lòng thông báo cho nhân viên nhận tour nếu Quý khách sử dụng các hồ sơ khác ngoài hộ chiếu.",
            "Thứ tự điểm tham quan và lộ trình có thể thay đổi tùy theo tình hình thực tế, nhưng vẫn đảm bảo đầy đủ các điểm tham quan như ban đầu.",
            "Công ty được phép thay đổi lịch trình chuyến đi và sử dụng các hãng hàng không thay thế, nhưng vẫn đảm bảo tham quan đầy đủ các điểm theo chương trình.",
            "Trong trường hợp bất khả kháng như khủng bố, thiên tai hoặc thay đổi lịch trình của phương tiện công cộng (máy bay, tàu hỏa...), Công ty du lịch giữ quyền điều chỉnh lộ trình tour cho phù hợp và an toàn cho khách hàng, mà không phải chịu trách nhiệm bồi thường thiệt hại phát sinh."
        };

        public TourishOuterScheduleRepository(MyDbContext _context, IBlobService blobService)
        {
            this._context = _context;
            _blobService = blobService;
        }

        public async Task<Response> AddEatSchedule(EatScheduleModel addModel)
        {
            var addValue = new EatSchedule
            {
                Name = addModel.Name,
                PlaceName = addModel.PlaceName,
                RestaurantId = addModel.RestaurantId,
                SinglePrice = addModel.SinglePrice,
                Address = addModel.Address,
                SupportNumber = addModel.SupportNumber,
                TourishPlanId = null,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            await _context.AddAsync(addValue);
            await _context.SaveChangesAsync();

            await _blobService.UploadStringBlobAsync(
                "eatschedule-content-container",
                addModel.Description ?? "Không có thông tin",
                "text/plain",
                addValue.Id.ToString() ?? "" + ".txt"
            );

            return new Response
            {
                resultCd = 0,
                MessageCode = "I431",
                returnId = addValue.Id
                // Create type success
            };
        }

        public async Task<Response> AddMovingSchedule(string userId, MovingScheduleModel addModel)
        {
            var addValue = new MovingSchedule
            {
                Name = addModel.Name,
                BranchName = addModel.BranchName,
                HeadingPlace = addModel.HeadingPlace,
                StartingPlace = addModel.StartingPlace,
                TransportId = addModel.TransportId,
                VehicleType = addModel.VehicleType,
                SinglePrice = addModel.SinglePrice,
                DriverName = addModel.DriverName,
                VehiclePlate = addModel.VehiclePlate,
                PhoneNumber = addModel.PhoneNumber,
                TourishPlanId = null,
                ServiceScheduleList = null,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            if (addModel.ServiceScheduleList != null)
            {
                var serviceDataScheduleList = new List<ServiceSchedule>();
                foreach (var item in addModel.ServiceScheduleList)
                {
                    serviceDataScheduleList.Add(
                        new ServiceSchedule
                        {
                            MovingScheduleId = item.MovingScheduleId,
                            StayingScheduleId = item.StayingScheduleId,
                            RemainTicket = item.RemainTicket,
                            TotalTicket = item.TotalTicket,
                            Status = item.Status,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        }
                    );
                }
                addValue.ServiceScheduleList = serviceDataScheduleList;
            }

            addValue.InstructionList = initiateMovingInstructionList();

            var scheduleInterest = new ScheduleInterest();
            var scheduleInterestList = new List<ScheduleInterest>();

            if (userId != null)
            {
                var user = _context.Users.SingleOrDefault(u => u.Id.ToString() == userId);

                scheduleInterest = new ScheduleInterest
                {
                    InterestStatus = InterestStatus.Creator,
                    User = user,
                    MovingSchedule = addValue,
                    UpdateDate = DateTime.UtcNow
                };

                scheduleInterestList.Add(scheduleInterest);

                addValue.ScheduleInterestList = scheduleInterestList;
            }

            addValue.ScheduleInterestList = scheduleInterestList;

            await _context.AddAsync(addValue);
            await _context.SaveChangesAsync();

            await _blobService.UploadStringBlobAsync(
                "movingschedule-content-container",
                addModel.Description ?? "Không có thông tin",
                "text/plain",
                addValue.Id.ToString() ?? "" + ".txt"
            );

            return new Response
            {
                resultCd = 0,
                MessageCode = "I431",
                returnId = addValue.Id
                // Create type success
            };
        }

        public async Task<Response> AddStayingSchedule(string userId, StayingScheduleModel addModel)
        {
            var addValue = new StayingSchedule
            {
                Name = addModel.Name,
                PlaceName = addModel.PlaceName,
                RestHouseBranchId = addModel.RestHouseBranchId,
                RestHouseType = addModel.RestHouseType,
                SinglePrice = addModel.SinglePrice,
                Address = addModel.Address,
                SupportNumber = addModel.SupportNumber,
                TourishPlanId = null,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            var scheduleInterest = new ScheduleInterest();
            var scheduleInterestList = new List<ScheduleInterest>();

            if (addModel.ServiceScheduleList != null)
            {
                var serviceDataScheduleList = new List<ServiceSchedule>();
                foreach (var item in addModel.ServiceScheduleList)
                {
                    serviceDataScheduleList.Add(
                        new ServiceSchedule
                        {
                            MovingScheduleId = item.MovingScheduleId,
                            StayingScheduleId = item.StayingScheduleId,
                            Status = item.Status,
                            StartDate = item.StartDate,
                            RemainTicket = item.RemainTicket,
                            TotalTicket = item.TotalTicket,
                            EndDate = item.EndDate,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        }
                    );
                }
                addValue.ServiceScheduleList = serviceDataScheduleList;
            }

            if (userId != null)
            {
                var user = _context.Users.SingleOrDefault(u => u.Id.ToString() == userId);

                scheduleInterest = new ScheduleInterest
                {
                    InterestStatus = InterestStatus.Creator,
                    User = user,
                    StayingSchedule = addValue,
                    UpdateDate = DateTime.UtcNow
                };

                scheduleInterestList.Add(scheduleInterest);

                addValue.ScheduleInterestList = scheduleInterestList;
            }

            addValue.InstructionList = initiateStayingInstructionList();
            addValue.ScheduleInterestList = scheduleInterestList;

            await _context.AddAsync(addValue);
            await _context.SaveChangesAsync();

            await _blobService.UploadStringBlobAsync(
                "stayingschedule-content-container",
                addModel.Description ?? "Không có thông tin",
                "text/plain",
                addValue.Id.ToString() ?? "" + ".txt"
            );

            return new Response
            {
                resultCd = 0,
                MessageCode = "I431",
                returnId = addValue.Id
                // Create type success
            };
        }

        public async Task<Response> DeleteEatSchedule(Guid id)
        {
            var deleteEntity = _context.EatSchedules.FirstOrDefault((entity => entity.Id == id));
            if (deleteEntity != null)
            {
                await _blobService.DeleteFileBlobAsync(
                    "eatschedule-content-container",
                    deleteEntity.Id.ToString()
                );
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I433",
                // Delete type success
            };
        }

        public async Task<Response> DeleteMovingSchedule(Guid id)
        {
            var deleteEntity = _context.MovingSchedules.FirstOrDefault((entity => entity.Id == id));
            if (deleteEntity != null)
            {
                await _blobService.DeleteFileBlobAsync(
                    "movingschedule-content-container",
                    deleteEntity.Id.ToString()
                );
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I433",
                // Delete type success
            };
        }

        public async Task<Response> DeleteStayingSchedule(Guid id)
        {
            var deleteEntity = _context.StayingSchedules.FirstOrDefault(
                (entity => entity.Id == id)
            );
            if (deleteEntity != null)
            {
                await _blobService.DeleteFileBlobAsync(
                    "stayingschedule-content-container",
                    deleteEntity.Id.ToString()
                );
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I433",
                // Delete type success
            };
        }

        public Response GetAllEatSchedule(
            string? search,
            int? type,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            var entityQuery = _context.EatSchedules.AsQueryable();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlan == null);
            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.PlaceName.Contains(search));
            }
            #endregion

            #region Sorting
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
            var result = PaginatorModel<EatSchedule>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response GetAllMovingSchedule(
            string? search,
            int? type,
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
                .MovingSchedules.Include(entity => entity.InstructionList)
                .Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.ScheduleInterestList)
                .AsSplitQuery();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlan == null);
            entityQuery = entityQuery.Where(entity => entity.ServiceScheduleList.Count(entity1 => entity1.Status == ScheduleStatus.ConfirmInfo) >= 1);

            if (!string.IsNullOrEmpty(endPoint))
            {
                entityQuery = entityQuery.Where(entity => entity.HeadingPlace.Contains(endPoint));
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

                    entityQuery = entityQuery.Where(entity =>
                        entity.ServiceScheduleList.Count(schedule =>
                            schedule.StartDate.Day == dateTime.Day
                            && schedule.StartDate.Month == dateTime.Month
                            && schedule.StartDate.Year == dateTime.Year
                        ) >= 1
                    );
                }
            }

            if (type != null) entityQuery = entityQuery.Where(entity => (int)entity.VehicleType == type);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Name.Contains(search));
            }

            if (priceFrom != null)
            {
                entityQuery = entityQuery.Where(entity => (entity.SinglePrice) >= priceFrom);

                if (priceTo != null)
                {
                    entityQuery = entityQuery.Where(entity => (entity.SinglePrice) <= priceTo);
                }
            }
            #endregion

            #region Sorting
            entityQuery = entityQuery.OrderByDescending(entity => entity.CreateDate);
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
            var result = PaginatorModel<MovingSchedule>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response GetAllStayingSchedule(
            string? search,
            int? type,
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
                .StayingSchedules.Include(entity => entity.InstructionList)
                .Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.ScheduleInterestList)
                .AsSplitQuery();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlanId == null);

            if (!string.IsNullOrEmpty(endPoint))
            {
                entityQuery = entityQuery.Where(entity => entity.Address.Contains(endPoint));
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

                    entityQuery = entityQuery.Where(entity =>
                        entity.ServiceScheduleList.Count(schedule =>
                            schedule.StartDate.Day == dateTime.Day
                            && schedule.StartDate.Month == dateTime.Month
                            && schedule.StartDate.Year == dateTime.Year
                        ) >= 1
                    );
                }
            }

            entityQuery = entityQuery.Where(entity => entity.ServiceScheduleList.Count(entity1 => entity1.Status == ScheduleStatus.ConfirmInfo) >= 1);

            if (type != null) entityQuery = entityQuery.Where(entity => (int)entity.RestHouseType == type);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Name.Contains(search));
            }

            if (priceFrom != null)
            {
                entityQuery = entityQuery.Where(entity => (entity.SinglePrice) >= priceFrom);

                if (priceTo != null)
                {
                    entityQuery = entityQuery.Where(entity => (entity.SinglePrice) <= priceTo);
                }
            }
            #endregion

            #region Sorting
            entityQuery = entityQuery.OrderByDescending(entity => entity.CreateDate);
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
            var result = PaginatorModel<StayingSchedule>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response GetAllMovingScheduleWithAuthority(
            string? search,
            int? type,
            string? sortBy,
            string? sortDirection,
            string? userId,
            int page = 1,
            int pageSize = 5
        )
        {
            var entityQuery = _context
                .MovingSchedules.Include(entity => entity.InstructionList)
                .Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.ScheduleInterestList)
                .AsQueryable();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlan == null);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Name.Contains(search));
            }

            if (type != null) entityQuery = entityQuery.Where(entity => (int)entity.VehicleType == type);

            if (!string.IsNullOrEmpty(userId))
            {
                foreach (var schedule in entityQuery)
                {
                    schedule.ScheduleInterestList = schedule
                        .ScheduleInterestList.Where(interest =>
                            interest.UserId.ToString() == userId
                        )
                        .ToList();
                }
            }
            #endregion

            #region Sorting
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
            var result = PaginatorModel<MovingSchedule>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response GetAllStayingScheduleWithAuthority(
            string? search,
            int? type,
            string? sortBy,
            string? sortDirection,
            string? userId,
            int page = 1,
            int pageSize = 5
        )
        {
            var entityQuery = _context
                .StayingSchedules.Include(entity => entity.InstructionList)
                .Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.ScheduleInterestList)
                .AsQueryable();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlan == null);
            if (type != null) entityQuery = entityQuery.Where(entity => (int)entity.RestHouseType == type);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Name.Contains(search));
            }
            if (!string.IsNullOrEmpty(userId))
            {
                // Lọc trên danh sách TourishInterestList của từng đối tượng TourishPlan
                foreach (var schedule in entityQuery)
                {
                    schedule.ScheduleInterestList = schedule
                        .ScheduleInterestList.Where(interest =>
                            interest.UserId.ToString() == userId
                        )
                        .ToList();
                }
            }
            #endregion

            #region Sorting
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
            var result = PaginatorModel<StayingSchedule>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response getByEatScheduleId(Guid id)
        {
            var entity = _context.EatSchedules.FirstOrDefault((entity => entity.Id == id));

            return new Response { resultCd = 0, Data = entity };
        }

        public Response getByNameEatSchedule(string name)
        {
            var entity = _context.EatSchedules.FirstOrDefault((entity => entity.PlaceName == name));

            return new Response { resultCd = 0, Data = entity };
        }

        public async Task<Response> UpdateEatSchedule(EatScheduleModel entityModel)
        {
            var entity = _context.EatSchedules.FirstOrDefault(
                (entity => entity.Id == entityModel.Id)
            );
            if (entity != null)
            {
                entity.Name = entityModel.Name;
                entity.UpdateDate = DateTime.UtcNow;
                entity.PlaceName = entityModel.PlaceName;
                entity.RestaurantId = entityModel.RestaurantId;
                entity.SinglePrice = entityModel.SinglePrice;
                entity.SupportNumber = entityModel.SupportNumber;
                entity.Address = entityModel.Address;

                _context.SaveChanges();

                if (!String.IsNullOrEmpty(entityModel.Description))
                    await _blobService.UploadStringBlobAsync(
                        "eatschedule-content-container",
                        entityModel.Description ?? "Không có thông tin",
                        "text/plain",
                        entity.Id.ToString() ?? "" + ".txt"
                    );
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I432",
                returnId = entityModel.Id
                // Update type success
            };
        }

        public Response getByStayingScheduleId(Guid id)
        {
            var entity = _context
                .StayingSchedules.Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.InstructionList)
                .AsSplitQuery()
                .FirstOrDefault((entity => entity.Id == id));

            return new Response { resultCd = 0, Data = entity };
        }

        public Response clientGetByStayingScheduleId(Guid id)
        {
            var entity = _context
                .StayingSchedules.Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.InstructionList)
                .AsSplitQuery()
                .FirstOrDefault((entity => entity.Id == id));

            entity.ServiceScheduleList = entity
                .ServiceScheduleList.Where(entity => entity.Status == ScheduleStatus.ConfirmInfo)
                .ToList();

            return new Response { resultCd = 0, Data = entity };
        }

        public Response getByNameStayingSchedule(string name)
        {
            var entity = _context.StayingSchedules.FirstOrDefault(
                (entity => entity.PlaceName == name)
            );

            return new Response { resultCd = 0, Data = entity };
        }

        public async Task<Response> UpdateStayingSchedule(
            string userId,
            StayingScheduleModel entityModel
        )
        {
            List<string> propertyChangeList = new List<string>();
            List<string> scheduleChangeList = new List<string>();
            var isNewScheduleAdded = false;

            var entity = _context
                .StayingSchedules.Include(entity => entity.ScheduleInterestList)
                .Include(entity => entity.InstructionList)
                .FirstOrDefault((entity => entity.Id == entityModel.Id));
            if (entity != null)
            {
                var scheduleInterest = new ScheduleInterest();
                if (userId != null)
                {
                    var user = _context.Users.SingleOrDefault(u => u.Id.ToString() == userId);

                    if (user != null)
                    {
                        scheduleInterest = new ScheduleInterest
                        {
                            InterestStatus = InterestStatus.Modifier,
                            User = user,
                            StayingSchedule = entity,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        };

                        if (entity.ScheduleInterestList == null)
                        {
                            entity.ScheduleInterestList = new List<ScheduleInterest>();
                        }

                        if (
                            entity.ScheduleInterestList.Count(interest =>
                                interest.UserId.ToString() == userId
                            ) <= 0
                        )
                        {
                            entity.ScheduleInterestList.Add(scheduleInterest);
                        }
                    }
                }

                entity.Name = entityModel.Name;
                entity.PlaceName = entityModel.PlaceName;
                entity.SupportNumber = entityModel.SupportNumber;
                entity.Address = entityModel.Address;
                entity.SinglePrice = entityModel.SinglePrice;
                entity.RestHouseBranchId = entityModel.RestHouseBranchId;
                entity.RestHouseType = entityModel.RestHouseType;

                if (
                    entityModel.InstructionList != null
                    && (entity.InstructionList ?? new List<Instruction>()).Count > 0
                )
                {
                    var instructionList = new List<Instruction>();

                    foreach (var item in entityModel.InstructionList)
                    {
                        var existingInstruction = _context.Instructions.FirstOrDefault(i =>
                            i.Id == item.Id.Value
                        );

                        if (existingInstruction != null)
                        {
                            // If the instruction already exists in the context, update its properties
                            existingInstruction.StayingScheduleId = entity.Id;
                            existingInstruction.MovingScheduleId = null;
                            existingInstruction.Description = item.Description;
                            existingInstruction.InstructionType = item.InstructionType;
                            existingInstruction.CreateDate = item.CreateDate;
                            existingInstruction.UpdateDate = DateTime.UtcNow;

                            instructionList.Add(existingInstruction);
                        }
                        else
                        {
                            // If the instruction is not already in the context, create a new instance
                            var newInstruction = new Instruction
                            {
                                Id = item.Id.Value,
                                StayingScheduleId = entity.Id,
                                MovingScheduleId = null,
                                Description = item.Description,
                                InstructionType = item.InstructionType,
                                CreateDate = item.CreateDate,
                                UpdateDate = DateTime.UtcNow,
                            };

                            instructionList.Add(newInstruction);
                        }
                    }

                    entity.InstructionList = instructionList;
                }
                else
                {
                    entity.InstructionList = initiateMovingInstructionList();
                }

                if (entityModel.ServiceScheduleList != null)
                {
                    var dataScheduleList = new List<ServiceSchedule>();
                    foreach (var item in entityModel.ServiceScheduleList)
                    {
                        if (item.Id.HasValue)
                        {
                            var existSchedule = await _context.ServiceSchedule.FirstOrDefaultAsync(
                                e => e.Id == item.Id.Value
                            );
                            if (existSchedule != null)
                            {
                                existSchedule.MovingScheduleId = item.MovingScheduleId;
                                existSchedule.StayingScheduleId = item.StayingScheduleId;
                                existSchedule.Status = item.Status;
                                existSchedule.StartDate = item.StartDate;
                                existSchedule.RemainTicket = item.RemainTicket;
                                existSchedule.TotalTicket = item.TotalTicket;
                                existSchedule.EndDate = item.EndDate;

                                var changedSchedule = GetChangedProperties(existSchedule);
                                if (changedSchedule.Any())
                                    scheduleChangeList.Add(existSchedule.Id.ToString());

                                existSchedule.UpdateDate = DateTime.UtcNow;
                                dataScheduleList.Add(existSchedule);
                            }
                        }
                        else
                        {
                            isNewScheduleAdded = true;
                            dataScheduleList.Add(
                                new ServiceSchedule
                                {
                                    MovingScheduleId = item.MovingScheduleId ?? null,
                                    StayingScheduleId = item.StayingScheduleId ?? null,
                                    Status = item.Status,
                                    RemainTicket = item.RemainTicket,
                                    TotalTicket = item.TotalTicket,
                                    StartDate = item.StartDate,
                                    EndDate = item.EndDate,
                                    CreateDate = item.CreateDate ?? DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                }
                            );
                        }
                    }
                    entity.ServiceScheduleList = dataScheduleList;
                }

                var changedProperties = GetChangedProperties(entity);
                propertyChangeList = changedProperties;

                entity.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                if (!String.IsNullOrEmpty(entityModel.Description))
                    await _blobService.UploadStringBlobAsync(
                        "stayingschedule-content-container",
                        entityModel.Description ?? "Không có thông tin",
                        "text/plain",
                        entity.Id.ToString() ?? "" + ".txt"
                    );
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I432",
                returnId = entityModel.Id,
                Change = new Change
                {
                    scheduleChangeList = scheduleChangeList,
                    propertyChangeList = propertyChangeList,
                    isNewScheduleAdded = isNewScheduleAdded
                }
            };
        }

        public Response getByMovingScheduleId(Guid id)
        {
            var entity = _context
                .MovingSchedules.Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.InstructionList)
                .AsSplitQuery()
                .FirstOrDefault((entity => entity.Id == id));

            return new Response { resultCd = 0, Data = entity };
        }

        public Response clientGetByMovingScheduleId(Guid id)
        {
            var entity = _context
                .MovingSchedules.Include(entity => entity.ServiceScheduleList)
                .Include(entity => entity.InstructionList)
                .AsSplitQuery()
                .FirstOrDefault((entity => entity.Id == id));

            entity.ServiceScheduleList = entity
                .ServiceScheduleList.Where(entity => entity.Status == ScheduleStatus.ConfirmInfo)
                .ToList();

            return new Response { resultCd = 0, Data = entity };
        }

        public Response getByNameMovingSchedule(string name)
        {
            var entity = _context.MovingSchedules.FirstOrDefault((entity => entity.Name == name));

            return new Response { resultCd = 0, Data = entity };
        }

        public async Task<Response> UpdateMovingSchedule(
            string userId,
            MovingScheduleModel entityModel
        )
        {
            List<string> propertyChangeList = new List<string>();
            List<string> scheduleChangeList = new List<string>();
            var isNewScheduleAdded = false;

            var entity = await _context
                .MovingSchedules.Include(entity => entity.ScheduleInterestList)
                .Include(entity => entity.InstructionList)
                .FirstOrDefaultAsync((entity => entity.Id == entityModel.Id));
            if (entity != null)
            {
                var scheduleInterest = new ScheduleInterest();
                if (userId != null)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u =>
                        u.Id.ToString() == userId
                    );

                    if (user != null)
                    {
                        scheduleInterest = new ScheduleInterest
                        {
                            InterestStatus = InterestStatus.Modifier,
                            User = user,
                            MovingSchedule = entity,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        };

                        if (entity.ScheduleInterestList == null)
                        {
                            entity.ScheduleInterestList = new List<ScheduleInterest>();
                        }

                        if (
                            entity.ScheduleInterestList.Count(interest =>
                                interest.UserId.ToString() == userId
                            ) <= 0
                        )
                        {
                            entity.ScheduleInterestList.Add(scheduleInterest);
                        }
                    }
                }

                entity.Name = entityModel.Name;
                entity.BranchName = entityModel.BranchName;
                entity.PhoneNumber = entityModel.PhoneNumber;
                entity.TransportId = entityModel.TransportId;
                entity.VehiclePlate = entityModel.VehiclePlate;
                entity.VehicleType = entityModel.VehicleType;
                entity.SinglePrice = entityModel.SinglePrice;
                entity.DriverName = entityModel.DriverName;
                entity.StartingPlace = entityModel.StartingPlace;
                entity.HeadingPlace = entityModel.HeadingPlace;

                if (
                    entityModel.InstructionList != null
                    && (entity.InstructionList ?? new List<Instruction>()).Count > 0
                )
                {
                    var instructionList = new List<Instruction>();

                    foreach (var item in entityModel.InstructionList)
                    {
                        var existingInstruction = _context.Instructions.FirstOrDefault(i =>
                            i.Id == item.Id.Value
                        );

                        if (existingInstruction != null)
                        {
                            // If the instruction already exists in the context, update its properties
                            existingInstruction.StayingScheduleId = null;
                            existingInstruction.MovingScheduleId = entity.Id;
                            existingInstruction.Description = item.Description;
                            existingInstruction.InstructionType = item.InstructionType;
                            existingInstruction.CreateDate = item.CreateDate;
                            existingInstruction.UpdateDate = DateTime.UtcNow;

                            instructionList.Add(existingInstruction);
                        }
                        else
                        {
                            // If the instruction is not already in the context, create a new instance
                            var newInstruction = new Instruction
                            {
                                Id = item.Id.Value,
                                StayingScheduleId = null,
                                MovingScheduleId = entity.Id,
                                Description = item.Description,
                                InstructionType = item.InstructionType,
                                CreateDate = item.CreateDate,
                                UpdateDate = DateTime.UtcNow,
                            };

                            instructionList.Add(newInstruction);
                        }
                    }

                    entity.InstructionList = instructionList;
                }
                else
                {
                    entity.InstructionList = initiateMovingInstructionList();
                }

                if (entityModel.ServiceScheduleList != null)
                {
                    var dataScheduleList = new List<ServiceSchedule>();
                    foreach (var item in entityModel.ServiceScheduleList)
                    {
                        if (item.Id.HasValue)
                        {
                            var existSchedule = await _context.ServiceSchedule.FirstOrDefaultAsync(
                                e => e.Id == item.Id.Value
                            );
                            if (existSchedule != null)
                            {
                                existSchedule.MovingScheduleId = item.MovingScheduleId;
                                existSchedule.StayingScheduleId = item.StayingScheduleId;
                                existSchedule.Status = item.Status;
                                existSchedule.StartDate = item.StartDate;
                                existSchedule.RemainTicket = item.RemainTicket;
                                existSchedule.TotalTicket = item.TotalTicket;
                                existSchedule.EndDate = item.EndDate;

                                var changedSchedule = GetChangedProperties(existSchedule);
                                if (changedSchedule.Any())
                                    scheduleChangeList.Add(existSchedule.Id.ToString());

                                existSchedule.UpdateDate = DateTime.UtcNow;
                                dataScheduleList.Add(existSchedule);
                            }
                        }
                        else
                        {
                            isNewScheduleAdded = true;
                            dataScheduleList.Add(
                                new ServiceSchedule
                                {
                                    MovingScheduleId = item.MovingScheduleId ?? null,
                                    StayingScheduleId = item.StayingScheduleId ?? null,
                                    Status = item.Status,
                                    RemainTicket = item.RemainTicket,
                                    TotalTicket = item.TotalTicket,
                                    StartDate = item.StartDate,
                                    EndDate = item.EndDate,
                                    CreateDate = item.CreateDate ?? DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                }
                            );
                        }
                    }
                    entity.ServiceScheduleList = dataScheduleList;
                }

                var changedProperties = GetChangedProperties(entity);
                propertyChangeList = changedProperties;

                entity.UpdateDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                if (!String.IsNullOrEmpty(entityModel.Description))
                    await _blobService.UploadStringBlobAsync(
                        "movingschedule-content-container",
                        entityModel.Description ?? "Không có thông tin",
                        "text/plain",
                        entity.Id.ToString() ?? "" + ".txt"
                    );
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I432",
                returnId = entityModel.Id,
                Change = new Change
                {
                    scheduleChangeList = scheduleChangeList,
                    propertyChangeList = propertyChangeList,
                    isNewScheduleAdded = isNewScheduleAdded
                }
                // Update type success
            };
        }

        public async Task<List<ScheduleInterest>> getScheduleInterest(
            Guid id,
            ScheduleType scheduleType
        )
        {
            if (scheduleType == ScheduleType.MovingSchedule)
            {
                var entity = await _context
                    .MovingSchedules.Where(entity => entity.Id == id)
                    .Include(tour => tour.ScheduleInterestList)
                    .ThenInclude(entity => entity.User)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return null;
                }

                return entity.ScheduleInterestList.ToList();
            }
            else if (scheduleType == ScheduleType.StayingSchedule)
            {
                var entity = await _context
                    .StayingSchedules.Where(entity => entity.Id == id)
                    .Include(tour => tour.ScheduleInterestList)
                    .ThenInclude(entity => entity.User)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return null;
                }

                return entity.ScheduleInterestList.ToList();
            }

            return new List<ScheduleInterest>();
        }

        public Response getScheduleInterest(Guid scheduleId, ScheduleType scheduleType, Guid userId)
        {
            if (scheduleType == ScheduleType.MovingSchedule)
            {
                var data = _context.ScheduleInterests.FirstOrDefault(entity =>
                    entity.MovingScheduleId == scheduleId && entity.UserId == userId
                );
                return new Response
                {
                    resultCd = 0,
                    Data = data,
                    // Update type success
                };
            }
            else if (scheduleType == ScheduleType.StayingSchedule)
            {
                var data = _context.ScheduleInterests.FirstOrDefault(entity =>
                    entity.StayingScheduleId == scheduleId && entity.UserId == userId
                );
                return new Response
                {
                    resultCd = 0,
                    Data = data,
                    // Update type success
                };
            }

            return new Response
            {
                resultCd = 1,
                Data = null,
                MessageCode = "C430"
                // Update type success
            };
        }

        public async Task<Response> setScheduleInterest(
            Guid scheduleId,
            ScheduleType scheduleType,
            Guid userId,
            InterestStatus interestStatus
        )
        {
            if (scheduleType == ScheduleType.MovingSchedule)
            {
                var existInterest = _context.ScheduleInterests.FirstOrDefault(entity =>
                    entity.MovingScheduleId == scheduleId && entity.UserId == userId
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
                    var insertValue = new ScheduleInterest
                    {
                        MovingScheduleId = scheduleId,
                        UserId = userId,
                        InterestStatus = interestStatus,
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
            else if (scheduleType == ScheduleType.StayingSchedule)
            {
                var existInterest = _context.ScheduleInterests.FirstOrDefault(entity =>
                    entity.StayingScheduleId == scheduleId && entity.UserId == userId
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
                    var insertValue = new ScheduleInterest
                    {
                        StayingScheduleId = scheduleId,
                        UserId = userId,
                        InterestStatus = interestStatus,
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

            return new Response
            {
                resultCd = 1,
                MessageCode = "C434",
                // Update type success
            };
        }

        public async Task<Response> UpdateInstructionList(
            ScheduleInstructionModel scheduleInstructionModel
        )
        {
            if (scheduleInstructionModel.ScheduleType == ScheduleType.MovingSchedule)
            {
                var existSchedule = _context.MovingSchedules.FirstOrDefault(entity =>
                    entity.Id == scheduleInstructionModel.ScheduleId
                );
                if (existSchedule != null)
                {
                    if (scheduleInstructionModel.InstructionList != null)
                    {
                        var instructionList = new List<Instruction>();
                        foreach (var item in scheduleInstructionModel.InstructionList)
                        {
                            instructionList.Add(
                                new Instruction
                                {
                                    Id = item.Id.Value,
                                    TourishPlanId = item.TourishPlanId,
                                    Description = item.Description,
                                    InstructionType = item.InstructionType,
                                    CreateDate = item.CreateDate,
                                    UpdateDate = DateTime.UtcNow,
                                }
                            );
                        }
                        existSchedule.InstructionList = instructionList;

                        await _context.SaveChangesAsync();

                        return new Response
                        {
                            resultCd = 1,
                            MessageCode = "C432",
                            // Update type success
                        };
                    }
                }
                else
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C430",
                        // Update type success
                    };
            }
            else if (scheduleInstructionModel.ScheduleType == ScheduleType.StayingSchedule)
            {
                var existSchedule = _context.StayingSchedules.FirstOrDefault(entity =>
                    entity.Id == scheduleInstructionModel.ScheduleId
                );
                if (existSchedule != null)
                {
                    if (scheduleInstructionModel.InstructionList != null)
                    {
                        var instructionList = new List<Instruction>();
                        foreach (var item in scheduleInstructionModel.InstructionList)
                        {
                            instructionList.Add(
                                new Instruction
                                {
                                    Id = item.Id.Value,
                                    TourishPlanId = item.TourishPlanId,
                                    Description = item.Description,
                                    InstructionType = item.InstructionType,
                                    CreateDate = item.CreateDate,
                                    UpdateDate = DateTime.UtcNow,
                                }
                            );
                        }
                        existSchedule.InstructionList = instructionList;

                        await _context.SaveChangesAsync();

                        return new Response
                        {
                            resultCd = 1,
                            MessageCode = "C432",
                            // Update type success
                        };
                    }
                }
                else
                    return new Response
                    {
                        resultCd = 1,
                        MessageCode = "C430",
                        // Update type success
                    };
            }

            return new Response
            {
                resultCd = 1,
                MessageCode = "C434",
                // Update type success
            };
        }

        public async Task<Boolean> checkArrangeScheduleFromUser(
            String email,
            Guid scheduleId,
            ScheduleType scheduleType,
            List<string> scheduleList
        )
        {
            if (scheduleType == ScheduleType.MovingSchedule)
            {
                if (scheduleList.Count > 0)
                {
                    var count = await _context
                        .FullScheduleReceiptList.Include(entity => entity.ServiceSchedule)
                        .Include(entity => entity.TotalReceipt)
                        .Where(entity =>
                            entity.Email == email
                            && entity.TotalReceipt.MovingScheduleId == scheduleId
                            && entity.ServiceSchedule.EndDate >= DateTime.UtcNow
                            && scheduleList.Any(entity1 =>
                                entity1 == entity.ServiceSchedule.Id.ToString()
                            )
                        )
                        .CountAsync();

                    if (count >= 1)
                        return true;
                }
                else
                {
                    var count = await _context
                        .FullScheduleReceiptList.Include(entity => entity.ServiceSchedule)
                        .Include(entity => entity.TotalReceipt)
                        .Where(entity =>
                            entity.Email == email
                            && entity.TotalReceipt.MovingScheduleId == scheduleId
                            && entity.ServiceSchedule.EndDate >= DateTime.UtcNow
                        )
                        .CountAsync();

                    if (count >= 1)
                        return true;
                }
            }
            else if (scheduleType == ScheduleType.StayingSchedule)
            {
                if (scheduleList.Count > 0)
                {
                    var count = await _context
                        .FullScheduleReceiptList.Include(entity => entity.ServiceSchedule)
                        .Include(entity => entity.TotalReceipt)
                        .Where(entity =>
                            entity.Email == email
                            && entity.TotalReceipt.StayingScheduleId == scheduleId
                            && entity.ServiceSchedule.EndDate >= DateTime.UtcNow
                            && scheduleList.Any(entity1 =>
                                entity1 == entity.ServiceSchedule.Id.ToString()
                            )
                        )
                        .CountAsync();

                    if (count >= 1)
                        return true;
                }
                else
                {
                    var count = await _context
                        .FullScheduleReceiptList.Include(entity => entity.ServiceSchedule)
                        .Include(entity => entity.TotalReceipt)
                        .Where(entity =>
                            entity.Email == email
                            && entity.TotalReceipt.StayingScheduleId == scheduleId
                            && entity.ServiceSchedule.EndDate >= DateTime.UtcNow
                        )
                        .CountAsync();

                    if (count >= 1)
                        return true;
                }
            }

            return false;
        }

        private List<Instruction> initiateMovingInstructionList()
        {
            var instructionList = new List<Instruction>();

            foreach (var instruction in travelTicketPriceInstructionLines)
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

            foreach (var instruction in travelTicketCautionInstructionLines)
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

        private List<Instruction> initiateStayingInstructionList()
        {
            var instructionList = new List<Instruction>();

            foreach (var instruction in bookingPriceInstructionLines)
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

            foreach (var instruction in bookingCautionInstructionLines)
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
}
