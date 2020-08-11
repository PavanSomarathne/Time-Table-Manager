using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Text;

namespace TimeTableManager.Models
{
    class myDbContext : DbContext
    {
        public myDbContext(DbContextOptions<myDbContext> options) : base(options)
        {
            Database.EnsureCreated();

        }

        public DbSet<Schedule> schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>().HasData(GetSchedules());
            base.OnModelCreating(modelBuilder);
        }

        private Schedule[] GetSchedules()
        {
            return new Schedule[]
                {
                    new Schedule{ Id=1, Working_days_count=3,Working_days="Moday,Tuesday,Friday",working_time_hrs=5,Working_days_mins=30,Working_duration=1}
                };
        }
    }

}

 