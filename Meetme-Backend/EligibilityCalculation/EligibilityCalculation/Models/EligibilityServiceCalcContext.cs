namespace EligibilityCalculation.Models
{
    public class EligibilityServiceCalcContext
    {
        public int PlanId { get; set; }
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public DateTime CalculationDate { get; set; }
        public DateTime DatePlanYearBegin { get; set; }
        public DateTime DatePlanYearEnd { get; set; }
        public int UserId { get; set; }

    }


}
