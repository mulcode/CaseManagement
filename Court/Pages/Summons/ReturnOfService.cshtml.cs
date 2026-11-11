using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Enums;
using CourtDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Summons
{
    [Authorize(Policy = "AuthorizedUsers")]
    public class ReturnOfServiceModel : PageModel
    {
        private readonly IComplaint r_complaint;
        private readonly IDefendant r_defendant;
        private readonly IPetition r_petition;
        private readonly ISummon r_summon;
        private readonly IReturnOfService r_returnOfService;

        public ReturnOfServiceModel(IComplaint complaint, IDefendant defendant, IPetition petition, ISummon summon, IReturnOfService returnOfService)
        {
            r_complaint = complaint;
            r_defendant = defendant;
            r_petition = petition;
            r_summon = summon;
            r_returnOfService = returnOfService;
        }

        [BindProperty]
        public ReturnOfService ReturnInfo { get; set; }

        [BindProperty]
        public int ComplaintId { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int complaintId, int summonId)
        {
            if (summonId <= 0)
            {
                TempData["ErrorMessage"] = "Save the summons first, then record its return of service.";
                return RedirectToPage("/Summons/SummonList", new { complaintId });
            }

            var summon = r_summon.GetSummon(summonId);

            if (summon == null)
            {
                TempData["ErrorMessage"] = "The selected summons could not be found.";
                return RedirectToPage("/Summons/SummonList", new { complaintId });
            }

            if (summon.IsCompleted
                && r_returnOfService.GetReturnOfServicesBySummonId(summonId).Count > 0)
            {
                TempData["ErrorMessage"] = "This summons is already completed, so no further return of service can be added.";
                return RedirectToPage("/Summons/SummonList", new { complaintId });
            }

            ComplaintId = complaintId;

            ReturnInfo = new ReturnOfService
            {
                SummonId = summonId,
                Date = DateTime.Today
            };

            LoadTabs();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ReturnInfo == null)
            {
                TempData["ErrorMessage"] = "Return of service details were not submitted.";
                return RedirectToPage("/Summons/SummonList", new { complaintId = ComplaintId });
            }

            // ReturnType is a non-nullable enum, so [Required] cannot catch "nothing selected".
            // The value 0 is not a valid member and must be rejected.
            var returnTypeIsValid = Enum.IsDefined(typeof(ReturnType), ReturnInfo.ReturnType);

            if (!returnTypeIsValid)
            {
                ModelState.AddModelError("ReturnInfo.ReturnType", "Please select a return type.");
            }

            if (!ReturnInfo.Date.HasValue)
            {
                ModelState.AddModelError("ReturnInfo.Date", "Date of service is required.");
            }
            else if (ReturnInfo.Date.Value.Date > DateTime.Today)
            {
                ModelState.AddModelError("ReturnInfo.Date", "Date of service cannot be in the future.");
            }

            if (!ReturnInfo.Time.HasValue)
            {
                ModelState.AddModelError("ReturnInfo.Time", "Time of service is required.");
            }

            if (string.IsNullOrWhiteSpace(ReturnInfo.OfficerName))
            {
                ModelState.AddModelError("ReturnInfo.OfficerName", "Officer name is required.");
            }

            if (string.IsNullOrWhiteSpace(ReturnInfo.OfficerSignature))
            {
                ModelState.AddModelError("ReturnInfo.OfficerSignature", "Officer signature is required.");
            }

            // Every outcome other than a successful service must explain itself.
            if (returnTypeIsValid
                && ReturnInfo.ReturnType != ReturnType.Served
                && string.IsNullOrWhiteSpace(ReturnInfo.Reason))
            {
                ModelState.AddModelError(
                    "ReturnInfo.Reason",
                    "Please give the reason the summons was not served.");
            }

            var summon = ReturnInfo.SummonId > 0
                ? r_summon.GetSummon(ReturnInfo.SummonId)
                : null;

            if (summon == null)
            {
                TempData["ErrorMessage"] = "The selected summons could not be found.";
                return RedirectToPage("/Summons/SummonList", new { complaintId = ComplaintId });
            }

            if (summon.IsCompleted
                && r_returnOfService.GetReturnOfServicesBySummonId(summon.Id).Count > 0)
            {
                TempData["ErrorMessage"] = "This summons is already completed, so no further return of service can be added.";
                return RedirectToPage("/Summons/SummonList", new { complaintId = ComplaintId });
            }

            if (!ModelState.IsValid)
            {
                LoadTabs();
                return Page();
            }

            r_returnOfService.AddReturnOfService(ReturnInfo);

            // Completion is a manual decision on the summons form. A return of
            // service records the outcome only; it never changes IsCompleted.
            TempData["SuccessMessage"] = "Return of Service saved successfully. Mark the summons as completed when service is finished.";

            return RedirectToPage(
                "/Summons/SummonList",
                new { complaintId = ComplaintId });
        }

        private void LoadTabs()
        {
            var complaint = r_complaint.GetComplaint(ComplaintId);
            var defendant = r_defendant.GetDefendant(complaint.DefendantId);
            var petition = r_petition.GetPetitionByComplaintId(ComplaintId);

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
