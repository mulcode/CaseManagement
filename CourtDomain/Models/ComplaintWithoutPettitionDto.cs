using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDomain.Models
{
    public class ComplaintWithoutPetitionDto
    {
        public string DefendantName { get; set; } 
        public int ComplaintType { get; set; }
        public int ComplaintId { get; set; }
    }
}
