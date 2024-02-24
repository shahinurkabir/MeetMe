using EligibilityCalculation.Models;
using System.Net.Http.Headers;

namespace EligibilityCalculation
{
    public class EligibilityCalculatorUtil
    {
        public static DateTime CalculateAgeRequrementMeetDate(DateTime dateOfBirth, int minAgeREquiredYears = 0, int minAgeRequriedMonths = 0)
        {
            DateTime ageRequiredMeetDate = dateOfBirth.AddYears(minAgeREquiredYears).AddMonths(minAgeRequriedMonths).AddDays(-1);
            return ageRequiredMeetDate;
        }

        public static DateTime CalculateServiceRequirementMeetDate(DateTime dateOfHire, string servicePeriodCode, int servicePeriodUnit)
        {
            DateTime servicePeriodMeetDate = dateOfHire;

            if (servicePeriodCode == Utils.Constants.Elig_Service_Requirment_PeriodCode_Years)
            {
                servicePeriodMeetDate = dateOfHire.AddYears(servicePeriodUnit).AddDays(-1);
            }
            else if (servicePeriodCode == Utils.Constants.Elig_Service_Requirment_PeriodCode_Months)
            {
                servicePeriodMeetDate = dateOfHire.AddMonths(servicePeriodUnit).AddDays(-1);
            }
            else if (servicePeriodCode == Utils.Constants.Elig_Service_Requirment_PeriodCode_Days)
            {
                servicePeriodMeetDate = dateOfHire.AddDays(servicePeriodUnit).AddDays(-1);
            }

            return servicePeriodMeetDate;
        }


        public static DateTime CalculateHoursRequirementMeetDate(DateTime calculationDate, DateTime dateOfHire, int servicePeriodUnit)
        {
            DateTime servicePeriodMeetDate = calculationDate;

            servicePeriodMeetDate = dateOfHire.AddDays(servicePeriodUnit).AddDays(-1);

            return servicePeriodMeetDate;
        }

        public static DateTime CalculateEntryDate(DateTime eligibilityMeetDateAsOf, DateTime planYearStartDate, DateTime planYearEndDate, string planEntryDateCode, string planEntryTimingCode)
        {
            var isShortPlanYear = (planYearEndDate - planYearStartDate).TotalDays < 365 - 1;

            var planEntryDate = eligibilityMeetDateAsOf;


            if (planEntryDateCode == Utils.Constants.Elig_PlanEntryDateCode_Immediate || planEntryTimingCode == Utils.Constants.Elig_PLanEntryTimeCode_NotApplicable) // Entry Date:Immediate or Plan Entry Timing: Not applicable
            {
                return planEntryDate;
            }

            var intervalDates = CalculateIntervalDates(planYearStartDate, planYearEndDate, planEntryDateCode);

            // Entry Date Timing: Before, After, On or after, Nearest, Preceding

            if (planEntryTimingCode == Utils.Constants.Elig_PLanEntryTimeCode_After) // After
            {
                planEntryDate = intervalDates.Where(x => x > eligibilityMeetDateAsOf).First();
            }
            else if (planEntryTimingCode == Utils.Constants.Elig_PLanEntryTimeCode_ON_OR_After) // On or after
            {
                planEntryDate = intervalDates.Where(x => x >= eligibilityMeetDateAsOf).First();
            }
            else if (planEntryTimingCode == Utils.Constants.Elig_PLanEntryTimeCode_Nearest) // Nearest
            {
                planEntryDate = CalculateNearestDate(intervalDates, eligibilityMeetDateAsOf);
            }
            else if (planEntryTimingCode == Utils.Constants.Elig_PLanEntryTimeCode_Preceding) // Preceding
            {
                planEntryDate = intervalDates.Where(x => x < eligibilityMeetDateAsOf).Last();
            }

            return planEntryDate;

        }


        public static DateTime CalculateNearestDate(List<DateTime> dates, DateTime eligibilityMeetDateAsOf)
        {
            var nearestDate1 = dates.Where(x => x <= eligibilityMeetDateAsOf).OrderByDescending(e => e).First();
            var nearestDate2 = dates.Where(x => x >= eligibilityMeetDateAsOf).OrderBy(e => e).First();

            var dateDiff1 = (eligibilityMeetDateAsOf - nearestDate1).TotalDays;
            var dateDiff2 = (nearestDate2 - eligibilityMeetDateAsOf).TotalDays;

            if (dateDiff1 == dateDiff2)
            {
                return nearestDate1;
            }
            else if (dateDiff1 < dateDiff2)
            {
                return nearestDate1;
            }
            else
            {
                return nearestDate2;
            }
        }

