namespace EligibilityCalculation.Models
{
    public class EmployeeStatusHistoryDto
    {
        public int EmployeeStatusHistoryID { get; set; }
        public int EmployeeID { get; set; }
        //public int CompanyID { get; set; }
        public string EmployeeStatusTypeCode { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Default is 0. store user id if user overrides
        /// </summary>
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool DeletedYN { get; set; }
        public string? Note { get; set; }
    }


}
