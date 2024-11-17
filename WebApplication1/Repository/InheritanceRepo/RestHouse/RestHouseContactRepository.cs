using TourishApi.Extension;
using TourishApi.Repository.Interface;

using WebApplication1.Data.DbContextFile;
using WebApplication1.Data.RestHouse;
using WebApplication1.Model;
using WebApplication1.Model.RestHouse;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.InheritanceRepo.RestHoouse
{
    public class RestHouseContactRepository : IBaseRepository<RestHouseContactModel>
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public RestHouseContactRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public Response Add(RestHouseContactModel addModel)
        {

            var addValue = new RestHouseContact
            {
                PlaceBranch = addModel.PlaceBranch,
                HotlineNumber = addModel.HotlineNumber,
                SupportEmail = addModel.SupportEmail,
                RestHouseType = addModel.RestHouseType,
                HeadQuarterAddress = addModel.HeadQuarterAddress,
                Description = addModel.Description,
                DiscountAmount = addModel.DiscountAmount,
                DiscountFloat = addModel.DiscountFloat,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I211",
                // Create type success               
            };

        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.RestHouseContactList.FirstOrDefault((entity
               => entity.Id == id));
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I213",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.RestHouseContactList.AsQueryable();

            #region Filtering
            if (type != null)
            {
                entityQuery = entityQuery.Where(entity => (int)entity.RestHouseType == type);
            }

            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.PlaceBranch.Contains(search));
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
            var result = PaginatorModel<RestHouseContact>.Create(entityQuery, page, pageSize);
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
            var entity = _context.RestHouseContactList.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response getByName(String name)
        {
            var entity = _context.RestHouseContactList.FirstOrDefault((entity
                => entity.PlaceBranch == name));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response Update(RestHouseContactModel entityModel)
        {
            var entity = _context.RestHouseContactList.FirstOrDefault((entity
                => entity.Id == entityModel.Id));
            if (entity != null)
            {
                entity.UpdateDate = DateTime.UtcNow;
                entity.PlaceBranch = entityModel.PlaceBranch;
                entity.HotlineNumber = entityModel.HotlineNumber;
                entity.SupportEmail = entityModel.SupportEmail;
                entity.RestHouseType = entityModel.RestHouseType;
                entity.HeadQuarterAddress = entityModel.HeadQuarterAddress;
                entity.Description = entityModel.Description;
                entity.DiscountAmount = entityModel.DiscountAmount;
                entity.DiscountFloat = entityModel.DiscountFloat;
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I212",
                // Update type success               
            };
        }
    }
}
