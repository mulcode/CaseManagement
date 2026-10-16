namespace Court.ViewModels
{
    public class ComplaintTabsViewModels
    {
        public int ComplaintId { get; set; }

        public bool IsSubmitted { get; set; }

        public bool IsDecisionSaved { get; set; }
        public int PetitionId { get; set; }
        public bool IsPetitionFinished { get; set; }
    }
}
