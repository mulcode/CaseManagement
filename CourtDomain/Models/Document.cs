using CourtDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourtDomain.Models
{
    public class Document
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Date is required")]
        public DateTime DocumentDate { get; set; }


        [Required(ErrorMessage = "Name is required")]
        public string DocumentName { get; set; }


        public DocumentType DocumentType { get; set; }
        public string FileContentType { get; set; }

        public byte[] UploadDocument { get; set; }

        public string Comments { get; set; }

        public int ComplaintId { get; set; }

        public virtual Complaint Complaint { get; set; }
    }
}
