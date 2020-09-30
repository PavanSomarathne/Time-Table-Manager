using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        public int StdntCount { get; set; }

        public String GroupType { get; set; }

        public String Year { get; set; }
        public int durationinHours{get; set;}

        //one to many
        public Tag tagDSA { get; set; }
        public SubjectDetails subjectDSA { get; set; }

        public Room Room { get; set; }
        //getting grpidas and subgrids
        public Student studentDSA { get; set; }

        //many to many
        public virtual ICollection<SessionLecturer> SessionLecturers { get; set; }

        public String lecturesLstByConcadinating { get;set; }

        public String GroupOrsubgroupForDisplay { get; set; }

      

        public override string ToString()
        {
            return SessionId.ToString() + "\n " +lecturesLstByConcadinating + "\n " + subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + " \n " + tagDSA.tags + "\n " + GroupOrsubgroupForDisplay + "\n" + StdntCount + "(" + durationinHours + ")"; ;
        }

    }
}
