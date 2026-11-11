using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class SummonManager : ISummon
    {
        private readonly AppDbContext _contex;
        public SummonManager(AppDbContext contex)
        {
            _contex = contex;
        }

        public void AddSummon(Summon summon)
        {
            _contex.Summons.Add(summon);
            _contex.SaveChanges();
        }

        public Summon GetSummon(int summonId)
        {
            return _contex.Summons
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == summonId);
        }

        public IList<Summon> GetSummonsByPetitionId(int petitionId)
        {
            return _contex.Summons
                .AsNoTracking()
                .Where(s => s.PetitionId == petitionId)
                .ToList();
        }

        public void UpdateSummon(Summon summon)
        {
            _contex.Summons.Update(summon);
            _contex.SaveChanges();
        }


    }
}
