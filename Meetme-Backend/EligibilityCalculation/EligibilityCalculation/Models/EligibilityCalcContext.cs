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
            PartCalcEligibilityDto = new PartCalcEligibilityDto();
            PartCalcService = new PartCalcService();
            PlanYear = new PlanYear();
        }
        public DateTime CalculationDate { get; set; }
        public DateTime? LastEPD { get; set; }
        public int UserId { get; set; }
        public PlanYear PlanYear { get; set; }

        public ParticipantDetailDto ParticipantDetailDto { get; set; }
        public PartCalcService PartCalcService { get; set; }
        public PlanSpecEligibilityDetailDto PlanSpecEligibilityDetailDto { get; set; }
        public List<EmployeeStatusHistoryDto> EmployeeStatusHistoryList { get; set; }
        public List<EmployeeTypeHistoryDto> EmployeeTypeHistoryList { get; set; }
        public PlanSpecService PlanSpecService { get; set; }
        public PartCalcEligibilityDto PartCalcEligibilityDto { get; set; }

    }
}
