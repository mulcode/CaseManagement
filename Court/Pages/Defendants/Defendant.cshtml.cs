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
        public Defendant DefendantInfo { get; set; } 

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        [BindProperty]
        public bool IsEditMode { get; set; }
        public IActionResult OnGet(int defendantId, bool edit) 
        {
            try
            {
                if (defendantId > 0)
                {
                    DefendantInfo = m_defendant.GetDefendant(defendantId);
                }
                else
                {
                    DefendantInfo = new Defendant()
                    {
                        Date = DateTime.Now,
                        DateOfBirth = DateTime.Now
                    };
                   
                }
                IsEditMode = defendantId == 0 || edit;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading defendant. DefendantId: {DefendantId}",
                    defendantId);

                TempData["ErrorMessage"] =
                    "An error occurred while loading defendant information.";

                return RedirectToPage("/Defendants/DefendantList");
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                IsEditMode = true;

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

                    TempData["SuccessMessage"] = "Defendant Created Successfully";
                }
                else
                {
                    m_defendant.UpdateDefendant(DefendantInfo);

                    TempData["SuccessMessage"] = "Defendant Updated Successfully";
                }

                return RedirectToPage("/Defendants/DefendantList", new {defendantId = DefendantInfo.Id });
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