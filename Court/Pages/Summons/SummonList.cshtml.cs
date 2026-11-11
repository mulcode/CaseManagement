using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Summons
{
    [Authorize(Policy = "AuthorizedUsers")]
    public class SummonListModel : PageModel
    {
        private readonly IComplaint s_complaint;
        private readonly IDefendant s_defendant;
        private readonly IPetition s_petition;
        private readonly ISummon s_summon;
        private readonly IUser _user;
        

        public SummonListModel(IComplaint complaint, IDefendant defendant, IPetition petition, ISummon summon,IUser user)
        {
            s_complaint = complaint;
            s_defendant = defendant;
            s_petition = petition;
            s_summon = summon;
            _user = user;
        }

        public int ComplaintId { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }
        public IList<Summon> Summons { get; set; }
        public IList<User> Users { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            ComplaintId = complaintId;

            var petition = s_petition.GetPetitionByComplaintId(complaintId);

            Summons = s_summon.GetSummonsByPetitionId(petition.Id);
            Users = _user.GetUsersByRole(3).ToList();
            LoadTabs();
            return Page();
        }

        private void LoadTabs()
        {
            var complaint = s_complaint.GetComplaint(ComplaintId);
            var defendant = s_defendant.GetDefendant(complaint.DefendantId);
            var petition = s_petition.GetPetitionByComplaintId(ComplaintId);

            ComplaintTabs = new ComplaintTabsViewModels
            {
                ComplaintId = complaint.Id,
                DefendantId = complaint.DefendantId,
                DefendantName = $"{defendant.FirstName} {defendant.LastName}".Trim(),
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
