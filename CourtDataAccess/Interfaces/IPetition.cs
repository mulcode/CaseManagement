using CourtDomain.DTOs;
using CourtDomain.Models;
using CourtDomain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace CourtDataAccess.Interfaces
{
    public interface IPetition
    {
        void AddPetition(Petition petition);
        Petition GetPetition(int id);
        void UpdatePetition(Petition petition);
        Petition GetPetitionByComplaintId(int complaintId);

        List<PetitionDto> GetPetitions(int pageNumber, int pageSize);
        int GetPetitionCount();

        List<ComplaintWithoutPetitionDto> GetComplaintsWithoutPetition();

        IEnumerable<ArraignemntDto> GetCourteCases();

    }
}
