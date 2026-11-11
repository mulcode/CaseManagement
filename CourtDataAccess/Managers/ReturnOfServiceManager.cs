using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class ReturnOfServiceManager : IReturnOfService
    {
        private readonly AppDbContext _context;

        public ReturnOfServiceManager(AppDbContext context)
        {
            _context = context;
        }

        public void AddReturnOfService(ReturnOfService returnInfo)
        {
            _context.ReturnOfServices.Add(returnInfo);
            _context.SaveChanges();
        }

        public IList<ReturnOfService> GetReturnOfServicesBySummonId(int summonId)
        {
            return _context.ReturnOfServices
                .Where(r => r.SummonId == summonId)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.Id)
                .ToList();
        }
    }
}
