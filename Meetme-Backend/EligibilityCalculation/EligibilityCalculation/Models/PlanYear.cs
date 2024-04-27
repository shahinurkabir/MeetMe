namespace EligibilityCalculation.Models
{
    public class PlanYear
    {
        public int PlanYearID { get; set; }
        public int PlanID { get; set; }
        public DateTime PlanYearStartDate { get; set; }
        public DateTime PlanYearEndDate { get; set; }
        public DateTime CompensationYearStartDate { get; set; }
        public DateTime CompensationYearEndDate { get; set; }
        public DateTime LimitationYearStartDate { get; set; }
        public DateTime LimitationYearEndDate { get; set; }
        public bool ShortPlanYearYN { get; set; }
        public bool CompensationLimitOverrideYN { get; set; }
        public decimal? CompensationLimitOverride { get; set; }
        public bool PlanTerminationYN { get; set; }

    }
}
