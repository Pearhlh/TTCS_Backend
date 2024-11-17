using Microsoft.EntityFrameworkCore;
using TourishApi.Extension;
using TourishApi.Repository.Interface;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Service;

namespace WebApplication1.Repository.InheritanceRepo
{
    public class ScheduleRatingRepository : IBaseRepository<ScheduleRatingModel>
    {
        private readonly MyDbContext _context;
        private readonly IBlobService blobService;
        public static int PAGE_SIZE { get; set; } = 5;
        public ScheduleRatingRepository(MyDbContext _context, IBlobService blobService)
        {
            this._context = _context;
            this.blobService = blobService;
        }

        public Response Add(ScheduleRatingModel addModel)
        {

            var addValue = new ScheduleRating
            {
                UserId = addModel.UserId,
                ScheduleId = addModel.ScheduleId,
                ScheduleType = addModel.ScheduleType,
                Rating = addModel.Rating,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I831",
                // Create type success               
            };
        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.ScheduleRatings.FirstOrDefault((entity
               => entity.Id == id));
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                blobService.DeleteFileBlobAsync("tourish-comment-container", id.ToString());
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I833",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.ScheduleRatings.AsQueryable();

            #region Filtering
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
            var result = PaginatorModel<ScheduleRating>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;

        }

        public Response GetAllByScheduleId(Guid scheduleId, ScheduleType scheduleType)
        {
            var entityQuery = _context.ScheduleRatings.Include(entityQuery => entityQuery.User).AsQueryable();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.ScheduleId == scheduleId && entity.ScheduleType == scheduleType);
            #endregion

            var mapEntityQuery = entityQuery.Select(entity => new
            {
                Rating = entity.Rating,
                UserName = entity.User.UserName
            });

            var totalPoint = 0;
            var totalCount = entityQuery.Count();
            var mapEntityList = mapEntityQuery.ToList();

            foreach (var mapEntity in mapEntityList)
            {
                totalPoint += mapEntity.Rating;
            }

            var entityVM = new Response
            {
                resultCd = 0,
                Data = mapEntityQuery.ToList(),
                AveragePoint = totalCount <= 0 ? totalPoint : totalPoint / totalCount,
                count = entityQuery.Count(),
            };
            return entityVM;
        }

        public Response getById(Guid id)
        {
            var entity = _context.ScheduleRatings.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public ScheduleRating getByUserIdAndScheduleId(Guid UserId, Guid TourId, ScheduleType scheduleType)
        {
            var entity = _context.ScheduleRatings.FirstOrDefault((entity
                => entity.UserId == UserId && entity.ScheduleId == TourId && entity.ScheduleType == scheduleType));

            return entity;
        }

        public Response getByName(String name)
        {

            return new Response
            {
                resultCd = 0,
                Data = null
            };
        }

        public Response Update(ScheduleRatingModel entityModel)
        {
            var entity = _context.ScheduleRatings.FirstOrDefault((entity
                => entity.Id == entityModel.Id));
            if (entity != null)
            {
                entity.Rating = entityModel.Rating;
                entity.UpdateDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I832",
                // Update type success               
            };
        }
    }
}
