namespace EligibilityCalculation.Models
{
    public class PlanSpecEligibilityDetailDto
    {
        public PlanSpecEligibilityDetailDto()
        {
            ExcludedEEs = new List<int>();
        }
        public int PlanSourceId { get; set; }
        public string SourceCategoryCode { get; set; } = null!;
        public int AgeRequirementYears { get; set; }
        public int AgeRequrementMonths { get; set; }
        /// <summary>
        /// D: Days; M: Months; Y: Years
        /// </summary>
        public string ServiceRequirementPeriodCode { get; set; } = null!;
        public int ServiceRequirementPeriodUnits { get; set; }
        public int HoursRequirement { get; set; }

        /// <summary>
        /// A: Actual Hours;
        /// E: Elapsed Time;
        /// D: Hours - DOL Equivalency;
        /// 
        /// For &quot;Elapsed Time&quot;, hours requirement will be disabled.
        /// </summary>
        public string CalculationMethodCode { get; set; } = null!;
        /// <summary>
        /// AP: Anniversary / Plan Year
        /// PY: Plan Year
        /// AY: Anniversary Year
        /// 
        /// </summary>
        public string ComputationMethodCode { get; set; } = null!;
        /// <summary>
        /// IM: Immediate
        /// SA: Semi-Annual
        /// QR: Quarterly
        /// CM: First Day of Calendar Month
        /// PP: First Day of Payroll Period
        /// PY: First Day of Plan Year
        /// </summary>
        public string PlanEntryDateCode { get; set; } = null!;
        /// <summary>
        /// A: Not Applicable
        /// F: After
        /// O: On or After
        /// N: Nearest
        /// P: Preceding
        /// </summary>
        public string PlanEntryTimeCode { get; set; } = null!;
        public bool AgeWaiverYN { get; set; }
        public bool ServiceWaiverYN { get; set; }
        public DateTime? ImmediateEntryDate { get; set; }

        public List<int> ExcludedEEs { get; set; }
    }


    

}
