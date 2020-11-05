using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using WFMConsumer.Console.Boilerplate.DbHelper;
using Microsoft.Extensions.Options;

namespace WFMConsumer.Console.Boilerplate.Services
{
       
    public class WFMConsumerService : IWFMConsumerService
    {
        private readonly ILogger<WFMConsumerService> _logger;
        private readonly IDbHelper _dbHelper;
        private readonly IOptions<AppSettings> _config;
        /// <summary>
        /// Takes input of logger, DBHelper object to be used later in the solution
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbHelper"></param>
        /// <param name="config"></param>
        public WFMConsumerService(ILogger<WFMConsumerService> logger, IDbHelper dbHelper, IOptions<AppSettings> config)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _config = config;
        }

        public async Task<bool> Run()
        {
            bool success = true;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                   
                    var jsonResult = await client.GetStringAsync("http://pizzacabininc.azurewebsites.net/PizzaCabinInc.svc/schedule/" + _config.Value.Date + "");
                    if (jsonResult != null)
                    {
                        success = true;
                        JObject json = JObject.Parse(jsonResult);
                        _dbHelper.SaveScheduleData(json);
                    }
                    else
                        success = false;
                }
            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);
            }
            
            return success;


            ///
            /// We can also try to consume the service by adding it as part of the solution, following code
            /// shows how it can be done.
            ///

            //BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            //PizzaCabinIncClient _client = new PizzaCabinIncClient(binding, new System.ServiceModel.EndpointAddress("http://pizzacabininc.azurewebsites.net/PizzaCabinInc.svc?wsdl"));
            //var jsonResult =  await _client.ScheduleAsync("2015-12-14");
            //_logger.LogWarning($"Wow! We are now in the test service of: {_config.ConsoleTitle}");
            //_logger.LogWarning("json Result Length" + jsonResult);
            //return true;
        }
    }
}
