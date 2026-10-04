using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class ComplaintManager : IComplaint
    {
        private readonly AppDbContext _context;
        public ComplaintManager(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Complaint> GetComplaintsByDefendant(int defendantId)
        {
            return _context.Complaints
                .Where(c => c.DefendantId == defendantId)
                .ToList();
        }
        public Complaint GetComplaint(int id)
        {
            return _context.Complaints.Find(id);
        }
        public void AddComplaint(Complaint complaint)
        {
            _context.Complaints.Add(complaint);
            _context.SaveChanges();
        }
        public void UpdateComplaint(Complaint complaint)
        {
            _context.Complaints.Update(complaint);
            _context.SaveChanges();
        }
        public int DeleteComplaint(int id)
        {
            var complaint = _context.Complaints.Find(id);

            if (complaint != null)
            {
                int defendantId = complaint.DefendantId;
                _context.Complaints.Remove(complaint);
                _context.SaveChanges();

                return defendantId;
            }
            return 0;
        }

        public IEnumerable<DefendantComplaintViewModel> GetAwatingComplaintSubmision()
        {
            var awatingForSubmission = _context.Complaints
                .Where(c => c != null)
                .Select(c => new DefendantComplaintViewModel
                {
                    ComplaintId = c.Id,
                    Date = c.Date,
                    DefendantName = c.Defendant.FirstName + "" + c.Defendant.LastName,
                    ComplaintType = c.ComplaintType.ToString(),
                    ComplainantName = c.Complainant != null ? c.Complainant.UserName : ""
                }).ToList();

            return awatingForSubmission;
        }
    }
}
