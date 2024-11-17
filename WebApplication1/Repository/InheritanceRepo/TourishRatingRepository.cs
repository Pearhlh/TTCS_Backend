using Microsoft.EntityFrameworkCore;
using TourishApi.Extension;
using TourishApi.Repository.Interface;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Service;

namespace WebApplication1.Repository.InheritanceRepo
{
    public class TourishRatingRepository : IBaseRepository<TourishRatingModel>
    {
        private readonly MyDbContext _context;
        private readonly IBlobService blobService;
        public static int PAGE_SIZE { get; set; } = 5;
        public TourishRatingRepository(MyDbContext _context, IBlobService blobService)
        {
            this._context = _context;
            this.blobService = blobService;
        }

        public Response Add(TourishRatingModel addModel)
        {

            var addValue = new TourishRating
            {
                UserId = addModel.UserId,
                TourishPlanId = addModel.TourishPlanId,
                Rating = addModel.Rating,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I821",
                // Create type success               
            };
        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.TourishRatings.FirstOrDefault((entity
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
                MessageCode = "I823",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.TourishRatings.AsQueryable();

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
            var result = PaginatorModel<TourishRating>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;

        }

        public Response GetAllByTourishPlanId(Guid tourishPlanId)
        {
            var entityQuery = _context.TourishRatings.Include(entityQuery => entityQuery.User).AsSplitQuery();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlanId == tourishPlanId);
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
            var entity = _context.TourishRatings.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public TourishRating getByUserIdAndTourId(Guid UserId, Guid TourId)
        {
            var entity = _context.TourishRatings.AsSplitQuery().FirstOrDefault((entity
                => entity.UserId == UserId && entity.TourishPlanId == TourId));

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

        public Response Update(TourishRatingModel entityModel)
        {
            var entity = _context.TourishRatings.FirstOrDefault((entity
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
                MessageCode = "I822",
                // Update type success               
            };
        }
    }
}
