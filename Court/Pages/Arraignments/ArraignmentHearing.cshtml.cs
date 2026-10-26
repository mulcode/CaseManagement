using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Arraignments
{
    public class ArraignmentHearingModel : PageModel
    {
        private readonly IArraignment a_manager;
        private readonly IUser _user;
        private readonly IComplaint a_complaint;
        private readonly IDefendant d_defendant;
        private readonly IPetition a_petition;

        public ArraignmentHearingModel(
            IArraignment manager,
            IUser user,
            IComplaint complaint,
            IDefendant defendant,
            IPetition petition)
        {
            a_manager = manager;
            _user = user;
            a_complaint = complaint;
            d_defendant = defendant;
            a_petition = petition;
           
        }

        [BindProperty]
        public ArraignmentHearing ArraignmentInfo { get; set; }

        public IList<User> Users { get; set; } 
        public IList<User> Judges { get; set; }
        public IList<User> Attorneys { get; set; }

        [BindProperty]
        public int ComplaintId { get; set; }

 

        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int complaintId, int arraignmentId)
        {
            ComplaintId = complaintId;

            Judges = _user.GetUsersByRole(3).ToList();
            Attorneys = _user.GetUsersByRole(4).ToList();

            var petition = a_petition.GetPetitionByComplaintId(complaintId);
           

            if (arraignmentId > 0)
            {
                ArraignmentInfo = a_manager.GetArraignment(arraignmentId);
                
            }
            else
            {
                ArraignmentInfo = new ArraignmentHearing { PetitionId = petition.Id };
            }

            LoadTabs();
            return Page();
        }

        public IActionResult OnPost()
        {

            var petition = a_petition.GetPetitionByComplaintId(ComplaintId);
            ArraignmentInfo.PetitionId = petition.Id;

            if (!ModelState.IsValid)
            {
                Judges = _user.GetUsersByRole(3).ToList();
                Attorneys = _user.GetUsersByRole(4).ToList();

                LoadTabs();

                return Page();
            }

            if (ArraignmentInfo.Id == 0)
            {
                a_manager.AddArraignment(ArraignmentInfo);

                TempData["SuccessMessage"] = "Arraignment Saved Successfully";
            }
            else
            {
                a_manager.UpdateArraignment(ArraignmentInfo);

                TempData["SuccessMessage"] = "Arraignment Updated Successfully";
            }

            return RedirectToPage(
                "/Arraignments/ArraignmentHearings",
                new
                {
                    complaintId = ComplaintId
                });
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
