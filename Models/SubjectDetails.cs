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
        public float LecHours { get; set; }
        public float TutorialHours { get; set; }
        public float LabHours{ get; set; }
        public float EvalHours { get; set; }
    }
}
