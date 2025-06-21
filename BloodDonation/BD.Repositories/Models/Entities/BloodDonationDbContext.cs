using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BD.Repositories.Models.Entities;

public partial class BloodDonationDbContext : DbContext
{
    public BloodDonationDbContext()
    {
    }

    public BloodDonationDbContext(DbContextOptions<BloodDonationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<BloodCompatibility> BloodCompatibilities { get; set; }

    public virtual DbSet<BloodInventory> BloodInventories { get; set; }

    public virtual DbSet<BloodRequest> BloodRequests { get; set; }

    public virtual DbSet<DonationHistory> DonationHistories { get; set; }

    public virtual DbSet<DonorAvailability> DonorAvailabilities { get; set; }

    public virtual DbSet<MedicalFacility> MedicalFacilities { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StatusBloodDonor> StatusBloodDonors { get; set; }

    public virtual DbSet<StatusBloodInventory> StatusBloodInventories { get; set; }

    public virtual DbSet<StatusBloodRequest> StatusBloodRequests { get; set; }

    public virtual DbSet<StatusNotification> StatusNotifications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-BFQJI45M\\THANGND_SE183829;Initial Catalog=BloodDonationDB;Persist Security Info=True;User ID=sa;Password=12345;Encrypt=False");

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnection"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__BlogPost__3ED7876681296E8D");

            entity.ToTable("BlogPost");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.IsDocument)
                .HasDefaultValue(false)
                .HasColumnName("is_Document");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(true)
                .HasColumnName("is_Published");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Author).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogPost__author__00200768");
        });

        modelBuilder.Entity<BloodCompatibility>(entity =>
        {
            entity.HasKey(e => e.CompatibilityId).HasName("PK__BloodCom__AA12AC9993C7D67D");

            entity.ToTable("BloodCompatibility");

            entity.Property(e => e.CompatibilityId).HasColumnName("compatibility_id");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(50)
                .HasColumnName("component_type");
            entity.Property(e => e.DonorBloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("donor_blood_type");
            entity.Property(e => e.IsCompatible).HasColumnName("is_compatible");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.RecipientBloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("recipient_blood_type");
        });

        modelBuilder.Entity<BloodInventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__BloodInv__B59ACC4921691C91");

            entity.ToTable("BloodInventory");

            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("blood_type");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(50)
                .HasColumnName("component_type");
            entity.Property(e => e.ExpiredDate).HasColumnName("expired_date");
            entity.Property(e => e.FacilityId).HasColumnName("facility_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_updated");
            entity.Property(e => e.StatusInventoryId).HasColumnName("status_inventory_id");

            entity.HasOne(d => d.Facility).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodInve__facil__797309D9");

            entity.HasOne(d => d.StatusInventory).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.StatusInventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodInve__statu__7B5B524B");
        });

        modelBuilder.Entity<BloodRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BloodReq__18D3B90F70C77E71");

            entity.ToTable("BloodRequest");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("blood_type");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(50)
                .HasColumnName("component_type");
            entity.Property(e => e.FulfilledDate)
                .HasColumnType("datetime")
                .HasColumnName("fulfilled_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("request_date");
            entity.Property(e => e.StatusRequestId).HasColumnName("status_request_id");
            entity.Property(e => e.UrgencyLevel)
                .HasMaxLength(20)
                .HasColumnName("urgency_level");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.StatusRequest).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.StatusRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodRequ__statu__60A75C0F");

            entity.HasOne(d => d.User).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodRequ__user___5CD6CB2B");
        });

        modelBuilder.Entity<DonationHistory>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Donation__296B91DC2EA6D4FA");

            entity.ToTable("DonationHistory");

            entity.Property(e => e.DonationId)
                .ValueGeneratedNever()
                .HasColumnName("donation_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("blood_type");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(50)
                .HasColumnName("component_type");
            entity.Property(e => e.DonationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("donation_date");
            entity.Property(e => e.FacilityId).HasColumnName("facility_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Facility).WithMany(p => p.DonationHistories)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonationH__facil__70DDC3D8");

            entity.HasOne(d => d.Request).WithMany(p => p.DonationHistories)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonationH__reque__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.DonationHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonationH__user___6EF57B66");
        });

        modelBuilder.Entity<DonorAvailability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PK__DonorAva__86E3A801CD501914");

            entity.ToTable("DonorAvailability");

            entity.Property(e => e.AvailabilityId).HasColumnName("availability_id");
            entity.Property(e => e.AvailableDate).HasColumnName("available_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.LastDonationDate).HasColumnName("last_donation_date");
            entity.Property(e => e.RecoveryReminderDate).HasColumnName("recovery_reminder_date");
            entity.Property(e => e.StatusDonorId).HasColumnName("status_donor_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.StatusDonor).WithMany(p => p.DonorAvailabilities)
                .HasForeignKey(d => d.StatusDonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonorAvai__statu__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.DonorAvailabilities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonorAvai__user___6754599E");
        });

        modelBuilder.Entity<MedicalFacility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__MedicalF__B2E8EAAE8DF9F91D");

            entity.ToTable("MedicalFacility");

            entity.Property(e => e.FacilityId).HasColumnName("facility_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FFC2FEAB9");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .HasColumnName("message");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sent_at");
            entity.Property(e => e.StatusNotificationId).HasColumnName("status_notification_id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.StatusNotification).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.StatusNotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__statu__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__user___5441852A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC782C65DC");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StatusBloodDonor>(entity =>
        {
            entity.HasKey(e => e.StatusDonorId).HasName("PK__StatusBl__DEDAE0F7C8C78C25");

            entity.ToTable("StatusBloodDonor");

            entity.Property(e => e.StatusDonorId).HasColumnName("status_donor_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<StatusBloodInventory>(entity =>
        {
            entity.HasKey(e => e.StatusInventoryId).HasName("PK__StatusBl__794D33277FCFC34E");

            entity.ToTable("StatusBloodInventory");

            entity.Property(e => e.StatusInventoryId).HasColumnName("status_inventory_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<StatusBloodRequest>(entity =>
        {
            entity.HasKey(e => e.StatusRequestId).HasName("PK__StatusBl__BB59962BCFC549EA");

            entity.ToTable("StatusBloodRequest");

            entity.Property(e => e.StatusRequestId).HasColumnName("status_request_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<StatusNotification>(entity =>
        {
            entity.HasKey(e => e.StatusNotificationId).HasName("PK__StatusNo__F90F86D594BE3DF0");

            entity.ToTable("StatusNotification");

            entity.Property(e => e.StatusNotificationId).HasColumnName("status_notification_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F4FE76943");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.BloodType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("blood_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_Deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__role_id__4E88ABD4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
