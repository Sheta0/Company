using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Company.PL.Mapping;
using Company.PL.Services;
using Microsoft.EntityFrameworkCore;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Built-in MVC services

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Register DI for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Register DI for EmployeeRepository

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Register DI for CompanyDbContext

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile)); // Register DI for AutoMapper
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile())); // Register DI for AutoMapper
            builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile())); // Register DI for AutoMapper


            //builder.Services.AddScoped();    // Create Object Life Time Per Request - Unreachable Object
            //builder.Services.AddTransient(); // Create Object Life Time Per Operation
            //builder.Services.AddSingleton(); // Create Object Life Time Per Application

            builder.Services.AddScoped<IScopedService, ScopedService>(); // Per Request
            builder.Services.AddTransient<ITransientService, TransientService>(); // Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Per Application


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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
