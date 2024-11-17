
using WebApplication1.Model;

namespace SignalR.Hub.Client
{
    public interface IMessageHubClient
    {
        Task SendMessageToUser(Guid userId, UserMessageModel message);
        Task SendMessageToAll(UserMessageModel message);
        Task SendString(String stringA);
        Task SendError(Guid userId, string error);
    }
}