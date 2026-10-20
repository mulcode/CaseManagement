using CourtDataAccess.Interfaces;
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
        private readonly ILogger<DefendantListModel> _logger;

        public DefendantListModel(
            IDefendant defendant,
            ILogger<DefendantListModel> logger)
        {
            m_defendant = defendant;
            _logger = logger;
        }

        public List<Defendant> Defendants { get; set; }
            = new List<Defendant>();

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;
        public int TotalPage { get; set; }

        public IActionResult OnGet(int pageNumber = 1)
        {
            try
            {
                // Claimed user
                var userName = User.Identity?.Name;
                var userType = User.FindFirst("UserType")?.Value;

                // Pagination
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }

                PageNumber = pageNumber;

                int totalRecords =
                    m_defendant.GetTotalDefendantCount();

                TotalPage = (int)Math.Ceiling(
                    (double)totalRecords / PageSize
                );

                Defendants = m_defendant
                    .GetDefendants(PageNumber, PageSize)
                    .ToList();

                return Page();
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading defendant list.");

                TempData["ErrorMessage"] =
                    "An error occurred while loading defendants.";

                return Page();
            }
        }

        public IActionResult OnGetImage(int id)
        {
            try
            {
                var defendant = m_defendant.GetDefendant(id);

                if (defendant == null ||
                    defendant.Picture == null)
                {
                    return NotFound();
                }

                return File(
                    defendant.Picture,
                    "image/jpeg"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading image for defendant {DefendantId}",
                    id);

                return StatusCode(
                    500,
                    "Unable to load defendant image."
                );
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                m_defendant.DeleteDefendant(id);

                TempData["SuccessMessage"] =
                    "Defendant deleted successfully.";

                return RedirectToPage(
                    "/Defendants/DefendantList"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting defendant {DefendantId}",
                    id);

                TempData["ErrorMessage"] =
                    "An error occurred while deleting the defendant.";

                return RedirectToPage(
                    "/Defendants/DefendantList"
                );
            }
        }

        public bool HasComplaints(int defendantId)
        {
            try
            {
                return m_defendant.HasComplaints(defendantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error checking complaints for defendant {DefendantId}",
                    defendantId);

                return true;
            }
        }
    }
}

