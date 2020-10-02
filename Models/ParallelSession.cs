using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class ParallelSession
    {

        public int Id { get; set; }

        public Session first { get; set; }
        public Session second { get; set; }
    }
}
