namespace CourtDomain.ViewModels
{
    public class PetitionDto
    {
        public DateTime PetitionDate { get; set; }
        public int ComplaintId { get; set; }
        public string DefendantName { get; set; }
        public int ComplaintType { get; set; }
        public string ApproveBy { get; set; }
        public bool FinishAndLock { get; set; }
    }
}
