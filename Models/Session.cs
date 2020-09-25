using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        public int StdntCount { get; set; }

        public int durationinHours{get; set;}

        //one to many
        public Tag tagDSA { get; set; }
        public SubjectDetails subjectDSA { get; set; }

        //getting grpidas and subgrids
        public Student studentDSA { get; set; }

        //many to many
        public virtual ICollection<SessionLecturer> SessionLecturers { get; set; }

        public String lecturesLstByConcadinating { get;set; }

    }
}
