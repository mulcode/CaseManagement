using CourtDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourtDomain.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        public int DefendantId { get; set; }

        public virtual Defendant Defendant { get; set; }

        [Required(ErrorMessage = "Date is Required")]
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Complaint Type is Required")]
        public ComplaintType ComplaintType { get; set; }

        public DateTime OffesnseDate { get; set; }

        public string SummaryOfComplaint { get; set; }

        public int? ComplainantId { get; set; }
        public virtual User Complainant { get; set; }


        public string ComplainantSignature { get; set; }
        public string ComplainantPhone { get; set; }
        public string BadgeNumber { get; set; }
        public bool ReadyForApproval { get; set; }

        public int? MagistrateId { get; set; }
        public virtual User Magistrate { get; set; }
        public string MagistrateSignature { get; set; }
        public Decision? MagistrateDecision { get; set; }
        public DateTime MagistrateDecisionDate { get; set; }
        public TimeSpan MagistrateDecisionTime { get; set; }
        public DateTime ReceivedDateByMagistrate { get; set; }
     
    }
}
