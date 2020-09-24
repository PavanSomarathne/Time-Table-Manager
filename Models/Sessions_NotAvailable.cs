using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class Sessions_NotAvailable
    {
        //Sessions
        public int Id { get; set; }
        public String notAvailableSession { get; set; }
        public String notAvailableSessionDate { get; set; }
        public String notAvailableSessionStAt { get; set; }
        public String notAvailableSessionEndAt { get; set; }
    }
}
