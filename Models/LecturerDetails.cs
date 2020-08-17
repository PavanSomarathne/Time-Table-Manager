using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
   public class LecturerDetails
    {
        public int Id { get; set; }
        public string LecName { get; set; }
        public string EmpId { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string Center { get; set; }
        public string Building { get; set; }
        public int  EmpLevel { get; set; }

        public string Rank { get; set; }

    }
}
