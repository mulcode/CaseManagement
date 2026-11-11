using CourtDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDomain.Models
{
   public class ReturnOfService
    {
        public int Id { get; set; }

        public int SummonId { get; set; }

        public virtual Summon Summon { get; set; }

        public ReturnType ReturnType { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        public string OfficerName { get; set; }

        public string OfficerSignature { get; set; }

        // Why the summons was not served (attempted / dismissed / death of party).
        public string Reason { get; set; }
    }
}
