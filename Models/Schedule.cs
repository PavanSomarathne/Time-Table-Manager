using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int Working_days_count { get; set; }
        public String Working_days { get; set; }
        public int working_time_hrs { get; set; }

        public int Working_time_mins { get; set; }
        public int Working_duration { get; set; }


       
    }
}
