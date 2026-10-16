using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourtDataAccess.Managers
{
    public class ComplaintManager : IComplaint
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ComplaintManager> _logger;

        public ComplaintManager(
            AppDbContext context,
            ILogger<ComplaintManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Complaint> GetComplaintsByDefendant(int defendantId)
        {
            try
            {
                return _context.Complaints
                    .Where(c => c.DefendantId == defendantId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting complaints for DefendantId {DefendantId}",
                    defendantId);

                throw;
            }
        }

        public Complaint GetComplaint(int id)
        {
            try
            {
                return _context.Complaints
                    .FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting ComplaintId {ComplaintId}",
                    id);

                throw;
            }
        }

        public void AddComplaint(Complaint complaint)
        {
            try
            {
                _context.Complaints.Add(complaint);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding complaint for DefendantId {DefendantId}",
                    complaint.DefendantId);

                throw;
            }
        }

        public void UpdateComplaint(Complaint complaint)
        {
            try
            {
                // Existing tracked entity হলে SaveChanges() যথেষ্ট।
                // Detached entity হলে Update() প্রয়োজন হতে পারে।
                _context.Complaints.Update(complaint);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating ComplaintId {ComplaintId}",
                    complaint.Id);

                throw;
            }
        }

        public int DeleteComplaint(int id)
        {
            var complaint = _context.Complaints
                .FirstOrDefault(c => c.Id == id);

            if (complaint == null)
                return 0;

            int defendantId = complaint.DefendantId;

            _context.Complaints.Remove(complaint);

            _context.SaveChanges();

            return defendantId;
        }

        public IEnumerable<DefendantComplaintDto> GetAwatingComplaintSubmision()
        {
            try
            {
                return
                  (from c in _context.Complaints
                   join d in _context.Defendants
                       on c.DefendantId equals d.Id

                   where !c.ReadyForApproval

                   select new DefendantComplaintDto
                   {
                       ComplaintId = c.Id,
                       Date = c.Date,

                       DefendantName = d.FirstName + " " + d.LastName,

                       ComplaintType = c.ComplaintType.ToString()
                   })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting complaints awaiting submission.");

                throw;
            }
        }

        public IEnumerable<DefendantComplaintDto> GetAwatingDecision()
        {
            try
            {
                return
                  (from c in _context.Complaints
                   join d in _context.Defendants
                       on c.DefendantId equals d.Id

                   where c.MagistrateDecision  == null

                   select new DefendantComplaintDto
                   {
                       ComplaintId = c.Id,
                       Date = c.Date,

                       DefendantName = d.FirstName + " " + d.LastName,

                       ComplaintType = c.ComplaintType.ToString()
                   })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting complaints awaiting submission.");

                throw;
            }
        }
    }
}

