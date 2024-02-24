namespace EligibilityCalculation.Models
{
    public class PartCalcService
    {
        public int PartCalcServiceID { get; set; }
        public int ParticipantID { get; set; }
        public int PlanYearID { get; set; }
        public DateTime CalculationDate { get; set; }
        public DateTime? EarlyRetirementDate { get; set; }
        public DateTime? NormalRetirementDate { get; set; }
        public DateTime? CatchupEligibilityDate { get; set; }
        /// <summary>
        /// Y: Yes 
        /// N: No
        /// </summary>
        public string? ExcludeRuleOfParityServiceCode { get; set; }
        public DateTime? RuleOfParityStartDate { get; set; }
        public DateTime? RuleOfParityEndDate { get; set; }
        public byte? YearsOfService { get; set; }
        public bool? YearsOfServiceCreditThisYearYN { get; set; }
        public byte? VestingYearsOfService { get; set; }
        public bool? VestingYearsOfServiceCreditThisYearYN { get; set; }
        public byte? ParticipationYearsOfService { get; set; }
        public bool? ParticipationYearsOfServiceCreditThisYearYN { get; set; }
        public byte? BreakInServiceYears { get; set; }
        public byte? FutureYearsOfServiceForNRA { get; set; }
        public byte? EligibilityYearsOfService { get; set; }
        public byte? AgeYears { get; set; }
        public byte? SSRAgeYears { get; set; }
        public byte? SSRAgeMonths { get; set; }

    }
}
