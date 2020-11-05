namespace WFMConsumer.Console.Boilerplate.DbHelper
{
    public interface IDbHelper
    {
        /// <summary>
        /// Takes input parameter as Json object
        /// </summary>
        /// <param name="jsonScheduleResult"></param>
        void SaveScheduleData(Newtonsoft.Json.Linq.JObject jsonScheduleResult);
    }
}
