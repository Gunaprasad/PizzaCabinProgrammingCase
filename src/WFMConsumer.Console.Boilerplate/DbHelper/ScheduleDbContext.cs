using Microsoft.EntityFrameworkCore;
using WFMConsumer.Console.Boilerplate.Models;

namespace WFMConsumer.Console.Boilerplate.DbHelper
{
  
    public class ScheduleDbContext : DbContext
    {
        /// <summary>
        ///  Parameterized Constructor for ScheduleDBContext
        /// </summary>
        /// <param name="options"></param>
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Created DbSet  to hold the records
        /// </summary>
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Projection> Projections { get; set; }
        

        /// <summary>
        /// On Creation of Model ths references to the Table present in the 
        /// database.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>().ToTable("Schedule");
            modelBuilder.Entity<Projection>().ToTable("Projection");
        }
    }
}
