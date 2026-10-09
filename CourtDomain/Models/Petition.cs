using CourtDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourtDomain.Models
{
    public class Petition
    { 
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime PetitionDate { get; set; }

        public TimeSpan PetitionTime { get; set; }

        [Required(ErrorMessage = "Decision is required")]
        public Decision MagistrateDecision { get; set; }

        [Required(ErrorMessage = "Decision Date is required")]
        public DateTime MagistrateDecisionDate { get; set; }
        public TimeSpan MagistrateDecisionTime { get; set; }

        [Required(ErrorMessage = "Magistrate Name is required")]
        public int? MagistrateId { get; set; }
        public virtual User Magistrate { get; set; }

        [Required(ErrorMessage = "Magistrate Isgnature is required")]
        public string MagistrateSignature { get; set; }

        public bool Disobedient { get; set; }
        public bool FoundInUnlawfulPlace { get; set; }
        public bool RanAway { get; set; }
        public bool AbsentFromSchool { get; set; }
        public bool FinishAndLock { get; set; }
        public string SupportingFacts { get; set; }
        public bool IsPetitionSaved { get; set; }
    }
}
