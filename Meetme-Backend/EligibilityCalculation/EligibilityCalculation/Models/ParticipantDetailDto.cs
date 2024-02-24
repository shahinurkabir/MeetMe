namespace EligibilityCalculation.Models
{
    public class ParticipantDetailDto
    {
        public int ParticipantId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfHired { get; set; }
    }


}
