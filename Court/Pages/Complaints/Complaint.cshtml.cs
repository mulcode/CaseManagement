
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
        public Complaint ComplaintInfo { get; set; } = new Complaint();

        public int ComplaintId { get; set; }
        public int PetitionId { get; set; }
        public bool IsDecisionSaved { get; set; }
        public bool IsPetitionFinished { get; set; }


        public IActionResult OnGet(int? id, int? defendantId)
        {
            try
            {
                if (id.HasValue)
                {
                    ComplaintInfo = c_complaint.GetComplaint(id.Value);

                    if (ComplaintInfo == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    if (!defendantId.HasValue)
                    {
                        return BadRequest();
                    }

                    ComplaintInfo.DefendantId = defendantId.Value;
                    ComplaintInfo.OffesnseDate = DateTime.Now;
                    ComplaintInfo.Date = DateTime.Today;
                }

                ComplaintId = ComplaintInfo.Id;

                if (ComplaintId > 0)
                {
                    IsDecisionSaved =
                        ComplaintInfo.MagistrateDecision.HasValue;

                    var petition = c_petition.GetPetitionByComplaintId(ComplaintId);

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

                return Page();
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

                    TempData["SuccessMessage"] =
                        "Complaint Saved Successfully";

                   
                    return RedirectToPage(
                        "/Complaints/Complaint",
                        new { id = ComplaintInfo.Id }
                    );
                }
                else
                {
                    var complaint =
                        c_complaint.GetComplaint(ComplaintInfo.Id);

                    if (complaint == null)
                    {
                        return NotFound();
                    }

                    complaint.Date = ComplaintInfo.Date;
                    complaint.Time = ComplaintInfo.Time;
                    complaint.OffesnseDate = ComplaintInfo.OffesnseDate;
                    complaint.ComplaintType = ComplaintInfo.ComplaintType;
                    complaint.SummaryOfComplaint =
                    ComplaintInfo.SummaryOfComplaint;

                    c_complaint.UpdateComplaint(complaint);

                    TempData["SuccessMessage"] =
                        "Complaint Updated Successfully";
                }

                return RedirectToPage(
                    "/Complaints/Complaint",
                    new { id = ComplaintInfo.Id }
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

