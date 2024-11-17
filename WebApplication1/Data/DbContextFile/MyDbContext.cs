using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Authentication;
using WebApplication1.Data.Chat;
using WebApplication1.Data.Connection;
using WebApplication1.Data.Receipt;
using WebApplication1.Data.RestaurantPlace;
using WebApplication1.Data.RestHouse;
using WebApplication1.Data.Schedule;
using WebApplication1.Data.Transport;

namespace WebApplication1.Data.DbContextFile
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSet
        public DbSet<MovingContact> MovingContactList { get; set; }
        public DbSet<Restaurant> RestaurantList { get; set; }
        public DbSet<RestHouseContact> RestHouseContactList { get; set; }
        public DbSet<EatSchedule> EatSchedules { get; set; }
        public DbSet<MovingSchedule> MovingSchedules { get; set; }
        public DbSet<StayingSchedule> StayingSchedules { get; set; }

        public DbSet<TourishPlan> TourishPlan { get; set; }
        public DbSet<TourishSchedule> TourishScheduleList { get; set; }
        public DbSet<TourishCategory> TourishCategories { get; set; }
        public DbSet<TourishCategoryRelation> TourishCategoryRelations { get; set; }
        public DbSet<TourishInterest> TourishInterests { get; set; }
        public DbSet<TourishComment> TourishComments { get; set; }
        public DbSet<TourishRating> TourishRatings { get; set; }
        public DbSet<ScheduleRating> ScheduleRatings { get; set; }
        public DbSet<ScheduleInterest> ScheduleInterests { get; set; }

        public DbSet<ServiceComment> ServiceComments { get; set; }

        public DbSet<ServiceSchedule> ServiceSchedule { get; set; }

        public DbSet<Instruction> Instructions { get; set; }

        public DbSet<TotalReceipt> TotalReceiptList { get; set; }
        public DbSet<FullReceipt> FullReceiptList { get; set; }

        public DbSet<TotalScheduleReceipt> TotalScheduleReceiptList { get; set; }
        public DbSet<FullScheduleReceipt> FullScheduleReceiptList { get; set; }

        //
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<GuestMessage> GuestMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationFcmToken> NotificationFcmTokens { get; set; }
        public DbSet<NotificationCon> NotificationConList { get; set; }
        public DbSet<UserMessageCon> UserMessageConList { get; set; }
        public DbSet<GuestMessageCon> GuestMessageConList { get; set; }
        public DbSet<AdminMessageCon> AdminMessageConList { get; set; }
        public DbSet<GuestMessageConHistory> GuestMessageConHisList { get; set; }
        public DbSet<SaveFile> SaveFileList { get; set; }

        //
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ReqTemporaryToken> ReqTemporaryTokens { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TourishPlan>(entity =>
            {
                entity.ToTable(nameof(TourishPlan));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.Description).HasDefaultValueSql("''");

                entity.HasMany(e => e.MovingSchedules)
                .WithOne(e => e.TourishPlan)
                .HasForeignKey(e => e.TourishPlanId).IsRequired(false)
                .HasConstraintName("FK_TourishPlan_MovingSchedule").OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(e => e.EatSchedules)
                .WithOne(e => e.TourishPlan)
                .HasForeignKey(e => e.TourishPlanId).IsRequired(false)
                .HasConstraintName("FK_TourishPlan_EatSchedules").OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(e => e.StayingSchedules)
                .WithOne(e => e.TourishPlan)
                .HasForeignKey(e => e.TourishPlanId).IsRequired(false)
                .HasConstraintName("FK_TourishPlan_StayingSchedules").OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(e => e.NotificationList)
                .WithOne(e => e.TourishPlan)
                .HasForeignKey(e => e.TourishPlanId).IsRequired(false)
                .HasConstraintName("FK_TourishPlan_Notification").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MovingSchedule>(entity =>
            {
                entity.ToTable(nameof(MovingSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<EatSchedule>(entity =>
            {
                entity.ToTable(nameof(EatSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<StayingSchedule>(entity =>
            {
                entity.ToTable(nameof(StayingSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<TourishCategory>(entity =>
            {
                entity.ToTable(nameof(TourishCategory));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<TourishSchedule>(entity =>
            {
                entity.ToTable(nameof(TourishSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.TourishPlan)
               .WithMany(e => e.TourishScheduleList)
               .HasForeignKey(e => e.TourishPlanId)
               .IsRequired(false)
               .HasConstraintName("FK_TourishPlan_TourishSchedule").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ServiceSchedule>(entity =>
            {
                entity.ToTable(nameof(ServiceSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.MovingSchedule)
               .WithMany(e => e.ServiceScheduleList)
               .HasForeignKey(e => e.MovingScheduleId)
               .IsRequired(false)
               .HasConstraintName("FK_MovingSchedule_ServiceSchedule").OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.StayingSchedule)
               .WithMany(e => e.ServiceScheduleList)
               .HasForeignKey(e => e.StayingScheduleId)
               .IsRequired(false)
               .HasConstraintName("FK_StayingSchedule_ServiceSchedule").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TourishCategoryRelation>(entity =>
            {
                entity.ToTable(nameof(TourishCategoryRelation));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.TourishPlan)
                .WithMany(e => e.TourishCategoryRelations)
                .HasForeignKey(e => e.TourishPlanId)
                .HasConstraintName("FK_TourishPlan_TourishCategoryRelation").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TourishCategory)
                .WithMany(e => e.TourishCategoryRelations)
                .HasForeignKey(e => e.TourishCategoryId)
                .HasConstraintName("FK_TourishCategory_TourishCategoryRelation").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TourishInterest>(entity =>
            {
                entity.ToTable(nameof(TourishInterest));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.TourishPlan)
                .WithMany(e => e.TourishInterestList)
                .HasForeignKey(e => e.TourishPlanId)
                .HasConstraintName("FK_TourishPlan_TourishInterest").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                .WithMany(e => e.TourishInterests)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_TourishInterest").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ScheduleInterest>(entity =>
            {
                entity.ToTable(nameof(ScheduleInterest));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.MovingSchedule)
                .WithMany(e => e.ScheduleInterestList)
                .HasForeignKey(e => e.MovingScheduleId)
                .IsRequired(false)
                .HasConstraintName("FK_MovingSchedule_ScheduleInterest").OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.StayingSchedule)
                .WithMany(e => e.ScheduleInterestList)
                .HasForeignKey(e => e.StayingScheduleId)
                .IsRequired(false)
                .HasConstraintName("FK_StayingSchedule_ScheduleInterest").OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.User)
                .WithMany(e => e.ScheduleInterestList)
                .HasForeignKey(e => e.UserId)
                .IsRequired(false)
                .HasConstraintName("FK_User_ScheduleInterest");
            });

            modelBuilder.Entity<Instruction>(entity =>
            {
                entity.ToTable(nameof(Instruction));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.MovingSchedule)
                .WithMany(e => e.InstructionList)
                .IsRequired(false)
                .HasForeignKey(e => e.MovingScheduleId)
                .HasConstraintName("FK_MovingSchedule_Instruction").OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.StayingSchedule)
                .WithMany(e => e.InstructionList)
                .IsRequired(false)
                .HasForeignKey(e => e.StayingScheduleId)
                .HasConstraintName("FK_StayingSchedule_Instruction").OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.TourishPlan)
                .WithMany(e => e.InstructionList)
                .IsRequired(false)
                .HasForeignKey(e => e.TourishPlanId)
                .HasConstraintName("FK_TourishPlan_Instruction").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TourishComment>(entity =>
            {
                entity.ToTable(nameof(TourishComment));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.TourishCommentList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_TourishComment").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TourishPlan)
               .WithMany(e => e.TourishCommentList)
               .HasForeignKey(e => e.TourishPlanId)
               .HasConstraintName("FK_TourishPlan_TourishComment").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ServiceComment>(entity =>
            {
                entity.ToTable(nameof(ServiceComment));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.ServiceCommentList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_ServiceComment").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.MovingSchedule)
                .WithMany(e => e.ServiceCommentList)
                .IsRequired(false)
                .HasForeignKey(e => e.MovingScheduleId)
                .HasConstraintName("FK_MovingSchedule_ServiceComment").OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.StayingSchedule)
                .WithMany(e => e.ServiceCommentList)
                .IsRequired(false)
                .HasForeignKey(e => e.StayingScheduleId)
                .HasConstraintName("FK_StayingSchedule_ServiceComment").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TourishRating>(entity =>
            {
                entity.ToTable(nameof(TourishRating));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.TourishRatingList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_TourishRating").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TourishPlan)
               .WithMany(e => e.TourishRatingList)
               .HasForeignKey(e => e.TourishPlanId)
               .HasConstraintName("FK_TourishPlan_TourishRating").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ScheduleRating>(entity =>
            {
                entity.ToTable(nameof(ScheduleRating));
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.ScheduleRatingList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_ScheduleRating");
            });

            modelBuilder.Entity<TotalReceipt>(entity =>
            {
                entity.ToTable(nameof(TotalReceipt));
                entity.HasKey(e => e.TotalReceiptId);
                entity.Property(e => e.CreatedDate).IsRequired().HasDefaultValueSql("getutcdate()");


                entity.HasMany(e => e.FullReceiptList)
               .WithOne(e => e.TotalReceipt)
               .HasForeignKey(e => e.TotalReceiptId)
               .HasConstraintName("FK_TotalReceipt_FullReceipt");

                entity.HasOne(e => e.TourishPlan)
                .WithOne(e => e.TotalReceipt)
                .HasForeignKey<TotalReceipt>(e => e.TourishPlanId)
                .IsRequired(false)
                .HasConstraintName("FK_TourishPlan_TotalReceipt").OnDelete(DeleteBehavior.SetNull);

            });

            modelBuilder.Entity<FullReceipt>(entity =>
            {
                entity.ToTable(nameof(FullReceipt));
                entity.HasKey(e => e.FullReceiptId);
                entity.Property(e => e.CreatedDate).IsRequired().HasDefaultValueSql("getutcdate()");


                entity.HasOne(e => e.TourishSchedule)
               .WithMany(e => e.FullReceiptList)
               .IsRequired(false)
               .HasForeignKey(e => e.TourishScheduleId)
               .HasConstraintName("FK_FullReceipt_TourishSchedule");


            });

            modelBuilder.Entity<TotalScheduleReceipt>(entity =>
            {
                entity.ToTable(nameof(TotalScheduleReceipt));
                entity.HasKey(e => e.TotalReceiptId);
                entity.Property(e => e.CreatedDate).IsRequired().HasDefaultValueSql("getutcdate()");


                entity.HasMany(e => e.FullReceiptList)
               .WithOne(e => e.TotalReceipt)
               .HasForeignKey(e => e.TotalReceiptId)
               .HasConstraintName("FK_TotalScheduleReceipt_FullScheduleReceipt");

            });

            modelBuilder.Entity<MovingSchedule>(entity =>
            {
                entity.ToTable(nameof(MovingSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.TotalReceipt)
                .WithOne(e => e.MovingSchedule)
                .HasForeignKey<TotalScheduleReceipt>(e => e.MovingScheduleId)
                .IsRequired(false)
                .HasConstraintName("FK_MovingSchedule_TotalScheduleReceipt").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<StayingSchedule>(entity =>
            {
                entity.ToTable(nameof(StayingSchedule));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.TotalReceipt)
                .WithOne(e => e.StayingSchedule)
                .HasForeignKey<TotalScheduleReceipt>(e => e.StayingScheduleId)
                .IsRequired(false)
                .HasConstraintName("FK_StayingSchedule_TotalScheduleReceipt").OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<FullScheduleReceipt>(entity =>
            {
                entity.ToTable(nameof(FullScheduleReceipt));
                entity.HasKey(e => e.FullReceiptId);
                entity.Property(e => e.CreatedDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.ServiceSchedule)
               .WithMany(e => e.FullScheduleReceiptList)
               .IsRequired(false)
               .HasForeignKey(e => e.ServiceScheduleId)
               .HasConstraintName("FK_FullScheduleReceipt_ServiceSchedule").OnDelete(DeleteBehavior.SetNull);

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable(nameof(RefreshToken));

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.RefreshTokenList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_RefreshToken");
            });

            modelBuilder.Entity<ReqTemporaryToken>(entity =>
            {
                entity.Property(e => e.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable(nameof(RefreshToken));

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                .WithMany(e => e.RefreshTokenList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_RefreshToken");
            });


            modelBuilder.Entity<UserMessage>(entity =>
            {
                entity.ToTable(nameof(UserMessage));
                entity.HasKey(message => message.Id);
                entity.Property(message => message.UpdateDate).IsRequired().HasDefaultValueSql("getutcdate()");
                entity.Property(message => message.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.UserSent)
                .WithMany(e => e.UserMessageSentList)
                .HasForeignKey(e => e.UserSentId)
                .HasConstraintName("FK_User_SentMessage")
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.UserReceive)
                .WithMany(e => e.UserMessageReceiveList)
                .HasForeignKey(e => e.UserReceiveId)
                .HasConstraintName("FK_UserCon_UserMessage")
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<GuestMessage>(entity =>
            {
                entity.ToTable(nameof(GuestMessage));
                entity.HasKey(message => message.Id);
                entity.Property(message => message.UpdateDate).IsRequired().HasDefaultValueSql("getutcdate()");
                entity.Property(message => message.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.GuestMessageCon)
                .WithMany(e => e.GuestMessages)
                .HasForeignKey(e => e.GuestMessageConId)
                .HasConstraintName("FK_GuestCon_GuestMessage").IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.AdminMessageCon)
                .WithMany(e => e.GuestMessages)
                .HasForeignKey(e => e.AdminMessageConId)
                .HasConstraintName("FK_AdminCon_GuestMessage").IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable(nameof(Notification));
                entity.HasKey(notification => notification.Id);
                entity.Property(notification => notification.UpdateDate).IsRequired().HasDefaultValueSql("getutcdate()");
                entity.Property(notification => notification.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.UserCreator)
                .WithMany(e => e.NotificationCreateList)
                .HasForeignKey(e => e.UserCreateId).IsRequired(false)
                .HasConstraintName("FK_UserCreate_Notification");

                entity.HasOne(e => e.UserReceiver)
               .WithMany(e => e.NotificationReceiveList)
               .HasForeignKey(e => e.UserReceiveId).IsRequired(false)
               .HasConstraintName("FK_UserReceive_Notification");

                entity.HasOne(e => e.MovingSchedule)
              .WithMany(e => e.NotificationList)
              .HasForeignKey(e => e.MovingScheduleId).IsRequired(false)
              .HasConstraintName("FK_MovingSchedule_Notification");

                entity.HasOne(e => e.StayingSchedule)
              .WithMany(e => e.NotificationList)
              .HasForeignKey(e => e.StayingScheduleId).IsRequired(false)
              .HasConstraintName("FK_StayingSchedule_Notification");
            });

            modelBuilder.Entity<NotificationFcmToken>(entity =>
            {
                entity.ToTable(nameof(NotificationFcmToken));
                entity.HasKey(notification => notification.Id);
                entity.Property(notification => notification.UpdateDate).IsRequired().HasDefaultValueSql("getutcdate()");
                entity.Property(notification => notification.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.User)
                .WithOne(e => e.FcmToken)
                .HasForeignKey<NotificationFcmToken>(e => e.UserId)
                .HasConstraintName("FK_UserCreate_NotificationFcmToken");

            });

            modelBuilder.Entity<NotificationCon>(entity =>
            {
                entity.ToTable(nameof(NotificationCon));
                entity.HasKey(notification => notification.Id);
                entity.Property(notification => notification.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.User)
                .WithMany(e => e.NotificationConList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_NotificationCon");
            });

            modelBuilder.Entity<UserMessageCon>(entity =>
            {
                entity.ToTable(nameof(UserMessageCon));
                entity.HasKey(message => message.Id);
                entity.Property(message => message.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.User)
                .WithMany(e => e.UserMessageConList)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_MessageCon");
            });

            modelBuilder.Entity<GuestMessageCon>(entity =>
            {
                entity.ToTable(nameof(GuestMessageCon));
                entity.HasKey(message => message.Id);
                entity.Property(message => message.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasMany(e => e.GuestMessages)
                .WithOne(e => e.GuestMessageCon)
                .HasForeignKey(e => e.GuestMessageConId)
                .HasConstraintName("FK_Guest_MessageCon");

            });

            modelBuilder.Entity<AdminMessageCon>(entity =>
            {
                entity.ToTable(nameof(AdminMessageCon));
                entity.HasKey(message => message.Id);
                entity.Property(message => message.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                entity.HasOne(e => e.Admin)
                .WithMany(e => e.AdminMessageConList)
                .HasForeignKey(e => e.AdminId)
                .HasConstraintName("FK_User_AdminMessageCon");

                entity.HasMany(e => e.GuestMessages)
               .WithOne(e => e.AdminMessageCon)
               .HasForeignKey(e => e.AdminMessageConId)
               .HasConstraintName("FK_Admin_MessageCon");


            });

            modelBuilder.Entity<GuestMessageConHistory>(entity =>
            {
                entity.ToTable(nameof(GuestMessageConHistory));
                entity.HasKey(message => message.Id);
                entity.Property(message => message.CreateDate).IsRequired().HasDefaultValueSql("getutcdate()");

                // Remove the existing relationship with AdminCon
                entity.HasOne(e => e.AdminCon)
                    .WithOne(e => e.GuestMessageConHis)
                     .IsRequired(false)
                    .HasForeignKey<GuestMessageConHistory>(e => e.AdminConId).IsRequired(false)
                    .HasConstraintName("FK_GuestMessageCon_GuestMessageConHis_Admin").OnDelete(DeleteBehavior.ClientSetNull);

                // Define the new relationship with GuestCon
                entity.HasOne(e => e.GuestCon)
                    .WithOne(e => e.GuestMessageConHis)
                    .IsRequired(false)
                    .HasForeignKey<GuestMessageConHistory>(e => e.GuestConId)
                    .HasConstraintName("FK_GuestMessageCon_GuestMessageConHis_Guest").OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SaveFile>(entity =>
            {
                entity.ToTable(nameof(SaveFile));
                entity.Property(saveFile => saveFile.CreatedDate).IsRequired().HasDefaultValueSql("getutcdate()");
            });
        }
    }
}