        public static List<DateTime> CalculateIntervalDates(DateTime planYearBeginDate, DateTime planYearEndDate, string planEntryDateCode)
        {
            var beginDate = planYearBeginDate;
            var endDate = planYearEndDate;

            var dateList = new List<DateTime>() { };

            var isShortPlanYear = (endDate - beginDate).TotalDays + 1 < 365;

            if (isShortPlanYear)
            {
                beginDate = endDate.AddYears(-1).AddDays(1);
                if (DateTime.IsLeapYear(beginDate.Year))
                {
                    beginDate = beginDate.AddDays(1);
                }
                dateList.Add(planYearBeginDate);
            }

            var intervalDate = beginDate;
            var isFirstDayOfMonthOption = planEntryDateCode == Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;

            if ((isFirstDayOfMonthOption && intervalDate.Day != 1))
            {
                intervalDate = intervalDate.AddDays(-1 * (intervalDate.Date.Day - 1));
            }

            while (intervalDate <= endDate)
            {

                dateList.Add(intervalDate);

                if (planEntryDateCode == Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth)
                {
                    intervalDate = intervalDate.AddMonths(1);
                }
                else if (planEntryDateCode == Utils.Constants.Elig_PlanEntryDateCode_Quarterly)
                {
                    intervalDate = intervalDate.AddMonths(3);
                }
                else if (planEntryDateCode == Utils.Constants.Elig_PlanEntryDateCode_SemiAnnual)
                {
                    intervalDate = intervalDate.AddMonths(6);
                }
                else if (planEntryDateCode == Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfPlanYear)
                {
                    intervalDate = intervalDate.AddYears(1);
                }
            }

            dateList = dateList.Where(e => e >= planYearBeginDate && e <= planYearEndDate).Distinct().OrderBy(e => e).ToList();

            return dateList;

        }

        public static (DateTime eligibilityMeetDateAsOf, DateTime planEntryDate) CalculateAgeAndServiceWaiver(DateTime eligibilityMeetDate_Calculated, DateTime planEntryDate_Calculated, DateTime immedateEntryDate, DateTime dateOfHire, bool WaiveMinAgeYN, bool WaiveMinServiceYN)
        {
            var eligibilityMeetDateAsOf = eligibilityMeetDate_Calculated;
            var planEntryDate = planEntryDate_Calculated;

            if (dateOfHire > immedateEntryDate) return (eligibilityMeetDateAsOf, planEntryDate);

            if (WaiveMinAgeYN)
            {
                eligibilityMeetDateAsOf = immedateEntryDate < eligibilityMeetDateAsOf ? immedateEntryDate : eligibilityMeetDateAsOf;
            }
            if (WaiveMinServiceYN)
            {
                planEntryDate = immedateEntryDate < planEntryDate ? immedateEntryDate : planEntryDate;
            }

            // Re-calculate eligibility and entry date 
            if (eligibilityMeetDateAsOf == immedateEntryDate && planEntryDate == immedateEntryDate)
            {
                eligibilityMeetDateAsOf = immedateEntryDate;
                planEntryDate = immedateEntryDate;
            }
            else if (eligibilityMeetDateAsOf < immedateEntryDate && planEntryDate < immedateEntryDate)
            {
                eligibilityMeetDateAsOf = eligibilityMeetDate_Calculated;
                planEntryDate = planEntryDate_Calculated < immedateEntryDate ? planEntryDate_Calculated : immedateEntryDate;
            }
            else if (eligibilityMeetDateAsOf == immedateEntryDate && planEntryDate < immedateEntryDate)
            {
                eligibilityMeetDateAsOf = immedateEntryDate;
                planEntryDate = immedateEntryDate;
            }
            else if (eligibilityMeetDateAsOf == immedateEntryDate && planEntryDate > immedateEntryDate)
            {
                eligibilityMeetDateAsOf = eligibilityMeetDate_Calculated;
                planEntryDate = planEntryDate_Calculated;
            }
            else if (eligibilityMeetDateAsOf > immedateEntryDate && planEntryDate <= immedateEntryDate)
            {
                eligibilityMeetDateAsOf = eligibilityMeetDate_Calculated;
                planEntryDate = planEntryDate_Calculated;
            }
            else if (eligibilityMeetDateAsOf > immedateEntryDate && planEntryDate > immedateEntryDate)
            {
                eligibilityMeetDateAsOf = eligibilityMeetDate_Calculated;
                planEntryDate = planEntryDate_Calculated;
            }
            else if (eligibilityMeetDateAsOf < immedateEntryDate && planEntryDate == immedateEntryDate)
            {
                eligibilityMeetDateAsOf = immedateEntryDate;
                planEntryDate = immedateEntryDate;
            }
            else if (eligibilityMeetDateAsOf <= immedateEntryDate && planEntryDate > immedateEntryDate)
            {
                eligibilityMeetDateAsOf = eligibilityMeetDate_Calculated;
                planEntryDate = planEntryDate_Calculated;
            }
            return (eligibilityMeetDateAsOf, planEntryDate);
        }

