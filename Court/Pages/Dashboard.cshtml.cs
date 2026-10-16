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
        public List<PetitionDto> Petitions { get; set; } = new List<PetitionDto>();
        public List<ArraignemntDto> Arraignments { get; set; } = new();



        public IActionResult OnGet()
        {
            try
            {
                Defendants = d_complaint.GetAwatingComplaintSubmision();
                Decisions = d_complaint.GetAwatingDecision(); 
                ComplaintsWithoutPetition = d_petition.GetComplaintsWithoutPetition();
                Petitions = d_petition.GetPetitions();

                Arraignments = d_petition.GetCourteCases().ToList();

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

