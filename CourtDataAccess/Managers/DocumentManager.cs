using CourtDataAccess.DbModel;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Managers
{
    public class DocumentManager : IDocument
    {
        private readonly AppDbContext _context;
        public DocumentManager(AppDbContext context)
        {
            _context = context;
        }

        public List<Document> GetAllDocument()
        {
            return _context.Documents.ToList();
        }
        public Document GetDocument(int id)
        {
            return _context.Documents.FirstOrDefault(d => d.Id == id);
        }
        public void AddDocument(Document document)
        {
            _context.Documents.Add(document);
            _context.SaveChanges();
        }
        public void UpdateDocument(Document document)
        {
            _context.Documents.Update(document);
            _context.SaveChanges();
        }
        public void DeleteDocument(int id)
        {
            var document = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                _context.SaveChanges();
            }
        }
    }
}
