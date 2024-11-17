using WebApplication1.Model.VirtualModel;

namespace TourishApi.Repository.Interface
{
    public interface IBaseRepository<T>
    {
        Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5);
        Response getById(Guid id);
        Response getByName(string name);
        Response Add(T addModel);
        Response Update(T updateModel);
        Response Delete(Guid id);
    }
}
