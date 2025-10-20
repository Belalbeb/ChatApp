
using ChatApi.Hubs;
using ChatApi.Services;
using Scalar.AspNetCore;

namespace ChatApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSingleton<ChatServices>();
            builder.Services.AddSignalR();
            builder.Services.AddCors(e => e.AddPolicy("newPolicy", e =>
            {
                e.WithOrigins("http://localhost:4200").AllowCredentials();
                e.AllowAnyHeader();
                e.AllowAnyMethod();

            }

         ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseStaticFiles();
            app.UseCors("newPolicy");
            

            app.UseHttpsRedirection();

            app.UseAuthorization();
       


            app.MapControllers();
            app.MapHub<ChatHub>("/hubs/chat");

            app.Run();
        }
    }
}
