using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WFMConsumer.Console.Boilerplate.Models;
using WFMConsumer.Console.Boilerplate.Services;

namespace WFMConsumer.Console.Boilerplate
{
    public class App
    {
        private readonly IWFMConsumerService _testService;
        private readonly ILogger<App> _logger;
       
        public App(IWFMConsumerService testService,
                      ILogger<App> logger)
        {
            _testService = testService;
            _logger = logger;
    
        }

        public void Run()
        {

            try
            {
               bool result  =_testService.Run().GetAwaiter().GetResult();
               if(result)
                {
                    _logger.LogInformation("Schedule Data downloaded sucessfully");
                }
               else
                {
                    _logger.LogInformation("Schedule Data Failed to download");
                }
            }
            catch (System.Exception ex)
            {
                var message = ex.Message;
                
            }
            
        }
    }
}
