namespace EligibilityCalculation.Models
{
    public class PlanSpecVesting
    {
        public PlanSpecVesting()
        {
        }

        public int PlanSpecVestingID { get; set; }
        public int? HoursRequirement { get; set; }
        /// <summary>
        /// A: Actual Hours;
        /// E: Elapsed Time;
        /// D: Hours - DOL Equivalency;
        /// 
        /// For &quot;Elapsed Time&quot;, hours requirement will be disabled.
        /// </summary>
        public string ComputationMethodCode { get; set; } = null!;
        /// <summary>
        /// PY: Plan Year
        /// AY: Anniversary Year
        /// OP: Other Period
        /// 
        /// </summary>
        public string ComputationPeriodCode { get; set; } = null!;
        public string? ComputationPeriodEndDate { get; set; }
        /// <summary>
        /// A: Use DOL equivalency for all employees
        /// N: Use DOL equivalency for employees with no hours in payroll
        /// 
        /// </summary>
        public string? DolEquivalencyMethodCode { get; set; }
        /// <summary>
        /// D: Daily
        /// W: Weekly
        /// S: SemiMonthly
        /// M: Monthly
        /// 
        /// </summary>
        public string? DolEquivalencyFrequencyCode { get; set; }
        public string? VestingSpecialRules { get; set; }
        public bool BreakInServiceNonVestedYN { get; set; }
        /// <summary>
        /// T: Only terminated employees;
        /// A: All employees;
        /// 
        /// </summary>
        public string? BreakInServiceNonVestedEmployeeCode { get; set; }
        public bool BreakInServiceOneYearYN { get; set; }
        /// <summary>
        /// T: Only terminated employees;
        /// A: All employees;
        /// 
        /// </summary>
        public string? BreakInServiceOneYearEmployeeCode { get; set; }
        public bool ExcludeServiceBeforeDateYN { get; set; }
        public bool ExcludeServiceBeforeAgeYN { get; set; }
        public int? ExcludeServiceBeforeAgeYears { get; set; }
        public bool? ERFullyVestedYN { get; set; }
        public bool? DeceasedFullyVestedYN { get; set; }
        public bool? DisabledFullyVestedYN { get; set; }
        public bool? DisabledAndTerminatedFullyVestedYN { get; set; }
        public string? VestingScheduleException { get; set; }

    }
}
