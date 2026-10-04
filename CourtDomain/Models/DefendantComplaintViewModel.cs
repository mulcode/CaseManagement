using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDomain.Models
{
    public class DefendantComplaintViewModel
    {
        public int ComplaintId { get; set; }
        public DateTime Date { get; set; }

        public string DefendantName { get; set; }

        public string ComplaintType { get; set; }

        public string ComplainantName { get; set; }
    }
}
