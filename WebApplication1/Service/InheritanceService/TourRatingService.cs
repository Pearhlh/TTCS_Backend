using TourishApi.Service.Interface;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo;

namespace TourishApi.Service.InheritanceService
{
    public class TourishRatingService
        : IBaseService<TourishRatingRepository, TourishRatingModel>
    {
        private readonly TourishRatingRepository _entityRepository;

        public TourishRatingService(TourishRatingRepository airPlaneRepository)
        {
            _entityRepository = airPlaneRepository;
        }

        public Response CreateNew(TourishRatingModel entityModel)
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
                    MessageCode = "C824",
                    Error = ex.Message
                };
            }
        }

        public Response SendRating(TourishRatingModel entityModel)
        {
            try
            {
                var existRating = _entityRepository.getByUserIdAndTourId(entityModel.UserId, entityModel.TourishPlanId);
                if (existRating == null)
                {
                    var response = _entityRepository.Add(entityModel);

                    return (response);
                }
                else
                {
                    existRating.Rating = entityModel.Rating;
                    entityModel.Id = existRating.Id;
                    return _entityRepository.Update(entityModel);
                }

            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C824",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I823" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C824",
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
                    MessageCode = "C824",
                    Error = ex.Message
                };
                return response;
            }
        }
        public Response GetAllByTourishPlanId(Guid tourishPlanId)
        {
            try
            {
                var entityList = _entityRepository.GetAllByTourishPlanId(tourishPlanId);
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C824",
                    Error = ex.Message
                };
                return response;
            }
        }
        public Response getByUserIdAndTourId(Guid UserId, Guid TourId)
        {
            var entity = _entityRepository.getByUserIdAndTourId(UserId, TourId);

            if (entity == null)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C820"
                };
                return response;
            }
            else
            {
                var response = new Response
                {
                    resultCd = 0,
                    Data = entity
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
                    var response = new Response { resultCd = 1, MessageCode = "C820", };
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
                    MessageCode = "C824",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, TourishRatingModel TourishRatingModel)
        {
            try
            {
                var response = _entityRepository.Update(TourishRatingModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C824",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
