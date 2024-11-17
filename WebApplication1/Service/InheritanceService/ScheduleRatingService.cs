using TourishApi.Service.Interface;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo;

namespace TourishApi.Service.InheritanceService
{
    public class ScheduleRatingService
        : IBaseService<ScheduleRatingRepository, ScheduleRatingModel>
    {
        private readonly ScheduleRatingRepository _entityRepository;

        public ScheduleRatingService(ScheduleRatingRepository airPlaneRepository)
        {
            _entityRepository = airPlaneRepository;
        }

        public Response CreateNew(ScheduleRatingModel entityModel)
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
                    MessageCode = "C834",
                    Error = ex.Message
                };
            }
        }

        public Response SendRating(ScheduleRatingModel entityModel)
        {
            try
            {
                var existRating = _entityRepository.getByUserIdAndScheduleId(entityModel.UserId, entityModel.ScheduleId, entityModel.ScheduleType);
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
                    MessageCode = "C834",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I833" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C834",
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
                    MessageCode = "C834",
                    Error = ex.Message
                };
                return response;
            }
        }
        public Response GetAllByScheduleId(Guid scheduleId, ScheduleType scheduleType)
        {
            try
            {
                var entityList = _entityRepository.GetAllByScheduleId(scheduleId, scheduleType);
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C834",
                    Error = ex.Message
                };
                return response;
            }
        }
        public Response getByUserIdAndScheduleId(Guid UserId, Guid ScheduleId, ScheduleType scheduleType)
        {
            var entity = _entityRepository.getByUserIdAndScheduleId(UserId, ScheduleId, scheduleType);

            if (entity == null)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C830"
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
                    var response = new Response { resultCd = 1, MessageCode = "C830", };
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
                    MessageCode = "C834",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, ScheduleRatingModel ScheduleRatingModel)
        {
            try
            {
                var response = _entityRepository.Update(ScheduleRatingModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C834",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
