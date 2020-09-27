using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TimeTableManager.Models
{
    public class RoomNAT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Room room { get; set; }

        public String StartTime { get; set; }

        public String EndTime { get; set; }

        public String Day { get; set; }

        public override string ToString()
        {
            return Day + " " + StartTime +" To "+EndTime;
        }
    }
}
