using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourtDataAccess.Managers
{
    public class DocumentManager : IDocument
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DocumentManager> _logger;

        public DocumentManager(
            AppDbContext context,
            ILogger<DocumentManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Document> GetDocumentsByComplaint(int complaintId)
        {
            return _context.Documents
                .Where(d => d.ComplaintId == complaintId)
                .ToList();
        }

        public Document GetDocument(int id)
        {
            try
            {
                return _context.Documents
                    .Include(x => x.Complaint)
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting DocumentId {DocumentId}",
                    id);

                throw;
            }
        }

        public void AddDocument(Document document)
        {
            try
            {
                _context.Documents.Add(document);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding document.");

                throw;
            }
        }

        public void UpdateDocument(Document document)
        {
            try
            {
                _context.Documents.Update(document);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating DocumentId {DocumentId}",
                    document.Id);

                throw;
            }
        }

        public void DeleteDocument(int id)
        {
            try
            {
                var document = _context.Documents
                    .FirstOrDefault(d => d.Id == id);

                if (document != null)
                {
                    _context.Documents.Remove(document);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting DocumentId {DocumentId}",
                    id);

                throw;
            }
        }
    }
}

