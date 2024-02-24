namespace EligibilityCalculation.Models
{
    public class EligibilityCalcContext
    {
        public EligibilityCalcContext()
        {
            PlanSpecEligibilityDetailDto = new PlanSpecEligibilityDetailDto();
            ParticipantDetailDto = new ParticipantDetailDto();
            EmployeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();
            EmployeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            PlanSpecService = new PlanSpecService();
            PartCalcEligibilityList = new List<PartCalcEligibility>();
            PartCalcService = new PartCalcService();
        }
        public int PlanId { get; set; }
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public DateTime CalculationDate { get; set; }
        public DateTime? LastEligibilityProcessedDate { get; set; }
        public DateTime DatePlanYearBegin { get; set; }
        public DateTime DatePlanYearEnd { get; set; }
        public DateTime DateCompensationYearBegin { get; set; }
        public DateTime DateCompensationYearEnd { get; set; }
        public int UserId { get; set; }

        public PlanSpecEligibilityDetailDto PlanSpecEligibilityDetailDto { get; set; }
        public ParticipantDetailDto ParticipantDetailDto { get; set; }
        public List<EmployeeStatusHistoryDto> EmployeeStatusHistoryList { get; set; }
        public List<EmployeeTypeHistoryDto> EmployeeTypeHistoryList { get; set; }
        public PlanSpecService PlanSpecService { get; set; }
        public List<PartCalcEligibility> PartCalcEligibilityList { get; set; }
        public PartCalcService PartCalcService { get; set; }

    }

    public class PlanSpecService
    {
        public bool BreakingServiceOneYearYN { get; set; }
        public bool BreakInServiceNonVestedYN { get; set; }
    }
}
