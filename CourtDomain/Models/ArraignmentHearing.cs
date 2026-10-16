using CourtDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourtDomain.Models
{
    public class ArraignmentHearing
    {
        public int Id { get; set; }
        public int PetitionId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime? ArraignmentDate { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeSpan? ArraignmentTime { get; set; }
        public bool WaivedArraignment { get; set; }
        public bool WasInformed { get; set; }
        public bool LegalCounsel { get; set; }
        public int? AttorneyId { get; set; }
        public virtual User Attorney { get; set; }

        public bool WaivedCounsel { get; set; }
        public bool FoundGuilty { get; set; }
        public bool ReceivedCopyOfCharge { get; set; }
        public bool InquiredOfDefendant { get; set; }
        public bool EnteredPlea { get; set; }
        public bool BailStatus { get; set; }
        public string? BailStatusDetails { get; set; }
        public bool ExaminedCriminalProcess { get; set; }
        public bool AdditionalCharges { get; set; }
        public string? ChargeDetails { get; set; }
        public bool CourtDate { get; set; }

        [Required(ErrorMessage = "Hearing Date is required")]
        public DateTime? NextHearingDate { get; set; }

        [Required(ErrorMessage = "Hearing time is required")]
        public TimeSpan? NextHearingTime { get; set; }

        [Required(ErrorMessage = "Judge Name is required")]
        public int? JudgeId { get; set; }
        public virtual User Judge { get; set; }

        [Required(ErrorMessage = "Judge Isgnature is required")]
        public string JudgeSignature { get; set; }

        [Required(ErrorMessage = "Sign Date is required")]
        public DateTime? SignDate { get; set; }

        [Required(ErrorMessage = "Sign Time is required")]
        public TimeSpan? SignTime { get; set; }

        public bool FinishAndLock { get; set; }
    }
}
