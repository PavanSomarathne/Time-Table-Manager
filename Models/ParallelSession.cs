using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class ParallelSession
    {

        public int Id { get; set; }
        //public IList<ParallelSessionList> ParallelSessionLists { get; set; }

        public ICollection<Session> SessionsAS { get; set; }
    }
}
