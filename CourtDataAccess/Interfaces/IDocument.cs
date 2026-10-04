using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface IDocument
    {
        List<Document> GetAllDocument();
        Document GetDocument(int id);
        void AddDocument(Document document);
        void UpdateDocument(Document document);
        void DeleteDocument(int id);
    }
}
