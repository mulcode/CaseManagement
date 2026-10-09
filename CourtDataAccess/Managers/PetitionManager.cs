using CourtDataAccess.Interfaces;
using CourtDataAccess.DbModel;
using CourtDomain.Models;
using Microsoft.Extensions.Logging;

namespace CourtDataAccess.Managers
{
    public class PetitionManager : IPetition
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PetitionManager> _logger;

        public PetitionManager(
            AppDbContext context,
            ILogger<PetitionManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Petition GetPetitionByComplaintId(int complaintId)
        {
            try
            {
                return _context.Petitions
                    .FirstOrDefault(p => p.ComplaintId == complaintId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting petition for ComplaintId {ComplaintId}",
                    complaintId);

                throw;
            }
        }

        public Petition GetPetition(int id)
        {
            try
            {
                return _context.Petitions
                    .FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting PetitionId {PetitionId}",
                    id);

                throw;
            }
        }

        public void AddPetition(Petition petition)
        {
            try
            {
                _context.Petitions.Add(petition);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding petition for ComplaintId {ComplaintId}",
                    petition.ComplaintId);

                throw;
            }
        }

        public void UpdatePetition(Petition petition)
        {
            try
            {
                _context.Petitions.Update(petition);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating PetitionId {PetitionId}",
                    petition.Id);

                throw;
            }
        }
    }
}

