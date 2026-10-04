using CourtDataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourtDomain.Models;


namespace Court.Pages.Complaints
{
    public class ComplaintListModel : PageModel
    {
        private readonly IComplaint c_complaint;

        public ComplaintListModel(IComplaint complaint)
        {
            c_complaint = complaint;
        }


        [BindProperty]
        public List<Complaint> Complaints { get; set; }

        public int DefendantId { get; set; }
        public void OnGet(int id)
        {
            DefendantId = id;

            Complaints = c_complaint.GetComplaintsByDefendant(id).ToList();
        }

        
        public IActionResult OnPostDelete(int id)
        {
            var defendantId = c_complaint.DeleteComplaint(id);

            if (defendantId != 0)
            {
                return RedirectToPage(
                    "/Complaints/ComplaintList",
                    new { id = defendantId }
                );
            }

            return RedirectToPage("/Complaints/ComplaintList");
        }

    }
}
