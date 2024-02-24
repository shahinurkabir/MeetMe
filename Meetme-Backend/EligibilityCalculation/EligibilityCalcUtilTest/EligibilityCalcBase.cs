using EligibilityCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityCalcUtilTest
{
    public class EligibilityCalcBase
    {
        public void AddEmployeeTypeHistory(List<EmployeeTypeHistoryDto> employeeTypeHistoryList, int employeeID, int employeeTypeId, DateTime starDate, DateTime endDate)
        {
            if (employeeTypeHistoryList == null)
            {
                employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            }

            var employeeTypeHistory = new EmployeeTypeHistoryDto
            {
                EmployeeTypeHistoryID = employeeTypeHistoryList.Count + 1,
                EmployeeID = employeeID,
                EmployeeTypeID = employeeTypeId,
                CompanyID = 1,
                StartDate = starDate,
                EndDate = endDate,
                Note = "Note",
                DeletedYN = false,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,

            };
            employeeTypeHistoryList.Add(employeeTypeHistory);
        }

        public void AddEmployeeStatusHistory(List<EmployeeStatusHistoryDto> employeeStatusHistoryList, int employeeID, string employeeStatusTypeCode, DateTime starDate, DateTime endDate)
        {
            if (employeeStatusHistoryList == null)
            {
                employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();
            }

            var employeeStatusHistory = new EmployeeStatusHistoryDto
            {
                EmployeeStatusHistoryID = employeeStatusHistoryList.Count + 1,
                EmployeeID = employeeID,
                EmployeeStatusTypeCode = employeeStatusTypeCode,
                StartDate = starDate,
                EndDate = endDate,
                Note = "Note",
                DeletedYN = false,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
            };
            employeeStatusHistoryList.Add(employeeStatusHistory);
        }

        public PlanSpecEligibilityDetailDto GetEligibilityInstance()
        {
            return new PlanSpecEligibilityDetailDto
            {
                PlanSourceId = 1,
                SourceCategoryCode = EligibilityCalculation.Utils.Constants.SourceCategoryCode_Deferral,
                AgeRequirementYears = 22,
                AgeRequrementMonths = 0,
                HoursRequirement = 1000,
                ServiceRequirementPeriodCode = EligibilityCalculation.Utils.Constants.Elig_Service_Requirment_PeriodCode_Years,
                ServiceRequirementPeriodUnits = 1,
                CalculationMethodCode = EligibilityCalculation.Utils.Constants.Elig_Calc_Method_ElapsedTime,
                ComputationMethodCode = EligibilityCalculation.Utils.Constants.Elig_Computation_Method_PlanYear,
                PlanEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_Immediate,
                PlanEntryTimeCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_NotApplicable,
                AgeWaiverYN = false,
                ServiceWaiverYN = false,
                ImmediateEntryDate = null,
                ExcludedEEs = new List<int> { }
            };
            
        }
        
    }
}
    