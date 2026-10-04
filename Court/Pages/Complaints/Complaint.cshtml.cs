using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Court.Pages.Complaints
{
    public class ComplaintModel : PageModel
    {
        private readonly IComplaint c_complaint;
        public ComplaintModel(IComplaint complaint)
        {
            c_complaint = complaint;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; } = new Complaint();

     
        public IActionResult OnGet(int? id, int defendantId)
        {
            
            if (id.HasValue)
            {
                ComplaintInfo = c_complaint.GetComplaint(id.Value);
            }
            else
            {
                ComplaintInfo.DefendantId = defendantId;
                ComplaintInfo.OffesnseDate = DateTime.Now;
                ComplaintInfo.Date = DateTime.Now;
            }
            return Page();
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ComplaintInfo.Id == 0)
            {
                c_complaint.AddComplaint(ComplaintInfo);
            }
            else
            {
                c_complaint.UpdateComplaint(ComplaintInfo);
            }

            return RedirectToPage("/Complaints/ComplaintList", new { id = ComplaintInfo.DefendantId });
        }

        public IActionResult OnGetCheckDecision(int complaintId)
        {
            var complaint = c_complaint.GetComplaint(complaintId);

            if (complaint == null)
            {
                return new JsonResult(new
                {
                    readyForApproval = false
                });
            }

            return new JsonResult(new
            {
                readyForApproval = complaint.ReadyForApproval
            });
        }
    }
}
