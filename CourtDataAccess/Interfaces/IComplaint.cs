using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface IComplaint
    {
        IList<Complaint> GetComplaintsByDefendant(int defendantId);
        IList<DefendantComplaintDto> GetAwatingComplaintSubmision(int pageNumber, int pageSize);
        int GetAwatingComplaintCount();

        IList<DefendantComplaintDto> GetAwatingDecision(int pageNumber, int pageSize);
        int GetAwatingDecisionCount();
        Complaint GetComplaint(int id);
        void AddComplaint(Complaint complaint);
        void UpdateComplaint(Complaint complaint);
        int DeleteComplaint(int id);
    }
}


