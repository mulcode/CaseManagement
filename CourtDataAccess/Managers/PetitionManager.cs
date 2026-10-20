using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.DTOs;
using CourtDomain.Models;
using CourtDomain.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;


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


        public List<PetitionDto> GetPetitions(int pageNumber,int pageSize)
        {
            try
            {
                var result = _context.Database
                    .SqlQuery<PetitionDto>($"EXEC GetPetitionList")
                    .ToList();


                return result
                     .OrderBy(d => d.ComplaintId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting petitions");
                throw;
            }
        }

        public int GetPetitionCount()
        {
            try
            {
                return _context.Petitions.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting total petition count.");

                throw;
            }
        }

        public List<ComplaintWithoutPetitionDto> GetComplaintsWithoutPetition()
        {
            return _context.Database
                .SqlQuery<ComplaintWithoutPetitionDto>(
                    $"EXEC GetComplaintsWithoutPetition")
                .ToList();
        }

    
        public IEnumerable<ArraignemntDto> GetCourteCases()
        {
            try
            {
                var result = _context.Database
                    .SqlQuery<ArraignemntDto>(
                        $"EXEC dbo.GetUpcomingCourtCases")
                    .ToList();

                _logger.LogInformation(
                    "Upcoming court cases returned: {Count}",
                    result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting upcoming court cases");
                throw;
            }
        }

    }
}

