using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTableManager.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Rid { get; set; }
        public Building BuildingAS { get; set; }
        public String Type { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<RoomLecturer> RoomLecturers { get; set; }
        public virtual ICollection<RoomSubject> RoomSubjects { get; set; }

    }
}
