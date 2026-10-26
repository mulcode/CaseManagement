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
        private readonly IArraignment d_arraignment;
        private readonly ILogger<DashboardModel> _logger;

        public int SubmissionPageNumber { get; set; }
        public int SubmissionTotalPage { get; set; }
        public int DecisionPageNumber { get; set; }
        public int DecisionTotalPage { get; set; }
        public int PetitionPageNumber { get; set; }
        public int PetitionTotalPage { get; set; }
        public int PetitionAwatingPageNumber { get; set; }
        public int PetitionAwatingTotalPage { get; set; }
        public int ArraignmentPageNumber { get; set; }
        public int ArraignmentTotalPage { get; set; }

        public int PageSize { get; set; } = 5;
        public int TotalAwatingSubmission { get; set; }
        public int TotalAwatingDecision { get; set; }
        public int TotalPetition { get; set; }
        public int TotalAwatingPetition { get; set; }
        public int TotalArraignment { get; set; }

        public DashboardModel(
            IComplaint complaint,
            ILogger<DashboardModel> logger,
            IArraignment arraignment,
            IPetition petition)
        {
            d_complaint = complaint;
            d_petition = petition;
            _logger = logger;
            d_arraignment = arraignment;
        }

        public IList<DefendantComplaintDto> Defendants{ get; set; } 
        public IList<DefendantComplaintDto> Decisions{ get; set; }

        public IList<ComplaintWithoutPetitionDto> ComplaintsWithoutPetition { get; set; } 
        public IList<PetitionDto> Petitions { get; set; }
        public IList<ArraignemntDto> Arraignments { get; set; } 



        public IActionResult OnGet(int submissionPage = 1, int decisionPage = 1, int petitionPage = 1, int petitionAwatingPage = 1, int arraignmentPage = 1)
        {
            try
            {
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

                //Awating Petition
                TotalAwatingPetition = d_petition.GetAwatingPetitionCount();

                PetitionAwatingTotalPage = (int)Math.Ceiling(
                   (double)TotalAwatingPetition / PageSize);

                PetitionAwatingPageNumber = Math.Max(1, petitionAwatingPage);

                if (PetitionAwatingTotalPage > 0)
                {
                    PetitionAwatingPageNumber = Math.Min(PetitionAwatingPageNumber, PetitionAwatingTotalPage);
                }
                else
                {
                    PetitionAwatingPageNumber = 1;
                }

                ComplaintsWithoutPetition = d_petition.GetComplaintsWithoutPetition(PetitionAwatingPageNumber,PageSize).ToList();

                // Arraignment Pagination
                TotalArraignment = d_arraignment.ArraignmentCount();

                ArraignmentTotalPage = (int)Math.Ceiling(
                    (double)TotalArraignment / PageSize);

                ArraignmentPageNumber = Math.Max(1, arraignmentPage);

                if (ArraignmentTotalPage > 0)
                {
                    ArraignmentPageNumber = Math.Min(ArraignmentPageNumber, ArraignmentTotalPage);
                }
                else
                {
                    ArraignmentPageNumber = 1;
                }

                Arraignments = d_arraignment.GetCourteCases(ArraignmentPageNumber, PageSize).ToList();

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

