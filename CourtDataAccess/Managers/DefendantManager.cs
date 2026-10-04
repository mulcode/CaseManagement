using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class DefendantManager : IDefendant
    {
        private readonly AppDbContext _context;
        public DefendantManager(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Defendant> GetAllDefendant()
        {
            return _context.Defendants.ToList();
        }
        public Defendant GetDefendant(int id)
        {
            return _context.Defendants.Find(id);
        }

        public IEnumerable<Defendant> GetDefendants(int pageNumber, int pageSize)
        {
            return _context.Defendants
                .OrderBy(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalDefendantCount()
        {
            return _context.Defendants.Count();
        }

        public void AddDefendant(Defendant defendant)
        {
            _context.Defendants.Add(defendant);
            _context.SaveChanges();
        }
        public void UpdateDefendant(Defendant defendant)
        {
            _context.Defendants.Update(defendant);
            _context.SaveChanges();
        }
         public void DeleteDefendant(int id)
        {
            var defendant = _context.Defendants.Find(id);
            if(defendant != null)
            {
                _context.Defendants.Remove(defendant);
                _context.SaveChanges();
            }
        }
    }
}
