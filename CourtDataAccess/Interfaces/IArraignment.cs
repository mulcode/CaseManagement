using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface IArraignment
    {
        void AddArraignment(ArraignmentHearing arraignment);
        List<ArraignmentHearing> GetArraignmentsByPetitionId(int petitionId);
        void UpdateArraignment(ArraignmentHearing arraignment);
        ArraignmentHearing? GetArraignment(int id);
    }
}
