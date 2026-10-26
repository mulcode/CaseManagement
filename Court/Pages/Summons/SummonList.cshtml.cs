using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Summons
{
    public class SummonListModel : PageModel
    {
        private readonly IComplaint s_complaint;
        private readonly IDefendant s_defendant;
        private readonly IPetition s_petition;

        public SummonListModel(IComplaint complaint, IDefendant defendant, IPetition petition)
        {
            s_complaint = complaint;
            s_defendant = defendant;
            s_petition = petition;
        }

        public int ComplaintId { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet()
        {
            //ComplaintId = complaintId;
            
            return Page();
        }

        //private void LoadTabs()
        //{
        //    var complaint = s_complaint.GetComplaint(ComplaintId);
        //    var defendant = s_defendant.GetDefendant(complaint.DefendantId);
        //    var petition = s_petition.GetPetitionByComplaintId(ComplaintId);

        //    ComplaintTabs = new ComplaintTabsViewModels
        //    {
        //        ComplaintId = complaint.Id,
        //        DefendantId = complaint.DefendantId,
        //        DefendantName = $"{complaint.Defendant?.FirstName} {complaint.Defendant?.LastName}".Trim(),
        //        EnrolledNumber = defendant.EnrolledNumber,
        //        Address = defendant.Address,
        //        Gender = defendant.Gender,
        //        IsDecisionSaved = complaint.MagistrateDecision.HasValue,
        //        PetitionId = petition.Id,
        //        IsPetitionFinished = petition.FinishAndLock
        //    };
        //}
    }
}
