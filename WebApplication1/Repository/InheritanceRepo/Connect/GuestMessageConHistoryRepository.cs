using Microsoft.EntityFrameworkCore;
using TourishApi.Extension;
using TourishApi.Repository.Interface;
using WebApplication1.Data.Connection;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.Connection;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.InheritanceRepo.Connect
{
    public class GuestMessageConHistoryRepository : IBaseRepository<GuestMessageConHistoryModel>
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;

        public GuestMessageConHistoryRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public Response Add(GuestMessageConHistoryModel addModel)
        {
            var addValue = new GuestMessageConHistory
            {
                GuestConId = addModel.Id,
                AdminConId = addModel.AdminConId,
                CreateDate = DateTime.UtcNow
            };
            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I1111",
                // Create type success
            };
        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.GuestMessageConHisList.FirstOrDefault(
                (entity => entity.Id == id)
            );
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I1113",
                // Delete type success
            };
        }

        public Response GetAll(
            string? search,
            int? type,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            var entityQuery = _context
                .GuestMessageConHisList.Include(entity => entity.GuestCon)
                .Include(entity => entity.AdminCon)
                .ThenInclude(entity => entity.Admin)
                .AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity =>
                    entity.AdminConId.ToString().Contains(search)
                );
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
            var result = PaginatorModel<GuestMessageConHistory>.Create(entityQuery, page, pageSize);
            #endregion

            var resultDto = result
                .Select(guestConHis => new GuestMessageConHistoryDTOModel
                {
                    Id = guestConHis.Id,
                    GuestMessageCon = new GuestMessageConDTOModel
                    {
                        Id = guestConHis.GuestCon.Id,
                        Connected = guestConHis.GuestCon.Connected,
                        GuestEmail = guestConHis.GuestCon.GuestEmail,
                        GuestName = guestConHis.GuestCon.GuestName,
                        GuestPhoneNumber = guestConHis.GuestCon.GuestPhoneNumber,
                        ConnectionID = guestConHis.GuestCon.ConnectionID,
                        UserAgent = guestConHis.GuestCon.UserAgent,
                        CreateDate = guestConHis.GuestCon.CreateDate
                    },
                    AdminMessageCon =
                        guestConHis.AdminCon != null
                            ? new AdminMessageConDTOModel
                            {
                                Id = guestConHis.AdminCon.Id,
                                Connected = guestConHis.AdminCon.Connected,
                                ConnectionID = guestConHis.AdminCon.ConnectionID,
                                AdminFullName = guestConHis.AdminCon.Admin.FullName,
                                AdminId = guestConHis.AdminCon.AdminId.Value,
                                UserAgent = guestConHis.AdminCon.UserAgent,
                                CreateDate = guestConHis.AdminCon.CreateDate
                            }
                            : null,
                    Status = guestConHis.GuestCon.Connected
                        ? (guestConHis.AdminCon != null ? 1 : 2)
                        : 0,
                    CreateDate = guestConHis.CreateDate,
                    CloseDate = guestConHis.CloseDate,
                })
                .OrderByDescending(entity => entity.Status)
                .ToList();

            var entityVM = new Response
            {
                resultCd = 0,
                Data = resultDto,
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response GetAllForAdmin(
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
                .GuestMessageConHisList.Include(entity => entity.GuestCon)
                .ThenInclude(entity => entity.GuestMessages)
                .Include(entity => entity.AdminCon)
                .ThenInclude(entity => entity.Admin)
                .AsQueryable();

            #region Filtering
            if (!String.IsNullOrEmpty(userId))
            {
                entityQuery = entityQuery.Where(entity => entity.AdminCon.Admin.Id.ToString() == userId);
            }

            entityQuery = entityQuery.Where(entity => entity.AdminCon != null);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity =>
                    entity.AdminConId.ToString().Contains(search)
                );
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
            var result = PaginatorModel<GuestMessageConHistory>.Create(entityQuery, page, pageSize);
            #endregion

            var resultDto = result
                .Select(guestConHis => new GuestMessageConHistoryDTOModel
                {
                    Id = guestConHis.Id,
                    GuestMessageCon = new GuestMessageConDTOModel
                    {
                        Id = guestConHis.GuestCon.Id,
                        Connected = guestConHis.GuestCon.Connected,
                        GuestEmail = guestConHis.GuestCon.GuestEmail,
                        GuestMessages = guestConHis
                            .GuestCon.GuestMessages.Select(element => new GuestMessageModel
                            {
                                Content = element.Content,
                                CreateDate = element.CreateDate,
                                UpdateDate = element.UpdateDate,
                                Id = element.Id,
                                IsDeleted = element.IsDeleted,
                                IsRead = element.IsRead,
                                Side = element.AdminMessageCon != null ? 1 : 2,
                                State = 2
                            }).OrderByDescending(entity => entity.CreateDate)
                            .ToList(),
                        GuestName = guestConHis.GuestCon.GuestName,
                        GuestPhoneNumber = guestConHis.GuestCon.GuestPhoneNumber,
                        ConnectionID = guestConHis.GuestCon.ConnectionID,
                        UserAgent = guestConHis.GuestCon.UserAgent,
                        CreateDate = guestConHis.GuestCon.CreateDate,
                    },
                    AdminMessageCon =
                        guestConHis.AdminCon != null
                            ? new AdminMessageConDTOModel
                            {
                                Id = guestConHis.AdminCon.Id,
                                Connected = guestConHis.AdminCon.Connected,
                                ConnectionID = guestConHis.AdminCon.ConnectionID,
                                AdminFullName = guestConHis.AdminCon.Admin.FullName,
                                AdminId = guestConHis.AdminCon.AdminId.Value,
                                UserAgent = guestConHis.AdminCon.UserAgent,
                                CreateDate = guestConHis.AdminCon.CreateDate
                            }
                            : null,
                    Status = guestConHis.GuestCon.Connected
                        ? (guestConHis.AdminCon != null ? 1 : 2)
                        : 0,
                    CreateDate = guestConHis.CreateDate,
                    CloseDate = guestConHis.CloseDate,
                })
                .OrderByDescending(entity => entity.CreateDate)
                .ThenByDescending(entity => entity.Status)
                .ToList();

            var entityVM = new Response
            {
                resultCd = 0,
                Data = resultDto,
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response getById(Guid id)
        {
            var entity = _context.GuestMessageConHisList.FirstOrDefault(
                (entity => entity.Id == id)
            );

            return new Response { resultCd = 0, Data = entity };
        }

        public async Task<Response> getByGuestConId(string connectionId)
        {
            try
            {
                var entity = _context
                    .GuestMessageConHisList.Include(entity => entity.GuestCon)
                    .ThenInclude(entity => entity.GuestMessages)
                    .Include(entity => entity.AdminCon)
                    .ThenInclude(entity => entity.Admin)
                    .Include(entity => entity.AdminCon)
                    .ThenInclude(entity => entity.GuestMessages)
                    .OrderByDescending(entity => entity.CreateDate)
                    .Where(entity => entity.GuestCon.ConnectionID == connectionId && entity.GuestCon.IsChatWithBot == 0);

                var resultDtoQuery = entity.Select(guestConHis => new GuestMessageConHistoryDTOModel
                {
                    Id = guestConHis.Id,

                    GuestMessageCon = new GuestMessageConDTOModel
                    {
                        Id = guestConHis.GuestCon.Id,
                        Connected = guestConHis.GuestCon.Connected,
                        GuestEmail = guestConHis.GuestCon.GuestEmail,
                        GuestName = guestConHis.GuestCon.GuestName,
                        GuestPhoneNumber = guestConHis.GuestCon.GuestPhoneNumber,
                        ConnectionID = guestConHis.GuestCon.ConnectionID,
                        UserAgent = guestConHis.GuestCon.UserAgent,
                        CreateDate = guestConHis.GuestCon.CreateDate
                    },

                    AdminMessageCon =
                        guestConHis.AdminCon != null
                            ? new AdminMessageConDTOModel
                            {
                                Id = guestConHis.AdminCon.Id,
                                Connected = guestConHis.AdminCon.Connected,
                                ConnectionID = guestConHis.AdminCon.ConnectionID,
                                AdminFullName = guestConHis.AdminCon.Admin.FullName,
                                AdminId = guestConHis.AdminCon.AdminId.Value,
                                UserAgent = guestConHis.AdminCon.UserAgent,
                                CreateDate = guestConHis.AdminCon.CreateDate
                            }
                            : null,
                    Status = guestConHis.GuestCon.Connected
                        ? (guestConHis.AdminCon != null ? 1 : 2)
                        : 0,
                    CreateDate = guestConHis.CreateDate,
                    CloseDate = guestConHis.CloseDate,
                });

                var resultDtoList = await resultDtoQuery.ToListAsync();

                var resultDto = await resultDtoQuery.FirstOrDefaultAsync();

                if (resultDto.AdminMessageCon != null)
                {
                    var messageList = _context
                        .GuestMessages.Include(entity => entity.AdminMessageCon)
                        .Include(entity => entity.GuestMessageCon)
                        .Where(entity =>
                            (entity.GuestMessageCon.ConnectionID == connectionId && entity.GuestMessageCon.IsChatWithBot == 0)
                            || resultDtoQuery.Count(entity1 =>
                                entity1.AdminMessageCon.ConnectionID
                                == entity.AdminMessageCon.ConnectionID
                            ) >= 1
                        )
                        .Select(element => new GuestMessageModel
                        {
                            Content = element.Content,
                            CreateDate = element.CreateDate,
                            UpdateDate = element.UpdateDate,
                            Id = element.Id,
                            IsDeleted = element.IsDeleted,
                            IsRead = element.IsRead,
                            Side = element.AdminMessageConId != null ? 1 : 2,
                            State = 2
                        })
                        .ToList();

                    resultDto.GuestMessages = messageList;
                    resultDto.GuestMessageCon.GuestMessages = messageList
                        .OrderByDescending(entity => entity.CreateDate)
                        .ToList();
                }

                return new Response { resultCd = 0, Data = resultDto, };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 0,
                    Data = ex,
                    Error = ex.Message
                };
            }
        }

        public Response getByName(String name)
        {
            var entity = _context.GuestMessageConHisList.FirstOrDefault(
                (entity => entity.AdminConId.ToString() == name)
            );

            return new Response { resultCd = 0, Data = entity };
        }

        public Response Update(GuestMessageConHistoryModel entityModel)
        {
            var entity = _context.GuestMessageConHisList.FirstOrDefault(
                (entity => entity.Id == entityModel.Id)
            );
            if (entity != null)
            {
                entity.AdminConId = entityModel.AdminConId;
                entity.GuestConId = entityModel.GuestConId;

                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I1112",
                // Update type success
            };
        }
    }
}
