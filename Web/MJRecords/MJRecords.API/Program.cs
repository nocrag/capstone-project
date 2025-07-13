
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MJRecords.Repository;
using MJRecords.Service;
using System.Text;

namespace MJRecords.API
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
            builder.Services.AddScoped<IEmployeeReviewService, EmployeeReviewService>();
            builder.Services.AddScoped<IRatingOptionsService, RatingOptionsService>();
            builder.Services.AddScoped<IEmploymentStatusService, EmploymentStatusService>();

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseCors(policy =>
               policy
               .AllowAnyHeader()
               .AllowAnyMethod()
               .WithOrigins("*")
           );

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run("http://0.0.0.0:7030");
        }
    }
}
