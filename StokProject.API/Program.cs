using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StokProject.Repositories.Abstract;
using StokProject.Repositories.Concrete;
using StokProject.Repositories.Context;
using StokProject.Services.Abstract;
using StokProject.Services.Concrete;

namespace StokProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StokProjectContext>(option => 
            {
                option.UseSqlServer("Server=DESKTOP-OEIFO1O\\CAGRISERVER; Database=ApiEmployeeDB; Uid=sa; Pwd=1234;");
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}