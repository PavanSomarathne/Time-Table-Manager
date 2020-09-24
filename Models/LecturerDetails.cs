using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TimeTableManager.Models
{
    public class LecturerDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LecName { get; set; }
        public string EmpId { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string Center { get; set; }
        public Building BuildinDSA { get; set; }

        public int EmpLevel { get; set; }

        public string Rank { get; set; }

        public virtual ICollection<RoomLecturer> RoomLecturers { get; set; }

        public virtual ICollection<SessionLecturer> SessionLecturers { get; set; }


    }
}
