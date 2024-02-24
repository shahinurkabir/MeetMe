namespace EligibilityCalculation.Models
{
    public partial class PartCalcEligibility
    {
        public int PartCalcEligibilityID { get; set; }
        public int ParticipantID { get; set; }
        public int? PlanSourceID { get; set; }
        public DateTime CalculationDate { get; set; }
        /// <summary>
        /// YS: Yes 
        /// NS: No
        /// YO: Yes - Override
        /// NO: No - Override
        /// </summary>
        public string? MetEligibilityCode { get; set; }
        public DateTime? MetEligibilityAsOf { get; set; }
        public DateTime? EntryDate { get; set; }

    }

}
