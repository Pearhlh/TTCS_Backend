using Microsoft.EntityFrameworkCore;
using TourishApi.Extension;
using TourishApi.Repository.Interface;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.InheritanceRepo
{
    public class TourishCategoryRepository : IBaseRepository<TourishCategoryModel>
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public TourishCategoryRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public Response Add(TourishCategoryModel addModel)
        {

            var addValue = new TourishCategory
            {
                Name = addModel.Name,
                Description = addModel.Description,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I421",
                // Create type success               
            };

        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.TourishCategories.FirstOrDefault((entity
               => entity.Id == id));
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I423",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.TourishCategories.AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Name.Contains(search));
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
            var result = PaginatorModel<TourishCategory>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;

        }

        public Response clientGetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.TourishCategories.AsQueryable();

            #region Filtering
            entityQuery = entityQuery.Where(entity => _context.TourishPlan.Include(entity1 => entity1.TourishCategoryRelations).
            Include(entity1 => entity1.TourishScheduleList).
            Where(entity2 => entity2.TourishCategoryRelations
            .Where(entity3 => entity3.TourishCategoryId == entity.Id).Count() >= 1 && entity2.TourishScheduleList
            .Where(entity4 => entity4.PlanStatus == PlanStatus.ConfirmInfo).Count() >= 1)
            .Count() >= 1);

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.Name.Contains(search));
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
            var result = PaginatorModel<TourishCategory>.Create(entityQuery, page, pageSize);
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
            var entity = _context.TourishCategories.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response getByName(String name)
        {
            var entity = _context.TourishCategories.FirstOrDefault((entity
                => entity.Name == name));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response Update(TourishCategoryModel entityModel)
        {
            var entity = _context.TourishCategories.FirstOrDefault((entity
                => entity.Id == entityModel.Id));
            if (entity != null)
            {
                entity.UpdateDate = DateTime.UtcNow;
                entity.Description = entityModel.Description;
                entity.Name = entityModel.Name;
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I422",
                // Update type success               
            };
        }
    }
}
