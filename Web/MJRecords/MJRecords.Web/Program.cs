using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MJRecords.Repository;
using MJRecords.Service;
using System.Text;

namespace MJRecords.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DAL DI
            builder.Services.AddScoped<IDataAccess, DataAccess>();

            /* DI HR */
            // REPO DI
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddScoped<IJobRepo, JobRepo>();
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            builder.Services.AddScoped<IPurchaseOrderRepo, PurchaseOrderRepo>();
            builder.Services.AddScoped<IPurchaseOrderItemRepo, PurchaseOrderItemRepo>();
            builder.Services.AddScoped<ILoginRepo, LoginRepo>();
            builder.Services.AddScoped<IEmployeeReviewRepo, EmployeeReviewRepo>();
            builder.Services.AddScoped<IRatingOptionsRepo, RatingOptionsRepo>();
            builder.Services.AddScoped<IEmploymentStatusRepo, EmploymentStatusRepo>();

            // SERVICE DI
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            builder.Services.AddScoped<IPurchaseOrderItemService, PurchaseOrderItemService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ITokenService, TokenService>();


            builder.Services.AddSingleton(new SmtpSettings
            {
                Host = "localhost",
                Port = 25,
                Username = "admin@admin.com",
                Password = "",
                From = "mjrecords@noreply.com",
                EnableSsl = false
            });

            builder.Services.AddScoped<IEmailService, EmailService>();


            builder.Services.AddScoped<IEmployeeReviewService, EmployeeReviewService>();
            builder.Services.AddScoped<IRatingOptionsService, RatingOptionsService>();
            builder.Services.AddScoped<IEmploymentStatusService, EmploymentStatusService>();

            builder.Services.AddSession();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                string? jwtKey = builder.Configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("Jwt:Key is not configured.");
                }

                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Adding Authorization

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CEO", policy => policy.RequireRole("CEO"));
                options.AddPolicy("HR Supervisor", policy => policy.RequireRole("HR Supervisor"));
                options.AddPolicy("Regular Supervisor", policy => policy.RequireRole("Regular Supervisor"));
                options.AddPolicy("HR Employee", policy => policy.RequireRole("HR Employee"));
                options.AddPolicy("Regular Employee", policy => policy.RequireRole("Regular Employee"));
            });


            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
