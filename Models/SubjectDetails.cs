using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class SubjectDetails
    {
        public int Id { get; set; }
        public String SubjectName { get; set; }
        public String SubjectCode { get; set; }
        public String OfferedYear { get; set; }
        public String OfferedSemester{ get; set; }
        public int LecHours { get; set; }
        public int TutorialHours { get; set; }
        public int LabHours{ get; set; }
        public int EvalHours { get; set; }
    }
}
