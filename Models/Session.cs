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

        public ParallelSession par { get; set; }

        public String getSessionStu() 
        {
            if (Room != null)
            {

                return lecturesLstByConcadinating + "\n" + subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + "\n" + tagDSA.tags + "\n" + GroupOrsubgroupForDisplay + "\n" + Room.Rid;
            }else
             {

                return lecturesLstByConcadinating + "\n" + subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + "\n" + tagDSA.tags + "\n" + GroupOrsubgroupForDisplay + "\n" + "-";
            }
        }
        public String getSessionLec()
        {
            if (Room != null)
            {

                return lecturesLstByConcadinating + "\n" + subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + "\n" + tagDSA.tags + "\n" + GroupOrsubgroupForDisplay + "\n" + Room.Rid;
            }
            else
            {

                return subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + "\n" + tagDSA.tags + "\n" + GroupOrsubgroupForDisplay + "\n" + "-";
            }
        }
        public String getSessionRoom()
        {
            return lecturesLstByConcadinating + "\n" + subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + "\n" + tagDSA.tags + "\n" + GroupOrsubgroupForDisplay;

        }


        public override string ToString()
        {
            if (subjectDSA == null || tagDSA == null)
            {
                return SessionId.ToString() + "\n" + lecturesLstByConcadinating  + "\n" + GroupOrsubgroupForDisplay + "\n" + StdntCount + "(" + durationinHours + ")";
            }
            return SessionId.ToString() + "\n"+ lecturesLstByConcadinating + "\n" + subjectDSA.SubjectName + "(" + subjectDSA.SubjectCode + ")" + "\n" + tagDSA.tags + "\n" + GroupOrsubgroupForDisplay + "\n" + StdntCount + "(" + durationinHours + ")";
        }

        
    }
}
