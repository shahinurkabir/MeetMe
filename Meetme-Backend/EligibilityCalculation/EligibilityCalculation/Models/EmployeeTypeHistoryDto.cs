using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityCalculation.Models
{

    public class EmployeeTypeHistoryDto
    {
        public int EmployeeTypeHistoryID { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int EmployeeTypeID { get; set; }
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
    public  class EmployeeStatusType
    {
        public string EmployeeStatusTypeCode { get; set; } = null!;
        public string EmployeeStatusTypeName { get; set; } = null!;
        public string EmployeeStatusCategoryCode { get; set; } = null!;

    }

}
