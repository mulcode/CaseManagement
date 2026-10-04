using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IComplaint d_complaint;
        public DashboardModel(IComplaint complaint)
        {
            d_complaint = complaint;
        }

        public IEnumerable<DefendantComplaintViewModel> Defendants { get; set; }

        public void OnGet()
        {
            Defendants = d_complaint.GetAwatingComplaintSubmision();
        }
    }
}
