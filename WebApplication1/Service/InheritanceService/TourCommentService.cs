using TourishApi.Service.Interface;
using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo;

namespace TourishApi.Service.InheritanceService
{
    public class TourishCommentService
        : IBaseService<TourishCommentRepository, TourishCommentModel>
    {
        private readonly TourishCommentRepository _entityRepository;

        public TourishCommentService(TourishCommentRepository airPlaneRepository)
        {
            _entityRepository = airPlaneRepository;
        }

        public Response CreateNew(TourishCommentModel entityModel)
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
                    MessageCode = "C814",
                    Error = ex.Message
                };
            }
        }

        public async Task<Response> CreateNewAsync(TourishCommentModel entityModel)
        {
            try
            {
                var response = await _entityRepository.AddAsync(entityModel);

                return (response);
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C814",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I813" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C814",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UserDeleteById(Guid id, string userId)
        {
            try
            {
                var comment = (TourishComment)_entityRepository.getById(id).Data;

                if (comment == null) return new Response { resultCd = 1, MessageCode = "C813" };

                if (comment.UserId.ToString() != userId) return new Response { resultCd = 1, MessageCode = "C813" };

                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I813" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C814",
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
                    MessageCode = "C814",
                    Error = ex.Message
                };
                return response;
            }
        }
        public Response GetAllByTourishPlanId(Guid tourishPlanId, string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            try
            {
                var entityList = _entityRepository.GetAllByTourishPlanId(tourishPlanId, search, type, sortBy, sortDirection, page, pageSize);
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C814",
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
                    var response = new Response { resultCd = 1, MessageCode = "C810", };
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
                    MessageCode = "C814",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, TourishCommentModel TourishCommentModel)
        {
            try
            {
                var response = _entityRepository.Update(TourishCommentModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C814",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
