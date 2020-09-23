using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class RoomLecturer
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int LecturerId { get; set; }
        public LecturerDetails Lecturer { get; set; }
    }
}
