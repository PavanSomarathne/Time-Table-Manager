using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class ConsecutiveSession
    {
        public int Id { get; set; }
        public String consecutiveId { get; set; }
        public String firstSession { get; set; }
        public String secondSession { get; set; }
    }
}
