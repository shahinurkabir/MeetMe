using EligibilityCalculation.Models;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EligibilityCalculation
{

    public class EligibilityCalculator
    {
        public static async Task<EligibilityCalcResult> CalcAsync(EligibilityCalcContext eligibilityCalcContext)
        {
            var planId = eligibilityCalcContext.PlanId;
            var calculationDate = eligibilityCalcContext.CalculationDate;
            //var planYearBegin = eligibilityCalcContext.DatePlanYearBegin;
            var planYearEnd = eligibilityCalcContext.DatePlanYearEnd;
            var participantDto = eligibilityCalcContext.ParticipantDetailDto;
            var dateOfBirth = participantDto.DateOfBirth;
            var dateOfHired = participantDto.DateOfHired;
            var employeeStatusHistoryList = eligibilityCalcContext.EmployeeStatusHistoryList;
            var employeeTypeHistoryList = eligibilityCalcContext.EmployeeTypeHistoryList;
            var ageRequirementYears = eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequirementYears;
            var ageRequrementMonths = eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequrementMonths;
            var serviceRequirementPeriodCode = eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodCode;
            var serviceRequirementPeriodUnits = eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodUnits;
            var planEntryDateCode = eligibilityCalcContext.PlanSpecEligibilityDetailDto.PlanEntryDateCode;
            var planEntryTimeCode = eligibilityCalcContext.PlanSpecEligibilityDetailDto.PlanEntryTimeCode;
            var ageWaiverYN = eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeWaiverYN;
            var serviceWaiverYN = eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceWaiverYN;
            var planSpecEligibilityDetailDto = eligibilityCalcContext.PlanSpecEligibilityDetailDto;



        }
        private static void HandleRehiredEmployee()
        {

        }
        private static void CalculateNewEntryDate()
        {

        }
        private static void ReCalculateEntryDate()
        {

        }
        private static void RecalculateEntryDate()
        {
            var BreakInServiceOneYearYN = "Y";
            var rehiredYN = true;
            var dateOfBirthChangedYN = true;
            var dateOfHiredChangedYN = true;
            var participantNotEmployeedYN = true;

            if (BreakInServiceOneYearYN=="Y" && rehiredYN)
            {
                HandleRehiredEmployee();
            }
            else if (dateOfBirthChangedYN || dateOfHiredChangedYN)
            {
                CalcilateEntryDate();
            }
            if (participantNotEmployeedYN)
            {

            }
           
        }

        private static void HandleRehiredEmployee()
        {

        }

        private static void CalcilateEntryDate()
        {

        }
        private static async Task<EligibilityCalcResult> CalculateEntryDate(EligibilityCalcContext eligibilityCalcContext)
        {
            var result = new EligibilityCalcResult
            {
                ParticipantId = eligibilityCalcContext.ParticipantDetailDto.ParticipantId,
                PlanId = eligibilityCalcContext.PlanId,
                CalculationDate = eligibilityCalcContext.CalculationDate,
                EligibilityDateAsOf = null,
                PlanEntryDate = null
            };

            var ageRequirementMeetDate = EligibilityCalculatorUtil.CalculateAgeRequrementMeetDate(
                                            dateOfBirth: eligibilityCalcContext.ParticipantDetailDto.DateOfBirth,
                                            minAgeREquiredYears: eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequirementYears,
                                            minAgeRequriedMonths: eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequrementMonths
                                          );

            if (ageRequirementMeetDate > eligibilityCalcContext.CalculationDate)
            {
                return await Task.FromResult(result);
            }

            var employmentStatusCurrent = (eligibilityCalcContext.EmployeeStatusHistoryList
                .Where(e => e.DeletedYN == false && e.EndDate >= eligibilityCalcContext.CalculationDate)
                .OrderBy(e => e.StartDate).First());

            if (employmentStatusCurrent == null)
            {
                return await Task.FromResult(result);
            }

            var isHired = employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Hired;
            var isRehired = employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Rehired;
            var isTerminated = employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Terminated;


            if (isTerminated && eligibilityCalcContext.LastEligibilityProcessedDate > employmentStatusCurrent.StartDate)
            {
                return await Task.FromResult(result);
            }
            else if (isHired || (isTerminated && eligibilityCalcContext.LastEligibilityProcessedDate <= employmentStatusCurrent.StartDate))
            {
                var serviceRequiredMeetDate = EligibilityCalculatorUtil.CalculateServiceRequirementMeetDate(
                                              dateOfHire: eligibilityCalcContext.ParticipantDetailDto.DateOfHired,
                                              servicePeriodCode: eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodCode,
                                              servicePeriodUnit: eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodUnits
                                        );

                if (serviceRequiredMeetDate > eligibilityCalcContext.CalculationDate)
                {
                    return await Task.FromResult(result);
                }
            }
            else if (isRehired)
            {

            }




            var eligibilityMeetDate = ageRequirementMeetDate > serviceRequiredMeetDate ? ageRequirementMeetDate : serviceRequiredMeetDate;

            var planEntryDate = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDate, planYearBegin, planYearEnd, planEntryDateCode, planEntryTimeCode);

            var datesConsiderAgeAndWaiver = EligibilityCalculatorUtil.CalculateAgeAndServiceWaiver(eligibilityMeetDate, planEntryDate, DateTime.Now, DateTime.Now, ageWaiverYN, serviceWaiverYN);

            var planEntryDateConsiderExcludedType = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                                                        planYearBeginDate: eligibilityCalcContext.DatePlanYearBegin,
                                                        planYearEndDate: planYearEnd,
                                                        eligibilityProcessingDate: calculationDate,
                                                        planEntryDateCalculated: datesConsiderAgeAndWaiver.planEntryDate,
                                                        employeeStatusHistoryList: employeeStatusHistoryList,
                                                        employeeTypeHistoryList: employeeTypeHistoryList,
                                                        planSpecEligibilityInfo: planSpecEligibilityDetailDto,
                                                        partTimeEmployeeTypeId: Utils.Constants.EmployeeType_PartTime
                                                    );


            result.EligibilityDateAsOf = datesConsiderAgeAndWaiver.eligibilityMeetDateAsOf;
            result.PlanEntryDate = planEntryDateConsiderExcludedType;

            return await Task.FromResult(result);
        }
        private static void ReCalculateEntryDate(EligibilityCalcContext eligibilityCalcContext)
        {

            if (eligibilityCalcContext.PlanSpecService.BreakingServiceOneYearYN)
            {
                var isRehired = eligibilityCalcContext.EmployeeTypeHistoryList.Where(x => x.DeletedYN == false && x.EndDate >= eligibilityCalcContext.CalculationDate).Any();

                if (isRehired)
                {
                    // TODO: Handle rehired employee
                }
            }

            // TODO: Handle date of birth and date of hire since last EPD

        }
    }
}