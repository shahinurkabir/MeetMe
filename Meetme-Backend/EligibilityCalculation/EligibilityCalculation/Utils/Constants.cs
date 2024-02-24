using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityCalculation.Utils
{
    public class Constants
    {
        public const string SourceCategoryCode_Deferral = "D";
        public const string SourceCategoryCode_Matching = "M";
        public const string SourceCategoryCode_SafeHarbor = "S";
        public const string SourceCategoryCode_Employee = "E";

        public const int EmployeeType_PartTime = 1;
        public const int EmployeeType_Active_FullTime = 2;
        public const int EmployeeType_Terminated = 3;
        public const int EmployeeType_Other = 100;

        public const string EmployeeStatusCategory_Employed = "E";
        public const string EmployeeStatusCategory_Leave_Of_Absence = "L";
        public const string EmployeeStatusCategory_Terminated = "N";

        public const string EmployeeStatusTypeCode_Hired = "EH";
        public const string EmployeeStatusTypeCode_Rehired = "ER";
        public const string EmployeeStatusTypeCode_Terminated = "NT";

        public const string Elig_Calc_Method_ActulaHours = "A";
        public const string Elig_Calc_Method_ElapsedTime = "E";
        public const string Elig_Calc_Method_DOL_Equivalency = "D";

        public const string Elig_Service_Requirment_PeriodCode_Days = "D";
        public const string Elig_Service_Requirment_PeriodCode_Months = "M";
        public const string Elig_Service_Requirment_PeriodCode_Years = "Y";

        public const string Elig_Computation_Method_Anniversary_PlanYear = "AP";
        public const string Elig_Computation_Method_PlanYear = "PY";
        public const string Elig_Computation_Method_Annirversary = "AY";

        public const string Elig_PLanEntryTimeCode_NotApplicable = "A";
        public const string Elig_PLanEntryTimeCode_After = "F";
        public const string Elig_PLanEntryTimeCode_ON_OR_After = "O";
        public const string Elig_PLanEntryTimeCode_Nearest = "N";
        public const string Elig_PLanEntryTimeCode_Preceding = "P";

        public const string Elig_PlanEntryDateCode_Immediate = "IM";
        public const string Elig_PlanEntryDateCode_SemiAnnual = "SA";
        public const string Elig_PlanEntryDateCode_Quarterly = "QR";
        public const string Elig_PlanEntryDateCode_FirstDayOfCalendarMonth = "CM";
        public const string Elig_PlanEntryDateCode_FirstDayOfPayrollPeriod = "PP";
        public const string Elig_PlanEntryDateCode_FirstDayOfPlanYear = "PY";

    }
}
