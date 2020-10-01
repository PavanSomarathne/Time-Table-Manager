using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class ConsecutiveSession
    {
        public int Id { get; set; }
        //public String consecutiveId { get; set; }
        public Session firstSession { get; set; }
        public Session secondSession { get; set; }
    }
}
