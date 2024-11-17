
using WebApplication1.Model;

namespace SignalR.Hub.Client
{
    public interface INotificationHubClient
    {
        Task SendOffersToUser(Guid userId, NotificationDTOModel notification);
        Task SendOffersToAll(NotificationDTOModel notification);
        Task SendString(String stringA);
        Task SendError(Guid userId, string error);
        Task ChangeNotifyToRead(Guid notificationId, Boolean isRead);
    }
}