        public static DateTime? CalculatePlanEntryDateForExcludedType(
              DateTime planYearBeginDate,
              DateTime planYearEndDate,
              DateTime eligibilityProcessingDate,
              DateTime planEntryDateCalculated,
              List<EmployeeStatusHistoryDto> employeeStatusHistoryList,
              List<EmployeeTypeHistoryDto> employeeTypeHistoryList,
              PlanSpecEligibilityDetailDto planSpecEligibilityInfo,
              int partTimeEmployeeTypeId

              )
        {
            DateTime? planEntryDate = null;
            var isExcluededCurrently = false;
            var isExcludedPreviously = false;

            var employeeHired = employeeStatusHistoryList
                                    .Where(e => e.DeletedYN == false &&
                                                e.EmployeeStatusTypeCode == EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired)
                                    .OrderByDescending(e => e.EndDate)
                                    .FirstOrDefault();

            if (employeeHired == null) return planEntryDate;

            var dateOfHired = (DateTime)employeeHired?.StartDate!;

            var employeeTypeCurrently = employeeTypeHistoryList
                                     .Where(e => e.DeletedYN == false &&
                                                 e.EndDate >= eligibilityProcessingDate)
                                     .OrderByDescending(e => e.EndDate)
                                     .FirstOrDefault();

            if (employeeTypeCurrently == null) return planEntryDate;

            var isPartTimeEmployee = employeeTypeCurrently.EmployeeTypeID == partTimeEmployeeTypeId;

            isExcluededCurrently = planSpecEligibilityInfo.ExcludedEEs
                                      .Exists(e => e == employeeTypeCurrently.EmployeeTypeID);


            var employeeTypePrevious = employeeTypeHistoryList
                                      .Where(e => e.DeletedYN == false &&
                                                  e.EndDate < eligibilityProcessingDate)
                                      .OrderByDescending(e => e.EndDate)
                                      .FirstOrDefault();

            if (employeeTypePrevious != null)
            {
                isExcludedPreviously = planSpecEligibilityInfo.ExcludedEEs
                                                  .Exists(e => e == employeeTypePrevious.EmployeeTypeID);

            }


            if (!isExcluededCurrently && !isExcludedPreviously) // never excluded
            {
                planEntryDate = planEntryDateCalculated;
            }
            else if (!isExcluededCurrently && isExcludedPreviously) // Previously excluded
            {
                var dateEndOfExcludedType = (DateTime)employeeTypePrevious?.EndDate!;
                planEntryDate = planEntryDateCalculated < dateEndOfExcludedType ? dateEndOfExcludedType : planEntryDateCalculated;
            }
            else if (isExcluededCurrently && !isPartTimeEmployee) // currently excluded but not part time
            {
                if (planEntryDateCalculated < employeeTypeCurrently?.StartDate)
                {
                    planEntryDate = planEntryDateCalculated;
                }
                else
                {
                    planEntryDate = null;
                }
            }
            else if (isExcluededCurrently && isPartTimeEmployee) // currently excluded and part time
            {
                // TODO: it will different for actual hours
                var dateOfECP = dateOfHired.AddYears(1).AddDays(-1);

                if (eligibilityProcessingDate >= dateOfECP)
                {
                    var serviceRequirementMeetDate = dateOfECP.Date;
                    planEntryDate = CalculateEntryDate(serviceRequirementMeetDate, planYearBeginDate, planYearEndDate, planSpecEligibilityInfo.PlanEntryDateCode, planSpecEligibilityInfo.PlanEntryTimeCode);

                    // TODO:
                    // 1. unmark the employee as active full time
                }
                else
                {
                    planEntryDate = null;
                }

            }

            return planEntryDate;

        }
    }

    public static DateTime? CalculatePlanEntryDateForRehiredEmployee(
        string servicePeriodCode,
        int servicePeriodUnit,
        DateTime rehiredDate,
        DateTime terminationDate,
        DateTime planEntryDateCalculated,
        int YOS,
        bool participantHadVestedBalanceYN,
        bool breakingServiceNonVestedYN,
        bool breakingServiceOneYearYN
        )
    {
        DateTime? planEntryDate = null;
        int ruleOfParity_YOS = 5 > YOS ? 5 : YOS;
        int breakingServiceInYear = 0;//TODO: will discuss with Asif vai

        if (breakingServiceInYear < 1 && rehiredDate > planEntryDateCalculated)
        {
            planEntryDate = rehiredDate;
        }
        else if (breakingServiceInYear < 1 && rehiredDate <= planEntryDateCalculated)
        {
            planEntryDate = planEntryDateCalculated;
        }
        else if (breakingServiceInYear >= 1 && breakingServiceNonVestedYN)
        {
            if (breakingServiceInYear >= ruleOfParity_YOS && !participantHadVestedBalanceYN)
            {
                EligibilityCalculatorUtil.CalculateServiceRequirementMeetDate(rehiredDate, servicePeriodCode, servicePeriodUnit);
            }
            else if (breakingServiceInYear >= ruleOfParity_YOS && participantHadVestedBalanceYN)
            {
                // proceed to step 4
            }
            else if (breakingServiceInYear < ruleOfParity_YOS)
            {
                // proceed to step 4

            }
        }
        else if (breakingServiceInYear>=1 && !breakingServiceOneYearYN)
        {
            // proceed to step 4
        }
        else if (breakingServiceOneYearYN)
        {

        }


        return planEntryDate;
    }

}