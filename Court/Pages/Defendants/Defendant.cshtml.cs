using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Defendants
{
    public class DefendantModel : PageModel
    {
        private readonly IDefendant m_defendant;
        private readonly ILogger<DefendantModel> _logger;

        public DefendantModel(
            IDefendant defendant,
            ILogger<DefendantModel> logger)
        {
            m_defendant = defendant;
            _logger = logger;
        }

        [BindProperty]
        public Defendant DefendantInfo { get; set; } = new Defendant();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public IActionResult OnGet(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var defendant = m_defendant.GetDefendant(id.Value);

                    if (defendant == null)
                    {
                        return NotFound();
                    }

                    DefendantInfo = defendant;
                }
                else
                {
                    DefendantInfo = new Defendant();

                    DefendantInfo.Date = DateTime.Now;
                    DefendantInfo.DateOfBirth = DateTime.Now;
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading defendant. DefendantId: {DefendantId}",
                    id);

                TempData["ErrorMessage"] =
                    "An error occurred while loading defendant information.";

                return RedirectToPage("/Defendants/DefendantList");
            }
        }

        public IActionResult OnPost()
        {
            try
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

                    TempData["SuccessMessage"] =
                        "Defendant Created Successfully";
                }
                else
                {
                    m_defendant.UpdateDefendant(DefendantInfo);

                    TempData["SuccessMessage"] =
                        "Defendant Updated Successfully";
                }

                return RedirectToPage("/Defendants/DefendantList");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving or updating defendant. DefendantId: {DefendantId}",
                    DefendantInfo.Id);

                TempData["ErrorMessage"] =
                    "An error occurred while saving defendant information.";

                return Page();
            }
        }
    }
}