using CourtDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace CourtDomain.Models
{
    public class Defendant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Date is Required")]
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        [Required(ErrorMessage = "First name is Required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last name is Required")]
        public string LastName { get; set; }
        public Suffix Suffix { get; set; }
        public string NickName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public bool IsEnrolled { get; set; }
        public string EnrolledNumber { get; set; }
        public bool EnrolledOther { get; set; }
        public string AffiliationName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string ZipCode { get; set; }
        public string EmployerName { get; set; }
        public string Phone { get; set; }
        public byte[] Picture { get; set; }

        public bool ApproachCaution { get; set; }
        public string PhysicalAddress { get; set; }
        public string PhysicalZip { get; set; }
        public string MailingAddress { get; set; }
        public string MailingCity { get; set; }
        public string ComplaintPhone { get; set; }
    }
}
