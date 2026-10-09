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
        public DocumentModel(IDocument document)
        {
            d_document = document;
        }

        [BindProperty]
        public Document DocumentInfo { get; set; } = new Document();

        [BindProperty]
        public IFormFile? DocFile { get; set; }

        public void OnGet(int? id, int complaintId)
        {
            if (id.HasValue)
            {
                DocumentInfo = d_document.GetDocument(id.Value);
            }
            else
            {
                DocumentInfo.DocumentDate = DateTime.Now;
                DocumentInfo.ComplaintId = complaintId;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (DocumentInfo.Id == 0)
            {
                if (DocFile == null || DocFile.Length == 0)
                {
                    ModelState.AddModelError("DocFile", "Please upload a document.");
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

                if (existingDocument == null)
                {
                    return NotFound();
                }

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

            return RedirectToPage("/Documents/Document", new
            {
                id = DocumentInfo.Id,
                complaintId = DocumentInfo.ComplaintId
            });
        }
    }
}
