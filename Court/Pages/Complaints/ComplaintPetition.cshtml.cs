using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Complaints
{
    public class ComplaintPetitionModel : PageModel
    {
        private readonly IComplaint c_petiton;
        private readonly IUser _user;
        public ComplaintPetitionModel(IComplaint petiton, IUser user)
        {
            c_petiton = petiton;
            _user = user; 
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; }

        public IEnumerable<User> Users { get; set; }

        public bool IsPetitionSaved { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            ComplaintInfo = c_petiton.GetComplaint(complaintId);

            if(ComplaintInfo == null)
            {
                return NotFound();
            }

            if (ComplaintInfo.PetitionDate == default)
            {
                ComplaintInfo.PetitionDate = DateTime.Now;
                IsPetitionSaved = false;
            }
            else
            {
                IsPetitionSaved = true;
            }

            Users = _user.GetUsersByRole(2).ToList();
            
            return Page();
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                Users = _user.GetUsersByRole(2).ToList();
                return Page();
            }

            c_petiton.UpdateComplaint(ComplaintInfo);
            return RedirectToPage("/Complaints/ComplaintPetition", new
            {
                complaintId = ComplaintInfo.Id
            });
        }
    }
}
