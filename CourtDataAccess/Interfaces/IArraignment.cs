using CourtDomain.DTOs;
using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface IArraignment
    {
        void AddArraignment(ArraignmentHearing arraignment);
        IList<ArraignmentHearing> GetArraignmentsByPetitionId(int petitionId);
        void UpdateArraignment(ArraignmentHearing arraignment);
        ArraignmentHearing? GetArraignment(int id);

        IList<ArraignemntDto> GetCourteCases(int pageNumber, int pageSize);

        int ArraignmentCount();
    }
}
