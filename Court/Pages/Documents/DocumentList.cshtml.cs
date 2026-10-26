using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Documents
{
    public class DocumentListModel : PageModel
    {
        private readonly IDocument d_document;
        private readonly IComplaint c_complaint;
        private readonly IPetition d_petition;
        private readonly IDefendant d_defendant;
        private readonly ILogger<DocumentListModel> _logger;

        public DocumentListModel(
            IDocument document,
            IComplaint complaint,
            IPetition petition,
            IDefendant defendant,
            ILogger<DocumentListModel> logger)
        {
            d_document = document;
            c_complaint = complaint;
            d_petition = petition;
            d_defendant = defendant;
            _logger = logger;
        }

        public IList<Document> Documents { get; set; } = new List<Document>();
        public ComplaintTabsViewModels ComplaintTabs { get; set; }
        public int ComplaintId { get; set; }
       

        public IActionResult OnGet(int complaintId)
        {
            try
            {
                ComplaintId = complaintId;           
                Documents = d_document.GetDocumentsByComplaint(complaintId);

                LoadTabs();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error loading documents for ComplaintId {ComplaintId}",
                    complaintId);

                TempData["ErrorMessage"] = "An error occurred while loading documents.";
                return RedirectToPage("/Defendants/DefendantList");  
            }
        }

        public IActionResult OnGetViewDocument(int id)
        {
            try
            {
                var document = d_document.GetDocument(id);

                if (document == null || document.UploadDocument == null)
                {
                    return NotFound();
                }

                return File(
                    document.UploadDocument,
                    document.FileContentType ?? "application/octet-stream"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing document {DocumentId}", id);
                return StatusCode(500, "Unable to view the document.");
            }
        }

        public IActionResult OnPostDelete(int documentId, int complaintId)
        {
            try
            {
                d_document.DeleteDocument(documentId);

                TempData["SuccessMessage"] = "Document deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document {DocumentId}", documentId);

                TempData["ErrorMessage"] = "An error occurred while deleting the document.";
            }

            return RedirectToPage(
                "/Documents/DocumentList",
                new { complaintId = complaintId });
        }

        private void LoadTabs()
        {
            var complaint = c_complaint.GetComplaint(ComplaintId);
            var petition = d_petition.GetPetitionByComplaintId(ComplaintId);
            var defendant = d_defendant.GetDefendant(complaint.DefendantId);

            ComplaintTabs = new ComplaintTabsViewModels
            {
                ComplaintId = complaint.Id,
                DefendantId = complaint.DefendantId,
                DefendantName = $"{defendant.FirstName} {defendant.LastName}".Trim(),
                EnrolledNumber = defendant.EnrolledNumber,
                Address = defendant.Address,
                Gender = defendant.Gender,
                IsDecisionSaved = complaint.MagistrateDecision.HasValue,
                PetitionId = petition?.Id ?? 0,
                IsPetitionFinished = petition?.FinishAndLock ?? false
            };
        }
    }
}