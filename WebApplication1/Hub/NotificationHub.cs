using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SignalR.Hub.Client;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Data.Connection;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Service.InheritanceService;

namespace SignalR.Hub
{
    public class NotificationHub : Hub<INotificationHubClient>
    {
        private readonly MyDbContext _context;
        private readonly ILogger<NotificationHub> logger;
        private readonly UserService _userService;

        public NotificationHub(MyDbContext context, IOptionsMonitor<AppSetting> optionsMonitor, ILogger<NotificationHub> _logger, UserService userService)
        {
            _context = context;
            logger = _logger;
            _userService = userService;
        }

        public async Task SendOffersToAll(NotificationDTOModel notification)
        {
            await Clients.All.SendOffersToAll(notification);
        }

        public async Task SendTotalNumber(Guid userId)
        {
            var notificationCount = _context.Notifications.Where(u => (u.UserReceiveId == userId || u.UserReceiveId == null) && !u.IsRead && !u.IsDeleted).Count();
            await Clients.All.SendString(notificationCount.ToString());
        }

        public async Task SendString(String a)
        {
            await Clients.All.SendString(a);
        }

        public async Task SendOffersToUser(Guid userReceiveId, NotificationDTOModel notification)
        {
            try
            {
                var userSendId = Context.User.FindFirstValue("Id");

                if (userSendId != null && userSendId.Length > 0)
                {
                    if (notification.Id == null)
                    {
                        var notificationEntity = new Notification
                        {
                            UserCreateId = new Guid(userSendId),
                            UserReceiveId = userReceiveId,
                            Content = notification.Content,
                            IsRead = false,
                            IsDeleted = false,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,

                        };
                        _context.Add(notificationEntity);
                    }



                    var connection = _context.NotificationConList.OrderByDescending(connection => connection.CreateDate).FirstOrDefault(u => u.UserId == userReceiveId && u.Connected);
                    if (connection != null)
                    {
                        await Clients.Client(connection.ConnectionID).SendOffersToUser(userReceiveId, notification);
                    }

                }
            }
            catch (Exception ex)
            {
                var userSendId = Context.User.FindFirstValue("Id");
                var connection = _context.NotificationConList.OrderByDescending(connection => connection.CreateDate).FirstOrDefault(u => u.UserId.ToString() == userSendId && u.Connected);
                //await Clients.Client(connection.ConnectionID).SendOffersToUser(userId, null);
                if (connection != null)
                {
                    await Clients.Client(connection.ConnectionID).SendError(new Guid(userSendId), "Lỗi xảy ra: " + ex.ToString());
                }
            }
        }

        public async Task ChangeNotifyToRead(Guid notificationId, Boolean isRead)
        {
            try
            {
                var notification = await _context.Notifications.Where(u => (u.Id == notificationId)).AsSplitQuery().FirstOrDefaultAsync();
                if (notification != null)
                {
                    notification.IsRead = true;
                    await _context.SaveChangesAsync();
                    await Clients.Client(Context.ConnectionId).ChangeNotifyToRead(notificationId, true);
                }
                else
                    await Clients.Client(Context.ConnectionId).ChangeNotifyToRead(notificationId, false);
            }
            catch (Exception ex)
            {
                await Clients.Client(Context.ConnectionId).SendError(notificationId, "Lỗi xảy ra: " + ex.ToString());
            }

        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var token = httpContext.Request.Query["token"];
            var tokenClaim = (ClaimsPrincipal)_userService.checkIfTokenFormIsValid(token).Data;

            var userId = tokenClaim.FindFirstValue("Id");
            var userRole = tokenClaim.FindFirstValue("Role") ?? "";

            logger.LogInformation("user-log:" + token);
            logger.LogDebug("user-log:" + token);
            if (userRole != "Admin" && userRole != "User" && userRole != "AdminManager")
            {
                Context.Abort();
            }
            else
            {
                var user = _context.Users.Include(u => u.NotificationConList)
               .SingleOrDefault(u => u.Id.ToString() == userId);

                if (user != null)
                {

                    var notifyCon = new NotificationCon
                    {
                        UserId = user.Id,
                        ConnectionID = Context.ConnectionId,
                        UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString(),
                        Connected = true,
                        CreateDate = DateTime.UtcNow
                    };

                    _context.Add(notifyCon);


                    await _context.SaveChangesAsync();
                }
            }




            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            var connection = _context.NotificationConList.FirstOrDefault(u => u.ConnectionID == Context.ConnectionId);
            if (connection != null)
                connection.Connected = false;
            await _context.SaveChangesAsync();

            await base.OnDisconnectedAsync(exception);
        }
    }
}