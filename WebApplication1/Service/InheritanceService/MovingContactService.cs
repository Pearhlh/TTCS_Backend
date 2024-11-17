using TourishApi.Service.Interface;
using WebApplication1.Model.Transport;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo.Transport;

namespace TourishApi.Service.InheritanceService
{
    public class MovingContactService : IBaseService<MovingContactRepository, MovingContactModel>
    {
        private readonly MovingContactRepository _entityRepository;

        public MovingContactService(MovingContactRepository airPlaneRepository)
        {
            _entityRepository = airPlaneRepository;
        }

        public Response CreateNew(MovingContactModel entityModel)
        {
            try
            {
                var entityExist = _entityRepository.getByName(entityModel.BranchName);

                if (entityExist.Data == null)
                {
                    var response = _entityRepository.Add(entityModel);

                    return (response);
                }
                else
                {
                    var response = new Response { resultCd = 1, MessageCode = "C111", };
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C114",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I113" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C114",
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
                    MessageCode = "C114",
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
                    var response = new Response { resultCd = 1, MessageCode = "C110", };
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
                    MessageCode = "C114",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, MovingContactModel MovingContactModel)
        {
            try
            {
                var response = _entityRepository.Update(MovingContactModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C114",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
