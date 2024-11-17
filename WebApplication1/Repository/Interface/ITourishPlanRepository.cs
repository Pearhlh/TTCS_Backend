using WebApplication1.Data;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;

namespace WebApplication1.Repository.Interface
{
    public interface ITourishPlanRepository
    {
        Response GetAll(string? search, string? category, string? categoryString, string? startingPoint, string? endPoint, string? startingDate, double? priceFrom,
            double? priceTo, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5);
        Response GetAllWithAuthority(string? search, string? category, string? categoryString, string? startingPoint, string? endPoint, string? startingDate, double? priceFrom,
           double? priceTo, string? sortBy, string? sortDirection, string? userId, int page = 1, int pageSize = 5);
        Response getById(Guid id);
        Response clientGetById(Guid id);
        Response getByName(String TourName);
        Task<List<TourishInterest>> getTourInterest(Guid id);
        Task<Response> Add(TourishPlanInsertModel entityModel, String id);
        Task<Response> Update(TourishPlanUpdateModel entityModel, String id);
        Task<string> getDescription(string containerName, string blobName);

        Task<Boolean> checkArrangeScheduleFromUser(String email, Guid tourishPlanId, List<string> scheduleList);
        Response Delete(Guid id);
        Response getTourInterest(Guid tourId, Guid userId);
        Task<Response> setTourInterest(Guid tourId, Guid userId, InterestStatus interestStatus);
        Response getTopTourRating();
    }
}
