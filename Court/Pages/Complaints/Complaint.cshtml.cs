
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Complaints
{
    public class ComplaintModel : PageModel
    {
        private readonly IComplaint c_complaint;
        private readonly IPetition c_petition;

        public ComplaintModel(IComplaint complaint, IPetition petition)
        {
            c_complaint = complaint;
            c_petition = petition;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; } 

        public int ComplaintId { get; set; }
        public int PetitionId { get; set; }
        public bool IsDecisionSaved { get; set; }
        public bool IsPetitionFinished { get; set; }


        public IActionResult OnGet(int complaintId, int defendantId) 
        {
            try
            {
                if (complaintId > 0)
                {
                    ComplaintInfo = c_complaint.GetComplaint(complaintId);
                }
                else
                {
                    ComplaintInfo = new Complaint()
                    {
                        DefendantId = defendantId,
                        OffesnseDate = DateTime.Now,
                        Date = DateTime.Today
                    };
                }

                ComplaintId = ComplaintInfo.Id;

                if (ComplaintId > 0)
                {
                    IsDecisionSaved = ComplaintInfo.MagistrateDecision.HasValue;

                    Petition petition = c_petition.GetPetitionByComplaintId(ComplaintId);

                    if (petition != null)
                    {
                        PetitionId = petition.Id;
                        IsPetitionFinished = petition.FinishAndLock;
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception in production
                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while loading the complaint."
                );

                return RedirectToPage("/Complaints/ComplaintList");
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (ComplaintInfo.Id == 0)
                {
                    c_complaint.AddComplaint(ComplaintInfo);

                    TempData["SuccessMessage"] = "Complaint Saved Successfully";

                   
                    return RedirectToPage( "/Complaints/Complaint",
                        new { complaintId = ComplaintInfo.Id }
                    );
                }
                else
                {
                    Complaint complaint = c_complaint.GetComplaint(ComplaintInfo.Id);

                    complaint.Date = ComplaintInfo.Date;
                    complaint.Time = ComplaintInfo.Time;
                    complaint.OffesnseDate = ComplaintInfo.OffesnseDate;
                    complaint.ComplaintType = ComplaintInfo.ComplaintType;
                    complaint.SummaryOfComplaint = ComplaintInfo.SummaryOfComplaint;

                    c_complaint.UpdateComplaint(complaint);

                    TempData["SuccessMessage"] = "Complaint Updated Successfully";
                }

                return RedirectToPage( "/Complaints/Complaint",
                    new { complaintId = ComplaintInfo.Id }
                );
            }
            catch (Exception ex)
            {
                // Log the exception in production
                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while saving the complaint."
                );

                return Page();
            }
        }
    }
}

