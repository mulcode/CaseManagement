using CourtDataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourtDomain.Models;

namespace Court.Pages.Defendants
{
    [Authorize(Policy = "AuthorizedUsers")]
    public class DefendantListModel : PageModel
    {
        private readonly IDefendant m_defendant;
        public DefendantListModel(IDefendant defendant)
        {
            m_defendant = defendant;
        }

        public List<Defendant> Defendants { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;
        public int TotalPage { get; set; }

        public void OnGet(int pageNumber = 1)
        {
            // claimed user
            var userName = User.Identity?.Name;
            var userType = User.FindFirst("UserType")?.Value;


            //pagination
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            PageNumber = pageNumber;

            int totalRecords = m_defendant.GetTotalDefendantCount();

            TotalPage = (int)Math.Ceiling(
                (double)totalRecords / PageSize
            );

            Defendants = m_defendant
                .GetDefendants(PageNumber, PageSize)
                .ToList();
        }


        public IActionResult OnGetImage(int id)
        {
            var defendant = m_defendant.GetDefendant(id);
            if (defendant == null || defendant.Picture == null)
            {
                return NotFound();
            }

            return File(defendant.Picture, "image/jpeg");
        }

        public IActionResult OnPostDelete(int id)
        {
            m_defendant.DeleteDefendant(id);

            return RedirectToPage("/Defendants/DefendantList");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync("MyCookie");

            return RedirectToPage("/Login/Login");
        }
    }
}
