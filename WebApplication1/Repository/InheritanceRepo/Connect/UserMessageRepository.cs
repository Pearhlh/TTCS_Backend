using Microsoft.EntityFrameworkCore;
using TourishApi.Extension;
using TourishApi.Repository.Interface;
using WebApplication1.Data.Chat;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.InheritanceRepo.Connect
{
    public class UserMessageRepository : IBaseRepository<UserMessageModel>
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public UserMessageRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public Response Add(UserMessageModel addModel)
        {

            var addValue = new UserMessage
            {
                UserReceiveId = addModel.UserReceiveId,
                UserSentId = addModel.UserSentId,
                UpdateDate = addModel.UpdateDate,
                Content = addModel.Content,
                CreateDate = DateTime.UtcNow
            };
            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I601",
                // Create type success               
            };

        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.UserMessages.FirstOrDefault((entity
               => entity.Id == id));
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I603",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.UserMessages.AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Content.ToString().Contains(search));
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
            var result = PaginatorModel<UserMessage>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response GetAllForTwo(string? search, Guid userSendId, Guid userReceiveId, string? sortBy, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.UserMessages.Include(entity => entity.UserSent).Include(entity => entity.UserReceive).AsQueryable();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.UserSent.Id == userSendId && entity.UserReceive.Id == userReceiveId);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Content.ToString().Contains(search));
            }
            #endregion

            #region Sorting
            entityQuery = entityQuery.OrderByDescending(entity => entity.CreateDate);
            #endregion

            #region Paging
            var result = PaginatorModel<UserMessage>.Create(entityQuery, page, pageSize);
            #endregion

            var resultDto = entityQuery.Select(entity => new MessageReturnModel
            {
                UserReceiveId = userReceiveId,
                UserSentId = userSendId,
                Content = entity.Content,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate,
                UserReceiveName = entity.UserReceive.FullName,
                UserSentName = entity.UserSent.FullName
            }).ToList();

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
            var entity = _context.UserMessages.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response getByName(String name)
        {
            var entity = _context.UserMessages.FirstOrDefault((entity
                => entity.Content.ToString() == name));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response Update(UserMessageModel entityModel)
        {
            var entity = _context.UserMessages.FirstOrDefault((entity
                => entity.Id == entityModel.Id));
            if (entity != null)
            {
                entity.UserSentId = entityModel.UserSentId;
                entity.UserReceiveId = entityModel.UserReceiveId;
                entity.Content = entityModel.Content;
                entity.IsRead = entityModel.IsRead;
                entity.IsDeleted = entityModel.IsDeleted;

                entityModel.UpdateDate = DateTime.UtcNow;

                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I602",
                // Update type success               
            };
        }
    }
}
