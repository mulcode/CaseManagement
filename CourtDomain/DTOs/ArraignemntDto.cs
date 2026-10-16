using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDomain.DTOs
{
    public class ArraignemntDto
    {
        public DateTime PetitionDate { get; set; }
        public int ComplaintId { get; set; }
        public string DefendantName { get; set; } = string.Empty;
        public int ComplaintType { get; set; }
        public string? ApproveBy { get; set; }
        public DateTime? CourtDate { get; set; }
        public TimeSpan? CourtTime { get; set; }
        public bool FinishAndLock { get; set; }
    }
}
