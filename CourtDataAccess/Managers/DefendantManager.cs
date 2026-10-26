using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourtDataAccess.Managers
{
    public class DefendantManager : IDefendant
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DefendantManager> _logger;

        public DefendantManager(
            AppDbContext context,
            ILogger<DefendantManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Defendant> GetAllDefendant()
        {
            try
            {
                return _context.Defendants.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all defendants.");
                throw;
            }
        }

        public Defendant GetDefendant(int id)
        {
            try
            {
                return _context.Defendants.Find(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting DefendantId {DefendantId}",
                    id);

                throw;
            }
        }

        public IList<Defendant> GetDefendants( int pageNumber,int pageSize)
        {
            try
            {
                return _context.Defendants
                    .OrderBy(d => d.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting defendants for page {PageNumber}",
                    pageNumber);

                throw;
            }
        }

        public int GetTotalDefendantCount()
        {
            try
            {
                return _context.Defendants.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting total defendant count.");

                throw;
            }
        }

        public void AddDefendant(Defendant defendant)
        {
            try
            {
                _context.Defendants.Add(defendant);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding defendant.");

                throw;
            }
        }

        public void UpdateDefendant(Defendant defendant)
        {
            try
            {
                _context.Defendants.Update(defendant);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating DefendantId {DefendantId}",
                    defendant.Id);

                throw;
            }
        }

        public void DeleteDefendant(int id)
        {
            try
            {
                var defendant = _context.Defendants.Find(id);

                if (defendant != null)
                {
                    _context.Defendants.Remove(defendant);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting DefendantId {DefendantId}",
                    id);

                throw;
            }
        }

        public bool HasComplaints(int defendantId)
        {
            try
            {
                return _context.Complaints
                    .Any(c => c.DefendantId == defendantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error checking complaints for DefendantId {DefendantId}",
                    defendantId);

                throw;
            }
        }
    }
}

