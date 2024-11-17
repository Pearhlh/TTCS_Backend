using TourishApi.Service.Interface;
using WebApplication1.Model.Connection;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo.Connect;

namespace TourishApi.Service.InheritanceService
{
    public class GuestMessageConHistoryService : IBaseService<GuestMessageConHistoryRepository, GuestMessageConHistoryModel>
    {
        private readonly GuestMessageConHistoryRepository _entityRepository;

        public GuestMessageConHistoryService(GuestMessageConHistoryRepository airPlaneRepository)
        {
            _entityRepository = airPlaneRepository;
        }

        public Response CreateNew(GuestMessageConHistoryModel entityModel)
        {
            try
            {
                var response = _entityRepository.Add(entityModel);
                return (response);
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I1113" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            try
            {
                var entityList = _entityRepository.GetAll(search, type, sortBy, sortDirection, page, pageSize);
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAllForAdmin(string? search, int? type, string? sortBy, string? sortDirection, string? userId, int page = 1, int pageSize = 5)
        {
            try
            {
                var entityList = _entityRepository.GetAllForAdmin(search, type, sortBy, sortDirection, userId, page, pageSize);
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> getByGuestConId(string connectionId)
        {
            try
            {
                var entity = await _entityRepository.getByGuestConId(connectionId);
                if (entity.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C1110", };
                    return response;
                }
                else
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetById(Guid id)
        {
            try
            {
                var entity = _entityRepository.getById(id);
                if (entity.Data == null)
                {
                    var response = new Response { resultCd = 1, MessageCode = "C1110", };
                    return response;
                }
                else
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, GuestMessageConHistoryModel GuestMessageConHistoryModel)
        {
            try
            {
                var response = _entityRepository.Update(GuestMessageConHistoryModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C1114",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
