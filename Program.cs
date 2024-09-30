using Serilog;
namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information() // Set the minimum log level
               .WriteTo.Console() // Write logs to the console
               .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day) // Write logs to a file
               .CreateLogger();

            try
            {
                Log.Information("Starting up the application");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensure to flush logs at the end
            }
            var builder = CreateHostBuilder(args);
            builder.Build().Run();
            
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilders => { webBuilders.UseStartup<Startup>(); });
    }
}
