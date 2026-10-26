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

        IList<PetitionDto> GetPetitions(int pageNumber, int pageSize);
        int GetPetitionCount();

        IList<ComplaintWithoutPetitionDto> GetComplaintsWithoutPetition(int pageNumber, int pageSize);
        int GetAwatingPetitionCount();

       

    }
}
