using BD.Repositories.Implementation;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using BD.Services.Implementation;
using BD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

using BD.Repositories.Models.Entities;
using BD.Repositories.Interfaces;
using BD.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;

namespace BD.RazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);            // Add services to the container.
            builder.Services.AddRazorPages();


            builder.Services.AddScoped<IStatusNotificationRepository, StatusNotificationRepository>();
            builder.Services.AddScoped<IStatusNotificationService, StatusNotificationService>();
            //Notification
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            //BlogPost
            builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            builder.Services.AddScoped<IBlogPostService, BlogPostService>();
            //BloodInventory
            builder.Services.AddScoped<IBloodInventoryRepository, BloodInventoryRepository>();
            builder.Services.AddScoped<IBloodInventoryService, BloodInventoryService>();
            //StatusBloodInventory
            builder.Services.AddScoped<IStatusBloodInventoryRepository, StatusBloodInventoryRepository>();
            builder.Services.AddScoped<IStatusBloodInventoryService, StatusBloodInventoryService>();
            //BloodCompatibility
            builder.Services.AddScoped<IBloodCompatibilityRepository, BloodCompatibilityRepository>();
            builder.Services.AddScoped<IBloodCompatibilityService, BloodCompatibilityService>();
            //StatusBloodDonor
            builder.Services.AddScoped<IStatusBloodDonorRepository, StatusBloodDonorRepository>();
            builder.Services.AddScoped<IStatusBloodDonorService, StatusBloodDonorService>();
            //BloodRequest
            builder.Services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();
            builder.Services.AddScoped<IBloodRequestService, BloodRequestService>();
            //StatusBloodRequest
            builder.Services.AddScoped<IStatusBloodRequestRepository, StatusBloodRequestRepository>();
            builder.Services.AddScoped<IStatusBloodRequestService, StatusBloodRequestService>();
            //User
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            //Role
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            //DonorAvailability
            builder.Services.AddScoped<IDonorAvailabilityRepository, DonorAvailabilityRepository>();
            builder.Services.AddScoped<IDonorAvailabilityService, DonorAvailabilityService>();
            //MedicalFacility
            builder.Services.AddScoped<IMedicalFacilityRepository, MedicalFacilityRepository>();
            builder.Services.AddScoped<IMedicalFacilityService, MedicalFacilityService>();
            //DonationHistory
            builder.Services.AddScoped<IDonationHistoryRepository, DonationHistoryRepository>();
            builder.Services.AddScoped<IDonationHistoryService, DonationHistoryService>();

            builder.Services.AddDbContext<BloodDonationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                        
            // Add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            // Add Entity Framework
            builder.Services.AddDbContext<BloodDonationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));              // Add Repository services
            builder.Services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();
            builder.Services.AddScoped<IDonorAvailabilityRepository, DonorAvailabilityRepository>();
            builder.Services.AddScoped<IBloodInventoryRepository, BloodInventoryRepository>();
            builder.Services.AddScoped<IMedicalFacilityRepository, MedicalFacilityRepository>();
            builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            builder.Services.AddScoped<IDonationHistoryRepository, DonationHistoryRepository>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            // Add session middleware before authorization
            app.UseSession();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
