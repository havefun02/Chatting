using App.Core;
using App.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net.WebSockets;
public class Startup
{
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IPaginationService<>), typeof(OffsetPaginationService<>));
        services.AddScoped(typeof(IFilterService<>), typeof(FilterService<>));
        services.AddScoped(typeof(ISortingService<>), typeof(SortingService<>));
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IAppService, AppService>();

        services.AddSingleton<SocketHandler>();
        services.AddSingleton<SocketManager>();



        services.AddSingleton<IMapper>(provider =>
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return configuration.CreateMapper();
        });
        services.AddControllersWithViews();
        
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime appLifetime)
    {

        appLifetime.ApplicationStopping.Register(async () =>
        {
            var socketManager = app.ApplicationServices.GetService<SocketManager>();

            if (socketManager != null)
            {
                Log.Information("Close app");
                await socketManager.ClearAllConnections();
            }
        });
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseMiddleware<LoggingMiddleware>();
        app.UseRouting();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseWebSockets();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}"); // Default route
        });
            
    }
}
