using WebApplication1.Model.VirtualModel;

namespace TourishApi.Service.Interface
{
    public interface IBaseService<IRespository, IModel>
    {
        public Response CreateNew(IModel entityModel);
        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5);
        public Response GetById(Guid id);
        public Response UpdateEntityById(Guid id, IModel entityModel);
        public Response DeleteById(Guid id);
    }
}
