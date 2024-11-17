using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Connection;
using WebApplication1.Data.DbContextFile;

namespace WebApplication1.Trigger
{
    public class MessageConTrigger : IAfterSaveTrigger<UserMessageCon>
    {

        private readonly MyDbContext _dbContext;

        public MessageConTrigger(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Task AfterSave(ITriggerContext<UserMessageCon> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added)
            {
                var connectionCount = this._dbContext.UserMessageConList.Where(messageCon => messageCon.UserId == context.Entity.UserId).Count();
                if (connectionCount > 5)
                {
                    var conList = this._dbContext.UserMessageConList
                        .Where(messageCon => messageCon.UserId == context.Entity.UserId)
                        .OrderBy(messageCon => messageCon.CreateDate)
                        .Take(connectionCount - 5)
                        .ToList();
                    this._dbContext.RemoveRange(conList);
                }

                this._dbContext.UserMessageConList.Where(u => u.UserId == context.Entity.UserId && u.Connected)
                    .OrderByDescending(u => u.CreateDate)
                    .Skip(1)
                    .ExecuteUpdate(f => f.SetProperty(x => x.Connected, false));
            }
            return Task.CompletedTask;
        }

    }
}
