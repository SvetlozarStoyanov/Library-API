using Library.API.Extensions;
using Library.API.Filters;
using Library.API.Middlewares;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDB")));

            builder.Services.AddApplicationServices();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //    app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

