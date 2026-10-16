using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class ArraignmentManager : IArraignment
    {
        private readonly AppDbContext _context;
        public ArraignmentManager(AppDbContext context)
        {
            _context = context;
        }

        public void AddArraignment(ArraignmentHearing arraignment)
        {
            _context.Arraignments.Add(arraignment);
            _context.SaveChanges();
        }
        public List<ArraignmentHearing> GetArraignmentsByPetitionId(int petitionId)
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
    }
}
