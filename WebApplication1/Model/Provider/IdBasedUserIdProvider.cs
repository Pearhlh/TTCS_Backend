using Microsoft.AspNetCore.SignalR;

public class IdBasedUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        //TODO: Implement USERID Mapper Here
        //throw new NotImplementedException();

        return connection.User.Identity.Name;
    }
}