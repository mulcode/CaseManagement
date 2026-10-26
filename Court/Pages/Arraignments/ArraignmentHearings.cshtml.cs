using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Arraignments
{
    public class ArraignmentHearingsModel : PageModel
    {
        private readonly IArraignment a_arraignment;
        private readonly IComplaint a_complaint;
        private readonly IDefendant d_defendant;
        private readonly IPetition a_petition;
        private readonly IUser a_user;
        public ArraignmentHearingsModel(IArraignment arraignment, IComplaint complaint, IPetition petition, IUser user,IDefendant defendant)
        {
            a_arraignment = arraignment;
            a_complaint = complaint;
            a_petition = petition;
            a_user = user;
            d_defendant = defendant;
        }

        public IList<ArraignmentHearing> Arraignments { get; set; }
        public IList<User> Users { get; set; }
        public int ComplaintId { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            ComplaintId = complaintId;

            var petition = a_petition.GetPetitionByComplaintId(complaintId);

            Arraignments = a_arraignment.GetArraignmentsByPetitionId(petition.Id);

            Users = a_user.GetUsersByRole(3).ToList();
            LoadTabs();
            return Page();
        }

        private void LoadTabs()
        {
            var complaint = a_complaint.GetComplaint(ComplaintId);
            var defendant = d_defendant.GetDefendant(complaint.DefendantId);
            var petition = a_petition.GetPetitionByComplaintId(ComplaintId);

            ComplaintTabs = new ComplaintTabsViewModels
            {
                ComplaintId = complaint.Id,
                DefendantId = complaint.DefendantId,
                DefendantName = $"{complaint.Defendant?.FirstName} {complaint.Defendant?.LastName}".Trim(),
                EnrolledNumber = defendant.EnrolledNumber,
                Address = defendant.Address,
                Gender = defendant.Gender,
                IsDecisionSaved = complaint.MagistrateDecision.HasValue,
                PetitionId = petition.Id,
                IsPetitionFinished = petition.FinishAndLock
            };
        }
    }
}
