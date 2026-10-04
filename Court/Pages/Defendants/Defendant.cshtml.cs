using CourtDataAccess.Interfaces;
using CourtDataAccess.Managers;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourtDomain.Models;

namespace Court.Pages.Defendants
{
    public class DefendantModel : PageModel
    {
        private readonly IDefendant m_defendant;
        public DefendantModel(IDefendant  defendant)
        {
            m_defendant = defendant;
        }

        [BindProperty]
        public Defendant DefendantInfo { get; set; } = new Defendant();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public void OnGet(int? id)
        {
            DefendantInfo.Date = DateTime.Now;
            DefendantInfo.DateOfBirth = DateTime.Now;

            if (id.HasValue)
            {
                DefendantInfo = m_defendant.GetDefendant(id.Value);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();

                ImageFile.CopyTo(memoryStream);

                DefendantInfo.Picture = memoryStream.ToArray();
            }

            if (DefendantInfo.Id == 0)
            {
                m_defendant.AddDefendant(DefendantInfo);
            }
            else
            {
                m_defendant.UpdateDefendant(DefendantInfo);
            }

            return RedirectToPage("/Defendants/DefendantList");
        }
    }
}
