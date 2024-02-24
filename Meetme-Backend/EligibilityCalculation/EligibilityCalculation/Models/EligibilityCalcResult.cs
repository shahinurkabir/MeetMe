namespace EligibilityCalculation.Models
{
    public class EligibilityCalcResult {
        public int ParticipantId { get; set; }
        public int  PlanSourceId { get; set; }
        public int PlanId { get; set; }
        public DateTime CalculationDate { get; set; }
        public bool EligibleYN { get; set; }
        public DateTime? EligibilityDateAsOf { get; set; }
        public DateTime? PlanEntryDate { get; set; }
    }


}
