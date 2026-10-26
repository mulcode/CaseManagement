using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;



namespace Court.Pages.Documents
{
    public class DocumentModel : PageModel
    {
        private readonly IDocument d_document;
        private readonly IComplaint c_complaint;
        private readonly IPetition p_petition;
        private readonly IDefendant d_defendant;    

        public DocumentModel(IDocument document,IComplaint complaint,IPetition petition, IDefendant defendant)
        {
            d_document = document;
            c_complaint = complaint;
            p_petition = petition;
            d_defendant = defendant;
        }

        [BindProperty]
        public Document DocumentInfo { get; set; } 

        [BindProperty]
        public IFormFile? DocFile { get; set; }
        //public bool IsDecisionSaved { get; set; }
        //public int PetitionId { get; set; }
        //public bool IsPetitionFinished { get; set; }

        [BindProperty]
        public int ComplaintId { get; set; }
        //public int DefendantId { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int documentId, int complaintId)
        {
            ComplaintId = complaintId;

            if (documentId > 0)
            {
                DocumentInfo = d_document.GetDocument(documentId);

                ComplaintId = DocumentInfo.ComplaintId;
            }
            else
            {
                DocumentInfo = new Document
                {
                    DocumentDate = DateTime.Now,
                    ComplaintId = complaintId
                };
             
            }

            LoadTabs();

            return Page();
        }

        public IActionResult OnPost()
        {
            ComplaintId = DocumentInfo.ComplaintId;

            if (!ModelState.IsValid)
            {
                LoadTabs();
                return Page();
            }

            if (DocumentInfo.Id == 0)
            {
                if (DocFile == null || DocFile.Length == 0)
                {
                    ModelState.AddModelError("DocFile", "Please upload a document.");
                    LoadTabs();
                    return Page();
                }

                using var memoryStream = new MemoryStream();
                DocFile.CopyTo(memoryStream);

                DocumentInfo.UploadDocument = memoryStream.ToArray();
                DocumentInfo.FileContentType = DocFile.ContentType;
                d_document.AddDocument(DocumentInfo);
                TempData["SuccessMessage"] = "Document Saved Successfully";
            }
            else
            {
                var existingDocument = d_document.GetDocument(DocumentInfo.Id);

                existingDocument.DocumentDate = DocumentInfo.DocumentDate;
                existingDocument.DocumentName = DocumentInfo.DocumentName;
                existingDocument.DocumentType = DocumentInfo.DocumentType;
                existingDocument.Comments = DocumentInfo.Comments;
                existingDocument.ComplaintId = DocumentInfo.ComplaintId;

                if (DocFile != null && DocFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    DocFile.CopyTo(memoryStream);

                    existingDocument.UploadDocument = memoryStream.ToArray();
                    existingDocument.FileContentType = DocFile.ContentType;
                }

                d_document.UpdateDocument(existingDocument);

                TempData["SuccessMessage"] = "Document Updated Successfully";
            }

            return RedirectToPage("/Documents/DocumentList",
                new { complaintId = DocumentInfo.ComplaintId });
        }
        private void LoadTabs()
        {
            var complaint = c_complaint.GetComplaint(ComplaintId);

            var petition = p_petition.GetPetitionByComplaintId(ComplaintId);

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
