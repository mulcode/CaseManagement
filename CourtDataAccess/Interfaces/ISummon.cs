using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface ISummon
    {
        public void AddSummon(Summon summon);

        Summon GetSummon(int summonId);

        void UpdateSummon(Summon summon);

        IList<Summon> GetSummonsByPetitionId(int petitionId);
    }
}
