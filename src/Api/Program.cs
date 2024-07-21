using Microsoft.AspNetCore;

namespace Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var factory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = factory.CreateLogger("Program");

            try
            {
                logger.LogInformation("Starting application");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch
            {
                logger.LogCritical("Application stopped by exception");
                throw;
            }
            finally
            {
                logger.LogInformation("Server shutting down");
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
