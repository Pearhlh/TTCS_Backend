using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.Interface
{
    public interface IFileRepository
    {
        Response getByProductId(Guid id, ResourceTypeEnum type);
        Task<Response> getById(Guid id);
        Task<Response> Add(FileModel fileModel);
        Response Delete(Guid id);
    }
}
