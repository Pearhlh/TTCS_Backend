using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.Interface
{
    public interface IUserRepository
    {
        Response GetAll(string? search, int type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5);
        Response getById(Guid id, int? type);
        Response getByEmail(string email);
        Response getByName(String userName, int? type);
        Task<Response> UpdateInfo(UserRole userRoleAuthority, Boolean isSelfUpdate, UserUpdateModel entityModel);
        Task<Response> UpdatePassword(UserUpdatePasswordModel entityModel);
        Task<Response> ReclaimPassword(UserReclaimPasswordModel entityModel);
        Response Delete(Guid id);
    }
}
