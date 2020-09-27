using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class ParallelSession
    {

        public int Id { get; set; }
        public String parallelId { get; set; }
        public String firstSession { get; set; }
        public String secondSession { get; set; }
        public String thirdSession { get; set; }
    }
}
