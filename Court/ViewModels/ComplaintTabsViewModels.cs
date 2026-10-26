using CourtDomain.Enums;

namespace Court.ViewModels
{
    public class ComplaintTabsViewModels
    {
        public int ComplaintId { get; set; }
        public int DefendantId { get; set; }
        public string DefendantName { get; set; }
        public string EnrolledNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string Address { get; set; }
        public bool IsSubmitted { get; set; }

        public bool IsDecisionSaved { get; set; }
        public int PetitionId { get; set; }
        public bool IsPetitionFinished { get; set; }
    }
}
