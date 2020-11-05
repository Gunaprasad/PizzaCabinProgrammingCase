using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Reflection;
using WFMConsumer.Console.Boilerplate.DbHelper;
using WFMConsumer.Console.Boilerplate.Services;
namespace WFMConsumer.Console.Boilerplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            
            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            // add logging
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), configuration.GetSection("Logging:LogPath").Value)))
            {
                File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), configuration.GetSection("Logging:LogPath").Value));
            }
            // used serilog for logging 
            var serilogLogger = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), configuration.GetSection("Logging:LogPath").Value))
            .CreateLogger();
            // adding serilog to the servicecollection    
            serviceCollection.AddLogging(builder =>
            {
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
            // add services
            serviceCollection.AddTransient<IWFMConsumerService, WFMConsumerService>();
            // add dbherlper
            serviceCollection.AddTransient<IDbHelper, DbHelper.DbHelper>();
            // add dbcontext class
            serviceCollection.AddDbContext<ScheduleDbContext>(options =>
            options.UseSqlServer(configuration.GetSection("ConnectionStrings:DefaultConnection").Value));
            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}
