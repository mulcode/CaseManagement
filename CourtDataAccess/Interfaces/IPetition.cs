using CourtDomain.Models;
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
    }
}
