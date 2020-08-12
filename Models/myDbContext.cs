using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Text;

namespace TimeTableManager.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();

        }

        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>().HasData(GetSchedules());
            base.OnModelCreating(modelBuilder);
        }

        private Schedule[] GetSchedules()
        {
            return new Schedule[]
                {
                    new Schedule{ Id=1, Working_days_count=3,Working_days="Monday,Tuesday,Friday",working_time_hrs=5,Working_time_mins=30,Working_duration="One Hour"}
                };
        }
    }

}

 