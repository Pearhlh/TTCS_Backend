using TourishApi.Extension;
using TourishApi.Repository.Interface;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Data.Transport;
using WebApplication1.Model;
using WebApplication1.Model.Transport;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.InheritanceRepo.Transport
{
    public class MovingContactRepository : IBaseRepository<MovingContactModel>
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public MovingContactRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public Response Add(MovingContactModel addModel)
        {

            var addValue = new MovingContact
            {
                BranchName = addModel.BranchName,
                HotlineNumber = addModel.HotlineNumber,
                SupportEmail = addModel.SupportEmail,
                HeadQuarterAddress = addModel.HeadquarterAddress,
                Description = addModel.Description,
                DiscountAmount = addModel.DiscountAmount,
                DiscountFloat = addModel.DiscountFloat,
                VehicleType = addModel.VehicleType,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            _context.Add(addValue);
            _context.SaveChanges();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I111",
                // Create type success               
            };

        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.MovingContactList.FirstOrDefault((entity
               => entity.Id == id));
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I113",
                // Delete type success               
            };
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var entityQuery = _context.MovingContactList.AsQueryable();

            #region Filtering
            if (type != null)
            {
                entityQuery = entityQuery.Where(entity => (int)entity.VehicleType == type);
            }


            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.BranchName.Contains(search));
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
            var result = PaginatorModel<MovingContact>.Create(entityQuery, page, pageSize);
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
            var entity = _context.MovingContactList.FirstOrDefault((entity
                => entity.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response getByName(String name)
        {
            var entity = _context.MovingContactList.FirstOrDefault((entity
                => entity.BranchName == name));

            return new Response
            {
                resultCd = 0,
                Data = entity
            };
        }

        public Response Update(MovingContactModel entityModel)
        {
            var entity = _context.MovingContactList.FirstOrDefault((entity
                => entity.Id == entityModel.Id));
            if (entity != null)
            {
                entity.UpdateDate = DateTime.UtcNow;
                entity.BranchName = entityModel.BranchName;
                entity.HotlineNumber = entityModel.HotlineNumber;
                entity.SupportEmail = entityModel.SupportEmail;
                entity.HeadQuarterAddress = entityModel.HeadquarterAddress;
                entity.Description = entityModel.Description;
                entity.DiscountAmount = entityModel.DiscountAmount;
                entity.DiscountFloat = entityModel.DiscountFloat;
                entity.VehicleType = entityModel.VehicleType;
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I112",
                // Update type success               
            };
        }
    }
}
