using TourishApi.Service.Interface;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo.Connect;

namespace TourishApi.Service.InheritanceService
{
    public class UserMessageService : IBaseService<UserMessageRepository, UserMessageModel>
    {
        private readonly UserMessageRepository _entityRepository;

        public UserMessageService(UserMessageRepository airPlaneRepository)
        {
            _entityRepository = airPlaneRepository;
        }

        public Response CreateNew(UserMessageModel entityModel)
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
                    MessageCode = "C604",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I603" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C604",
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
                    MessageCode = "C604",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAllForTwo(string? search, Guid userSendId, Guid userReceiveId, string? sortBy, int page = 1, int pageSize = 5)
        {
            try
            {
                var entityList = _entityRepository.GetAllForTwo(search, userSendId, userReceiveId, sortBy, page, pageSize);
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C604",
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
                    var response = new Response { resultCd = 1, MessageCode = "C600", };
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
                    MessageCode = "C604",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, UserMessageModel UserMessageModel)
        {
            try
            {
                var response = _entityRepository.Update(UserMessageModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C604",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
