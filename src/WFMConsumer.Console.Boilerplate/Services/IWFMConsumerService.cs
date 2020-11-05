using System.Threading.Tasks;

namespace WFMConsumer.Console.Boilerplate.Services
{
    public interface IWFMConsumerService
    {
        /// <summary>
        /// Calls the run method of the service
        /// </summary>
        /// <returns></returns>
        Task<bool> Run();
    }
}
