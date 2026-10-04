using CourtDataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourtDomain.Models;


namespace Court.Pages.Documents
{
    public class DocumentListModel : PageModel
    {
        private readonly IDocument d_document;
        public DocumentListModel(IDocument document)
        {
            d_document = document;
        }

        public List<Document> Documents { get; set; } = new List<Document>();
        public int ComplaintId { get; set; }


        public void OnGet(int complaintId)
        {
            ComplaintId = complaintId;
            Documents = d_document.GetAllDocument()
                .Where(d => d.ComplaintId == complaintId)
                .ToList();
        }

        public IActionResult OnGetViewDocument(int id)
        {
            var document = d_document.GetDocument(id);

            if (document == null || document.UploadDocument == null)
            {
                return NotFound();
            }

            return File(document.UploadDocument, "application/pdf");
        }

        public IActionResult OnPostDelete(int id, int complaintId)
        {
            d_document.DeleteDocument(id);

            return RedirectToPage("/Documents/DocumentList", new
            {
                complaintId = complaintId
            });
        }
    }
}
