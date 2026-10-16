using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface IComplaint
    {
        IEnumerable<Complaint> GetComplaintsByDefendant(int defendantId);
        IEnumerable<DefendantComplaintDto> GetAwatingComplaintSubmision();
        IEnumerable<DefendantComplaintDto> GetAwatingDecision();
        Complaint GetComplaint(int id);
        void AddComplaint(Complaint complaint);
        void UpdateComplaint(Complaint complaint);
        int DeleteComplaint(int id);
    }
}
