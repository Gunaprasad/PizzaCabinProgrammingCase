using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WFMConsumer.Console.Boilerplate.Services;

namespace WFMConsumer.Console.Boilerplate.DbHelper
{
    public class DbHelper : IDbHelper
    {
        private readonly ScheduleDbContext _dbContext;
        private readonly ILogger<WFMConsumerService> _logger;

        public DbHelper(ScheduleDbContext dbContext, ILogger<WFMConsumerService> logger)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
            _logger = logger;
        }

        /// <summary>
        /// Implementation of Save Schedule
        /// This methods helps is storing the Schedule Data in the database
        /// </summary>
        /// <param name="jsonResult"></param>
        public void SaveScheduleData(JObject jsonResult)
        {

            JToken entireJson = JToken.Parse(jsonResult.ToString());

            int count = 0;
            int projectionCount = 0;
            foreach (var item in entireJson["ScheduleResult"].Values().Values())
            {
                try
                {
                    _dbContext.Schedules.Add(

                                new Models.Schedule
                                {
                                    ContractTimeMinutes = (int)item["ContractTimeMinutes"],
                                    Date = ((DateTime)item["Date"]).ToUniversalTime(),
                                    IsFullDayAbsence = (bool)item["IsFullDayAbsence"],
                                    Name = item["Name"].ToString(),
                                    PersonId = item["PersonId"].ToString()
                                }

                    );

                    _dbContext.SaveChanges();
                    count++;
                }
                catch (Exception)
                {
                    _logger.LogError("Schedule failed to save for Person Id {1}", item["PersonId"].ToString());
                    continue;
                }
                
                if (item["Projection"] is JArray && item["Projection"].HasValues)
                {
                    foreach (var projectionItem in item["Projection"])
                    {
                        try
                        {
                                _dbContext.Projections.Add(
                                new Models.Projection
                                  {
                                      Color = projectionItem["Color"].ToString(),
                                      Description = projectionItem["Description"].ToString(),
                                      minutes = (int)projectionItem["minutes"],
                                      PersonId = item["PersonId"].ToString(),
                                      Start = ((DateTime)projectionItem["Start"]).ToUniversalTime(),
                                      Id = Guid.NewGuid()
                                  }
                            );
                            projectionCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Projection failed to save for Person Id {1}", item["PersonId"].ToString());
                        }
                    }
                }

                _dbContext.SaveChanges();
            }
            _logger.LogWarning("Total Number of Schedules downloaded {0} along with Total Projections {1}", count, projectionCount);
        }
    }
}
