
using CourtDataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourtDomain.Models;

namespace Court.Pages.Documents
{
    public class DocumentListModel : PageModel
    {
        private readonly IDocument d_document;
        private readonly IComplaint c_complaint;

        public DocumentListModel(
            IDocument document,
            IComplaint complaint)
        {
            d_document = document;
            c_complaint = complaint;
        }

        public List<Document> Documents { get; set; }
            = new List<Document>();

        public int ComplaintId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsDecisionSaved { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            try
            {
                ComplaintId = complaintId;

                Documents = d_document.GetAllDocument()
                    .Where(d => d.ComplaintId == complaintId)
                    .ToList();

                var complaint =
                    c_complaint.GetComplaint(complaintId);

                // Keep your existing status logic here if needed.

                return Page();
            }
            catch (Exception ex)
            {
                // Log exception in production.
                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while loading documents."
                );

                return Page();
            }
        }

        public IActionResult OnGetViewDocument(int id)
        {
            try
            {
                var document = d_document.GetDocument(id);

                if (document == null ||
                    document.UploadDocument == null)
                {
                    return NotFound();
                }

                return File(
                    document.UploadDocument,
                    document.FileContentType
                );
            }
            catch (Exception ex)
            {
                // Log exception in production.
                return StatusCode(500, "Unable to view the document.");
            }
        }

        public IActionResult OnPostDelete(int id, int complaintId)
        {
            try
            {
                d_document.DeleteDocument(id);

                TempData["SuccessMessage"] =
                    "Document deleted successfully.";

                return RedirectToPage(
                    "/Documents/DocumentList",
                    new { complaintId = complaintId }
                );
            }
            catch (Exception ex)
            {
                // Log exception in production.
                TempData["ErrorMessage"] =
                    "An error occurred while deleting the document.";

                return RedirectToPage(
                    "/Documents/DocumentList",
                    new { complaintId = complaintId }
                );
            }
        }
    }
}

