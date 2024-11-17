using WebApplication1.Model;
using WebApplication1.Model.Connection;

namespace SignalR.Hub.Client
{
    public interface IGuestMessageHubClient
    {
        Task SendMessageToAdmin(Guid adminId, string email, GuestMessageModel message);
        Task SendMessageToUser(Guid adminId, string email, GuestMessageModel message);
        Task SendMessageToBot(Guid? adminId, string email, GuestMessageModel message);
        Task SendMessageToAll(GuestMessageModel message);
        Task SendString(String stringA);
        Task NotifyNewCon(String adminId, AdminMessageConDTOModel adminMessageConDTO);
        Task SendAdminError(Guid userId, string error);

        Task SendGuestError(String email, string error);
    }
}