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
        private readonly IPetition a_petition;
        public ArraignmentHearingModel(IArraignment manager, IUser user,IComplaint complaint,IPetition petition)
        {
            a_manager = manager;
            _user = user;
            a_complaint = complaint;
            a_petition = petition;
        }
        [BindProperty]
        public ArraignmentHearing ArraignmentInfo { get; set; } = new ArraignmentHearing();

        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<User> Judges { get; set; } = new List<User>();
        public IEnumerable<User> Attorneys { get; set; } = new List<User>();

        [BindProperty(SupportsGet = true)]
        public int ComplaintId { get; set; }
        public bool IsDecisionSaved { get; set; }
        public int PetitionId { get; set; }
        public bool FinishAndLock { get; set; }

        public IActionResult OnGet(int complaintId, int? id)
        {
            ComplaintId = complaintId;
            Judges = _user.GetUsersByRole(3).ToList();
            Attorneys = _user.GetUsersByRole(4).ToList();

            var complaint = a_complaint.GetComplaint(complaintId);
            

            IsDecisionSaved = complaint.MagistrateDecision.HasValue;

            var petition = a_petition.GetPetitionByComplaintId(complaintId);
            
            PetitionId = petition.Id;
            FinishAndLock = petition.FinishAndLock;
            ArraignmentInfo.PetitionId = petition.Id;

            if (id.HasValue)
            {
                var arraignment = a_manager.GetArraignment(id.Value);

                if (arraignment == null)
                    return NotFound();

                ArraignmentInfo = arraignment;
            }
            else
            {
                ArraignmentInfo.PetitionId = petition.Id;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var petition = a_petition.GetPetitionByComplaintId(ComplaintId);

            
            PetitionId = petition.Id;
            FinishAndLock = petition.FinishAndLock;

            ArraignmentInfo.PetitionId = petition.Id;

            if (!ModelState.IsValid)
            {
                Judges = _user.GetUsersByRole(3).ToList();
                Attorneys = _user.GetUsersByRole(4).ToList();

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
    }
}
