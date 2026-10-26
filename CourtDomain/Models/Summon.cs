using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourtDomain.Models
{
    public class Summon
    {
        public int Id { get; set; }
        public int PetitionId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        public TimeSpan? Time { get; set; }

        public bool IsAliasAndPluriesSummons { get; set; }

        
        public bool IsOrder { get; set; }                
        public bool IsFirstAppearance { get; set; }      
        public bool IsAdjudicatoryHearing { get; set; }  

       
        public bool MustAttendScheduledHearings { get; set; }
        public bool CourtJurisdiction { get; set; }

        public bool HasCourtAppointedLawyer { get; set; }
        public int? AttorneyId { get; set; }
        public virtual User Attorney { get; set; }
        public bool WillBeAdvisedOfAppointedLawyer { get; set; }

        [Required(ErrorMessage = "Judicial Name is required")]
        public int JudicialOfficialId { get; set; }
        public virtual User JudicialOfficial { get; set; }
        public string? JudicialName { get; set; }
        public string? JudicialESignature { get; set; }
        public DateTime? SignDate { get; set; }
        public TimeSpan? SignTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
