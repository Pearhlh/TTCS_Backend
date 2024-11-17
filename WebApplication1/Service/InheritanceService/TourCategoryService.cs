using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using TourishApi.Service.Interface;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.InheritanceRepo;

namespace TourishApi.Service.InheritanceService
{
    public class TourishCategoryService
        : IBaseService<TourishCategoryRepository, TourishCategoryModel>
    {
        private readonly TourishCategoryRepository _entityRepository;
        private readonly IDatabase _redisDatabase;

        public TourishCategoryService(TourishCategoryRepository airPlaneRepository, IConnectionMultiplexer connectionMultiplexer)
        {
            _entityRepository = airPlaneRepository;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public Response CreateNew(TourishCategoryModel entityModel)
        {
            try
            {
                var entityExist = _entityRepository.getByName(entityModel.Name);

                if (entityExist.Data == null)
                {
                    var response = _entityRepository.Add(entityModel);

                    return (response);
                }
                else
                {
                    var response = new Response { resultCd = 1, MessageCode = "C421", };
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    resultCd = 1,
                    MessageCode = "C424",
                    Error = ex.Message
                };
            }
        }

        public Response DeleteById(Guid id)
        {
            try
            {
                _entityRepository.Delete(id);
                var response = new Response { resultCd = 0, MessageCode = "I423" };
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C424",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response GetAll(
            string? search,
            int? type,
            string? sortBy,
            string? sortDirection,
            int page = 1,
            int pageSize = 5
        )
        {
            try
            {
                var entityList = _entityRepository.GetAll(
                    search,
                    type,
                    sortBy,
                    sortDirection,
                    page,
                    pageSize
                );
                return entityList;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C424",
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
                    var response = new Response { resultCd = 1, MessageCode = "C420", };
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
                    MessageCode = "C424",
                    Error = ex.Message
                };
                return response;
            }
        }

        public Response UpdateEntityById(Guid id, TourishCategoryModel TourishCategoryModel)
        {
            try
            {
                var response = _entityRepository.Update(TourishCategoryModel);

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C424",
                    Error = ex.Message
                };
                return response;
            }
        }

        public async Task<Response> clientGetAll(string? search, int? type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            try
            {
                String cacheKey =
                        $"tourish_category_list_{search ?? ""}_{type ?? 0}_{sortBy ?? ""}_{sortDirection ?? ""}_page_{page}_pageSize_{pageSize}";
                string cachedValue = await _redisDatabase.StringGetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedValue))
                {
                    var resultCache =
                        JsonConvert.DeserializeObject<WebApplication1.Model.VirtualModel.Response>(
                            cachedValue
                        );
                    if (resultCache != null)
                    {
                        return resultCache;
                    }
                }

                var result = _entityRepository.clientGetAll(
                search,
                type,
                sortBy,
                sortDirection,
                page,
                pageSize
            );

                string resultJson = JsonConvert.SerializeObject(
                    result,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                );

                // Cache the result in Redis
                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    resultJson,
                    TimeSpan.FromMinutes(60)
                ); // Cache for 10 minutes

                return result;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    resultCd = 1,
                    MessageCode = "C424",
                    Error = ex.Message
                };
                return response;
            }
        }
    }
}
