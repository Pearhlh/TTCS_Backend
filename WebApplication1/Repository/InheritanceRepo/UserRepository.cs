using TourishApi.Extension;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;

namespace WebApplication1.Repository.InheritanceRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly ILogger<UserRepository> logger;
        public static int PAGE_SIZE { get; set; } = 5;

        public UserRepository(MyDbContext _context, ILogger<UserRepository> _logger)
        {
            this._context = _context;
            this.logger = _logger;
        }

        public Response Delete(Guid id)
        {
            var deleteEntity = _context.Users.FirstOrDefault((entity => entity.Id == id));
            if (deleteEntity != null)
            {
                _context.Remove(deleteEntity);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I003",
                // Delete type success
            };
        }

        public Response GetAll(
            string? search,
            int type,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            var entityQuery = _context.Users.AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                entityQuery = entityQuery.Where(entity => entity.FullName.Contains(search));
            }

            entityQuery = entityQuery.Where(entity => (int)entity.Role == type);

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
            var result = PaginatorModel<User>.Create(entityQuery, page, pageSize);
            #endregion

            var entityVM = new Response
            {
                resultCd = 0,
                Data = result.ToList(),
                count = result.TotalCount,
            };
            return entityVM;
        }

        public Response getById(Guid id, int? type)
        {
            if (type != null)
            {
                var entity = _context.Users.FirstOrDefault(
                    (entity => entity.Id == id && (int)entity.Role == type)
                );

                return new Response { resultCd = 0, Data = entity };
            }
            else
            {
                var entity = _context.Users.FirstOrDefault((entity => entity.Id == id));

                return new Response { resultCd = 0, Data = entity };
            }
        }

        public Response getByEmail(string email)
        {

            var entity = _context.Users.FirstOrDefault((entity => entity.Email == email));

            return new Response { resultCd = 0, Data = entity };

        }

        public Response getByName(String name, int? type)
        {
            var entity = _context.Users.FirstOrDefault(
                (entity => entity.UserName == name && (int)entity.Role == type)
            );

            return new Response { resultCd = 0, Data = entity };
        }

        public async Task<Response> UpdateInfo(
            UserRole userRoleAuthority,
            Boolean isSelfUpdate,
            UserUpdateModel model
        )
        {
            var userExist = _context.Users.FirstOrDefault(p => p.UserName == model.UserName);

            logger.LogInformation(userExist.ToString());

            if ((int)userRoleAuthority == (int)userExist.Role)
            {
                if (!isSelfUpdate)
                {
                    return new Response { resultCd = 1, MessageCode = "C015", };
                }
            }

            if (
                (int)userRoleAuthority < (int)userExist.Role
                || (int)userRoleAuthority < (int)model.Role
            )
            {
                return new Response { resultCd = 1, MessageCode = "C015", };
            }

            if (userExist != null) //không đúng
            {
                userExist.UpdateDate = DateTime.UtcNow;
                userExist.FullName = model.FullName ?? userExist.FullName;
                userExist.PhoneNumber = model.PhoneNumber ?? userExist.PhoneNumber;
                userExist.Address = model.Address ?? userExist.Address;
                userExist.Role = model.Role ?? userExist.Role;

                if (userExist.PasswordHash != "None")
                {
                    userExist.Email = model.Email;
                }

                await _context.SaveChangesAsync();

                return new Response { resultCd = 0, MessageCode = "I011", };
            }

            return new Response { resultCd = 0, MessageCode = "C011", };
        }

        public async Task<Response> UpdatePassword(UserUpdatePasswordModel model)
        {
            var userExist = _context.Users.FirstOrDefault(p => p.UserName == model.UserName);
            if (userExist != null) //không đúng
            {
                userExist.UpdateDate = DateTime.UtcNow;

                if (userExist.PasswordHash != "None")
                {
                    userExist.PasswordHash = model.NewPassword;
                    userExist.PasswordSalt = model.PasswordSalt;
                }

                await _context.SaveChangesAsync();

                return new Response { resultCd = 0, MessageCode = "I012", };
            }

            return new Response { resultCd = 0, MessageCode = "C012", };
        }

        public async Task<Response> ReclaimPassword(UserReclaimPasswordModel model)
        {
            var userExist = _context.Users.FirstOrDefault(p => p.UserName == model.UserName);
            if (userExist != null) //không đúng
            {
                userExist.UpdateDate = DateTime.UtcNow;

                if (userExist.PasswordHash != "None")
                {
                    userExist.PasswordHash = model.NewPassword;
                    userExist.PasswordSalt = model.PasswordSalt;
                    userExist.AccessFailedCount = 0;
                    userExist.LockoutEnd = null;
                }

                await _context.SaveChangesAsync();

                return new Response { resultCd = 0, MessageCode = "I012", };
            }

            return new Response { resultCd = 0, MessageCode = "C012", };
        }
    }
}
