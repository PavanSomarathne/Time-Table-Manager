using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
   public class RoomSubject
    {
        public int SubjectId { get; set; }
        public SubjectDetails Subject { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
