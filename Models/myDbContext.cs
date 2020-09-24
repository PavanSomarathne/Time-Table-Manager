﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<Building> Buildings { get; set; }
        public DbSet<LecturerDetails> LectureInformation { get; set; }
        public DbSet<SubjectDetails> SubjectInformation { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Lecturers_NotAvailable> Lecturers_NotAvailables { get; set; }
        public DbSet<Sessions_NotAvailable> Sessions_NotAvailables { get; set; }
        public DbSet<Groups_NotAvailable> Groups_NotAvailables { get; set; }
        public DbSet<SubGroups_NotAvailable> SubGroups_NotAvailables { get; set; }


 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
            modelBuilder.Entity<RoomLecturer>()
                .HasKey(rl => new { rl.RoomId, rl.LecturerId });

            modelBuilder.Entity<RoomLecturer>()
                .HasOne(rl => rl.Room)
                .WithMany(p => p.RoomLecturers)
                .HasForeignKey(rl => rl.RoomId);

            modelBuilder.Entity<RoomLecturer>()
                .HasOne(rl => rl.Lecturer)
                .WithMany(c => c.RoomLecturers)
                .HasForeignKey(rl => rl.LecturerId);

            modelBuilder.Entity<Building>()
                .HasMany(q => q.LecturesDSA)
                .WithOne(k => k.BuildinDSA);

            modelBuilder.Entity<Schedule>().HasData(GetSchedules());
            modelBuilder.Entity<SubjectDetails>();
            //modelBuilder.Entity<Building>().HasData(GetBuildings());

            modelBuilder.Entity<Building>()
            .HasMany(c => c.RoomsAS)
            .WithOne(e => e.BuildingAS);

            base.OnModelCreating(modelBuilder);

        }
        private Schedule[] GetSchedules()
        {
            return new Schedule[]
                {
                    new Schedule{ Id=1, Working_days_count=3,Working_days="Monday,Tuesday,Friday",working_time_hrs=5,Working_time_mins=30,start_time="8:00 AM",Working_duration="One Hour"}
                };
        }

    


        private SubjectDetails[] GetSubjectDetails()
        {
            return new SubjectDetails[]
                {
                    new SubjectDetails{ Id=1, OfferedYear="4thYear",OfferedSemester="2nd Semester",SubjectName="OOP",SubjectCode="SE1258",LecHours=14,TutorialHours=10,LabHours=8,EvalHours=2}
                };
        }



    

        private Building[] GetBuildings()
        {
            return new Building[]
                {
                    new Building{ Id = -1,Bid="NB", Name="New Building"}
                };
        }

        private Room[] GetRooms()
        {
            return new Room[]
                {
                    new Room{Id = -1,Rid="B02", Type="Lab",BuildingAS = new Building{ Id = -1,Bid="NB", Name="New Building"}}
                };
        }

        private Student[] GetStudents()
        {
            return new Student[]
            {
                new Student { Id=-1 , accYrSem = "Y3.S1" , programme ="CSSE" , groupNo = 03 , groupId="Y3.S1.CSSE.03" , subGroupNo = 1},

            };
        }

        private Tag[] GetTags()
        {
            return new Tag[]
            {
                new Tag { Id=-1 , tags = "Lecture"},

            };
        }
    }

}

