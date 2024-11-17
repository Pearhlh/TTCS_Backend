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
    public class TourishCommentRepository : IBaseRepository<TourishCommentModel>
    {
        private readonly MyDbContext _context;
        private readonly IBlobService blobService;
        public static int PAGE_SIZE { get; set; } = 5;
        public TourishCommentRepository(MyDbContext _context, IBlobService blobService)
        {
            this._context = _context;
            this.blobService = blobService;
        }

        public Response Add(TourishCommentModel addModel)
        {

            var addValue = new TourishComment
            {
                UserId = addModel.UserId,
                TourishPlanId = addModel.TourishPlanId,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I811",
                Data = addValue
                // Create type success               
            };
        }

        public async Task<Response> AddAsync(TourishCommentModel addModel)
        {

            var addValue = new TourishComment
            {
                UserId = addModel.UserId,
                TourishPlanId = addModel.TourishPlanId,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            await _context.AddAsync(addValue);
            await _context.SaveChangesAsync();
            await blobService.UploadStringBlobAsync("tourish-comment-container", addModel.Content ?? "", "text/plain", addValue.Id.ToString() ?? "" + ".txt");

            return new Response
            {
                resultCd = 0,
                MessageCode = "I811",
                Data = addValue
                // Create type success               
            };

        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.TourishComments.FirstOrDefault((entity
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
                MessageCode = "I813",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.TourishComments.AsQueryable();

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
            var result = PaginatorModel<TourishComment>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;

        }

        public Response GetAllByTourishPlanId(Guid tourishPlanId, string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.TourishComments.Include(entityQuery => entityQuery.User).AsSplitQuery();

            #region Filtering
            entityQuery = entityQuery.Where(entity => entity.TourishPlanId == tourishPlanId);
            #endregion

            #region Sorting
            entityQuery = entityQuery.OrderByDescending(entity => entity.UpdateDate);
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
            var result = PaginatorModel<TourishComment>.Create(entityQuery, page, pageSize);
            #endregion


            var resultDto = result.Select(tourComment => new TourishCommentDTOModel
            {
                Id = tourComment.Id,
                UserId = tourComment.UserId,
                UserName = tourComment.User.UserName,
                TourishPlanId = tourComment.TourishPlanId,
                CreateDate = tourComment.CreateDate,
                UpdateDate = tourComment.UpdateDate,
            }).OrderByDescending(entity => entity.CreateDate).ToList();

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
            var entity = _context.TourishComments.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response getByName(String name)
        {

            return new Response
            {
                resultCd = 0,
                Data = null
            };
        }

        public Response Update(TourishCommentModel entityModel)
        {
            var entity = _context.TourishComments.FirstOrDefault((entity
                => entity.Id == entityModel.Id));
            if (entity != null)
            {
                if (entityModel.Content != null && entityModel.Content.Length > 0)
                {
                    blobService.UploadStringBlobAsync("tourish-comment-container", entityModel.Content ?? "", "text/plain", entityModel.Id.ToString() ?? "" + ".txt");
                }

                entity.UpdateDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I812",
                // Update type success               
            };
        }
    }
}
