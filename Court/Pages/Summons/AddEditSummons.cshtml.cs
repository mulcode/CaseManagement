using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Summons
{
    [Authorize(Policy = "AuthorizedUsers")]
    public class AddEditSummonsModel : PageModel
    {
        private readonly IUser s_user;
        private readonly IComplaint s_complaint;
        private readonly IDefendant s_defendant;
        private readonly IPetition s_petition;
        private readonly ISummon s_summon;
        private readonly IReturnOfService s_returnOfService;

        public AddEditSummonsModel(
            IUser user,
            ISummon summon,
            IComplaint complaint,
            IDefendant defendant,
            IPetition petition,
            IReturnOfService returnOfService)
        {
            s_user = user;
            s_summon = summon;
            s_complaint = complaint;
            s_defendant = defendant;
            s_petition = petition;
            s_returnOfService = returnOfService;
        }

        [BindProperty]
        public Summon SummonInfo { get; set; }

        public IList<User> Magistrates { get; set; }
        public IList<User> Attorneys { get; set; }

        public IList<ReturnOfService> ReturnOfServices { get; set; } = new List<ReturnOfService>();

        [BindProperty]
        public int ComplaintId { get; set; }

        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int complaintId, int summonId)
        {
            ComplaintId = complaintId;

            Magistrates = s_user.GetUsersByRole(2);
            Attorneys = s_user.GetUsersByRole(4);

            var petition = s_petition.GetPetitionByComplaintId(complaintId);

            if (summonId > 0)
            {
                SummonInfo = s_summon.GetSummon(summonId);
                LoadReturnOfServices(summonId);
            }
            else
            {
                SummonInfo = new Summon { PetitionId = petition.Id };
            }

            LoadTabs();
            return Page();
        }

        public IActionResult OnPost()
        {
            var petition = s_petition.GetPetitionByComplaintId(ComplaintId);
            SummonInfo.PetitionId = petition.Id;

            // A completed summons is a closed legal record and must not be edited.
            if (SummonInfo.Id > 0)
            {
                var existingSummon = s_summon.GetSummon(SummonInfo.Id);

                if (existingSummon != null && existingSummon.IsCompleted)
                {
                    TempData["ErrorMessage"] = "This summons is completed and can no longer be edited.";
                    return RedirectToPage(
                        "/Summons/SummonList",
                        new { complaintId = ComplaintId });
                }
            }

            if (!ModelState.IsValid)
            {
                Magistrates = s_user.GetUsersByRole(2);
                Attorneys = s_user.GetUsersByRole(4);

                if (SummonInfo.Id > 0)
                {
                    LoadReturnOfServices(SummonInfo.Id);
                }

                LoadTabs();
                return Page();
            }

            if (SummonInfo.Id == 0)
            {
                s_summon.AddSummon(SummonInfo);
                TempData["SuccessMessage"] = "Summon Saved Successfully";
            }
            else
            {
                s_summon.UpdateSummon(SummonInfo);
                TempData["SuccessMessage"] = "Summon Updated Successfully";
            }

            return RedirectToPage(
                "/Summons/SummonList",
                new { complaintId = ComplaintId });
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

        private void LoadReturnOfServices(int summonId)
        {
            ReturnOfServices = s_returnOfService.GetReturnOfServicesBySummonId(summonId);
        }
    }
}