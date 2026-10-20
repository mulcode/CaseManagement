using CourtDataAccess.Interfaces;
using CourtDomain.DTOs;
using CourtDomain.Models;
using CourtDomain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages
{
    [Authorize(Policy = "AuthorizedUsers")]
    public class DashboardModel : PageModel
    {
        private readonly IComplaint d_complaint;
        private readonly IPetition d_petition;
        private readonly ILogger<DashboardModel> _logger;

        public int SubmissionPageNumber { get; set; }
        public int SubmissionTotalPage { get; set; }
        public int DecisionPageNumber { get; set; }
        public int DecisionTotalPage { get; set; }
        public int PetitionPageNumber { get; set; }
        public int PetitionTotalPage { get; set; }

        public int PageSize { get; set; } = 5;
        public int TotalAwatingSubmission { get; set; }
        public int TotalAwatingDecision { get; set; }
        public int TotalPetition { get; set; }

        public DashboardModel(
            IComplaint complaint,
            ILogger<DashboardModel> logger,
            IPetition petition)
        {
            d_complaint = complaint;
            d_petition = petition;
            _logger = logger;
        }

        public IEnumerable<DefendantComplaintDto> Defendants{ get; set; } = new List<DefendantComplaintDto>();
        public IEnumerable<DefendantComplaintDto> Decisions{ get; set; } = new List<DefendantComplaintDto>();

        public List<ComplaintWithoutPetitionDto> ComplaintsWithoutPetition { get; set; } = new();
        public IEnumerable<PetitionDto> Petitions { get; set; } = new List<PetitionDto>();
        public List<ArraignemntDto> Arraignments { get; set; } = new();



        public IActionResult OnGet(int submissionPage = 1, int decisionPage = 1, int petitionPage = 1)
        {
            try
            {
                 
                ComplaintsWithoutPetition = d_petition.GetComplaintsWithoutPetition();
              
                Arraignments = d_petition.GetCourteCases().ToList();

                // submission pagination
                TotalAwatingSubmission = d_complaint.GetAwatingComplaintCount();

                SubmissionTotalPage = (int)Math.Ceiling(
                    (double)TotalAwatingSubmission / PageSize);

                SubmissionPageNumber = Math.Max(1, submissionPage);

                if (SubmissionTotalPage > 0)
                {
                    SubmissionPageNumber = Math.Min(SubmissionPageNumber, SubmissionTotalPage);
                }
                else
                {
                    SubmissionPageNumber = 1;
                }

                Defendants = d_complaint.GetAwatingComplaintSubmision(SubmissionPageNumber, PageSize).ToList();


                //decision pagination
                TotalAwatingDecision = d_complaint.GetAwatingDecisionCount();
                DecisionTotalPage = (int)Math.Ceiling(
                    (double)TotalAwatingDecision / PageSize);

                DecisionPageNumber = Math.Max(1, decisionPage);

                if (DecisionTotalPage > 0)
                {
                    DecisionPageNumber = Math.Min(DecisionPageNumber, DecisionTotalPage);
                }
                else
                {
                    DecisionPageNumber = 1;
                }

                Decisions = d_complaint.GetAwatingDecision(DecisionPageNumber, PageSize).ToList();

                // petition pagination
                TotalPetition = d_petition.GetPetitionCount();

                PetitionTotalPage = (int)Math.Ceiling(
                   (double)TotalPetition / PageSize);

                PetitionPageNumber = Math.Max(1, petitionPage);

                if (PetitionTotalPage > 0)
                {
                    PetitionPageNumber = Math.Min(PetitionPageNumber, PetitionTotalPage);
                }
                else
                {
                    PetitionPageNumber = 1;
                }

                Petitions = d_petition.GetPetitions(PetitionPageNumber,PageSize).ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading dashboard and awaiting complaint submissions.");

                TempData["ErrorMessage"] =
                    "An error occurred while loading the dashboard.";

                return Page();
            }
        }
    }
}

