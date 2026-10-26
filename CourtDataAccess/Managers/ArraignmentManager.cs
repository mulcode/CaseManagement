using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.DTOs;
using CourtDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class ArraignmentManager : IArraignment
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PetitionManager> _logger;
        public ArraignmentManager(AppDbContext context, ILogger<PetitionManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddArraignment(ArraignmentHearing arraignment)
        {
            _context.Arraignments.Add(arraignment);
            _context.SaveChanges();
        }
        public IList<ArraignmentHearing> GetArraignmentsByPetitionId(int petitionId)
        {
            return _context.Arraignments
                .Where(a => a.PetitionId == petitionId)
                .ToList();
        }

        public ArraignmentHearing? GetArraignment(int id)
        {
            return _context.Arraignments
                .FirstOrDefault(a => a.Id == id);
        }

        public void UpdateArraignment(ArraignmentHearing arraignment)
        {
            _context.Arraignments.Update(arraignment);
            _context.SaveChanges();
        }

        public IList<ArraignemntDto> GetCourteCases(int pageNumber, int pageSize)
        {
            try
            {
                var result = _context.Database
                    .SqlQuery<ArraignemntDto>(
                        $"EXEC dbo.GetUpcomingCourtCases")
                    .ToList();

                return result
                    .OrderBy(d => d.ComplaintId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting upcoming court cases.");
                throw;
            }
        }

        public int ArraignmentCount()
        {
            try
            {
                var result = _context.Database
                    .SqlQuery<ArraignemntDto>(
                        $"EXEC dbo.GetUpcomingCourtCases")
                    .ToList();

                return result.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting upcoming court cases.");
                throw;
            }
        }
    }
}
