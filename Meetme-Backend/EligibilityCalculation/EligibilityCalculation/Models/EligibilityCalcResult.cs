namespace EligibilityCalculation.Models
{
    public class EligibilityCalcResult {

        public PartCalcEligibilityDto PartCalcEligibilityDto { get; set; } = new PartCalcEligibilityDto();

        public  List <PartStatusHistoryDto> PartStatusHistoryList { get; set; }=new List<PartStatusHistoryDto>();
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; } = "";
    }

    public class PartStatusHistoryDto
    {
        public int PartStatusHistoryID { get; set; }
        public int ParticipantID { get; set; }
        /// <summary>
        /// StatusReasonCode	PartStatus	PartStatusReason
        /// EE		Eligible	 
        /// IA		Ineligible		Age
        /// IH		Ineligible		Hours
        /// IS		Ineligible		Service
        /// IT		Ineligible		Excluded Employee Type
        /// IE		Ineligible		Employment Status
        /// IP		Ineligible		Excluded by Other Plan
        /// ID		Ineligible		Excluded Division
        /// IQ		Ineligible		QDRO Recipient
        /// IB		Ineligible		Non-Employee Beneficiary
        /// IO		Ineligible		Elects Out of Plan
        /// AS		Inactive		Employment Status
        /// AT		Inactive		Excluded Employee Type
        /// </summary>
        public string PartStatusTypeCode { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool DeletedYN { get; set; }
        public string? Note { get; set; }

    }

}
