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
        private readonly IPetition a_petition;
        private readonly IUser a_user;
        public ArraignmentHearingsModel(IArraignment arraignment, IComplaint complaint, IPetition petition, IUser user)
        {
            a_arraignment = arraignment;
            a_complaint = complaint;
            a_petition = petition;
            a_user = user;
        }

        public int ComplaintId { get; set; }
        public int PetitionId { get; set; }
        public bool IsDecisionSaved { get; set; }
        public bool IsPetitionFinished { get; set; }
        public List<ArraignmentHearing> Arraignments { get; set; } = new();
        public List<User> Users { get; set; } = new();

        public IActionResult OnGet(int complaintId)
        {
            ComplaintId = complaintId;

            var complaint = a_complaint.GetComplaint(complaintId);

            if (complaint == null)
                return NotFound();

            IsDecisionSaved = complaint.MagistrateDecision.HasValue;

            var petition = a_petition.GetPetitionByComplaintId(complaintId);

            if (petition == null)
                return NotFound();

            PetitionId = petition.Id;
            IsPetitionFinished = petition.FinishAndLock;

            Arraignments = a_arraignment.GetArraignmentsByPetitionId(PetitionId);

            Users = a_user.GetUsersByRole(3).ToList();

            return Page();
        }
    }
}
