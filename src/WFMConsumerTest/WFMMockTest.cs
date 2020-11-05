using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using WFMConsumer.Console.Boilerplate.DbHelper;
using WFMConsumer.Console.Boilerplate.Services;
namespace WFMConsumerTest
{
    public class Tests
    {
        private HttpClient client;
        private HttpResponseMessage response;
        private IConfiguration configuration;
        private IDbHelper dbHelper;
        private ILogger<WFMConsumerService> _logger;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient();
            response =  client.GetAsync("http://pizzacabininc.azurewebsites.net/PizzaCabinInc.svc/schedule/2015-12-14").GetAwaiter().GetResult();
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("app-settings.json", false)
            .Build();
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<WFMConsumerService>();
        }

        [Test]
        public void GetResponseIsSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void GetResponseIsJson()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Test]
        public void SaveScheduleChanges()
        {
            var builder = new DbContextOptionsBuilder<ScheduleDbContext>();
            _ = builder.UseInMemoryDatabase(configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
            var options = builder.Options;
            using (var context = new ScheduleDbContext(options))
            {
                dbHelper = new DbHelper(context, _logger);
                JObject json = JObject.Parse(response.Content.ReadAsStringAsync().Result.ToString());
                dbHelper.SaveScheduleData(json);
                Assert.NotNull(context.Schedules.AnyAsync().GetAwaiter().GetResult());
            }
        }

    }

}