using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class Student
    {
        public int Id { get; set; }
        public String accYrSem { get; set; }
        public String programme { get; set; }
        public int groupNo { get; set; }
        public String groupId { get; set; }
        public int subGroupNo { get; set; }
        public String subGroupId { get; set; }


        public ICollection<Session> sessionDSA { get; set; }
    }
}
