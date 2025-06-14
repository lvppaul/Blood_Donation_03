using BD.Repositories.Implementation;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using BD.Services.Implementation;
using BD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BD.Mvcs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
                options.Cookie.HttpOnly = true; // For security
                options.Cookie.IsEssential = true; // Ensure session cookie is always created
            });
            builder.Services.AddControllersWithViews();
            //StatusNotification
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
            //builder.Services.AddScoped<IBloodRequestService, BloodRequestService>();
            //StatusBloodRequest
            builder.Services.AddScoped<IStatusBloodRequestRepository, StatusBloodRequestRepository>();
            builder.Services.AddScoped<IStatusBloodRequestService, StatusBloodRequestService>();
            //User
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddDbContext<BloodDonationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
