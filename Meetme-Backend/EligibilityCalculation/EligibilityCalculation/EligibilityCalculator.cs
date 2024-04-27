using EligibilityCalculation.Models;
using Microsoft.VisualBasic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EligibilityCalculation
{

    public class EligibilityCalculator
    {
        private readonly EligibilityCalcContext _eligibilityCalcContext;
        private readonly EligibilityCalcResult _eligibilityCalcResult;
        private EmployeeStatusHistoryDto _employmentStatusCurrent;
        private readonly bool _isHired = false;
        private readonly bool _isRehired = false;
        private readonly bool _isTerminatedYN = false;
        private DateTime _dateOfHired;
        private DateTime _dateOfBirth;
        private DateTime _dateOfRehired;
        private DateTime _dateOfTerminated;
        public EligibilityCalculator(EligibilityCalcContext eligibilityCalcContext)
        {
            this._eligibilityCalcContext = eligibilityCalcContext;
            this._eligibilityCalcResult = new EligibilityCalcResult();
            this._employmentStatusCurrent = new EmployeeStatusHistoryDto();
        }

        private void Init()
        {
            _eligibilityCalcContext.PartCalcEligibilityDto = new PartCalcEligibilityDto
            {
                PartCalcEligibilityID = _eligibilityCalcContext.ParticipantDetailDto.ParticipantId,
                ParticipantID = _eligibilityCalcContext.ParticipantDetailDto.ParticipantId,
                PlanSourceID = _eligibilityCalcContext.PlanSpecEligibilityDetailDto.PlanSourceId,
                CalculationDate = _eligibilityCalcContext.CalculationDate,
                MetEligibilityCode = _eligibilityCalcContext.PartCalcEligibilityDto.MetEligibilityCode,
                MetEligibilityAsOf = _eligibilityCalcContext.PartCalcEligibilityDto.MetEligibilityAsOf,
                EntryDate = _eligibilityCalcContext.PartCalcEligibilityDto.EntryDate,
            };

            // eliminate future records
            _eligibilityCalcContext.EmployeeStatusHistoryList = _eligibilityCalcContext.EmployeeStatusHistoryList
                .Where(e => e.DeletedYN == false && e.EndDate >= _eligibilityCalcContext.CalculationDate)
                .ToList();

            var employmentStatusHired = (_eligibilityCalcContext.EmployeeStatusHistoryList
                               .Where(e => e.DeletedYN == false && e.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Hired)
                               .FirstOrDefault());

            if (employmentStatusHired == null)
            {
                throw new ApplicationException("There is no status for employee hired");
            }

            this._dateOfHired = employmentStatusHired.StartDate;

            _eligibilityCalcContext.EmployeeTypeHistoryList = _eligibilityCalcContext.EmployeeTypeHistoryList
                .Where(e => e.DeletedYN == false && e.EndDate >= _eligibilityCalcContext.CalculationDate)
                .ToList();

            _employmentStatusCurrent = (_eligibilityCalcContext.EmployeeStatusHistoryList
                .Where(e => e.DeletedYN == false && e.EndDate >= _eligibilityCalcContext.CalculationDate)
                .OrderBy(e => e.StartDate)
                .First());

            var employeeCurrentlyHiredYN = _employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Hired;
            var employeeCurrentlyRehiredYN = _employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Rehired;
            var employeeCurrentlyTerminatedYN = _employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Terminated;
        }

        public async Task<EligibilityCalcResult> RunAsync()
        {
            try
            {
                Init();

                if (_eligibilityCalcContext.PlanSpecService.BreakingServiceOneYearYN && _isRehired)
                {
                    await HandleRehiredEmployeeAsync();
                }
                else if (_eligibilityCalcContext.PartCalcEligibilityDto.EntryDate.HasValue)
                {
                    await ReCalcEntryDateAsync();
                }
                else
                {
                    await CalcEntryDateAsync();
                }

            }
            catch (Exception ex)
            {
                _eligibilityCalcResult.HasError = true;
                _eligibilityCalcResult.ErrorMessage = ex.Message;
            }

            return _eligibilityCalcResult;

        }
        private async Task HandleRehiredEmployeeAsync()
        {
            await Task.CompletedTask;
        }

        private async Task ReCalcEntryDateAsync()
        {
            var dateOfBirthChangedYN = false;
            var dateOfHiredChangedYN = false;

            if (dateOfBirthChangedYN || dateOfHiredChangedYN)
            {
                await CalcEntryDateAsync();
            }
            if (_isTerminatedYN)
            {
                if (_eligibilityCalcContext.LastEPD.HasValue && _eligibilityCalcContext.LastEPD.Value < _employmentStatusCurrent.StartDate)
                {
                    _eligibilityCalcResult.PartStatusHistoryList.Add(new PartStatusHistoryDto
                    {
                        ParticipantID = _eligibilityCalcContext.ParticipantDetailDto.ParticipantId,
                        PartStatusTypeCode = "XS", // inactive
                        StartDate = _employmentStatusCurrent.StartDate,
                        EndDate = DateTime.MaxValue,
                        DeletedYN = false,
                        CreatedBy = _eligibilityCalcContext.UserId,
                        CreatedDate = _eligibilityCalcContext.CalculationDate,
                        Note = "Added record from Eligbility Calc Processs",
                    }) ;
                }

            }
            await Task.CompletedTask;
        }

        private async Task CalcEntryDateAsync()
        {

            var ageRequirementMetDate = EligibilityCalculatorUtil.CalculateAgeRequrementMeetDate(
                dateOfBirth: _eligibilityCalcContext.ParticipantDetailDto.DateOfBirth,
                minAgeREquiredYears: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequirementYears,
                minAgeRequriedMonths: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequrementMonths
                );

            if (ageRequirementMetDate > _eligibilityCalcContext.CalculationDate)
            {
                _eligibilityCalcResult.HasError = true;
                _eligibilityCalcResult.ErrorMessage = $"Age requirement date is -${ageRequirementMetDate}";
                return;
            }

            _eligibilityCalcResult.PartCalcEligibilityDto.MetEligibilityCode = "NS";
            _eligibilityCalcResult.PartCalcEligibilityDto.MetEligibilityAsOf = null;
            _eligibilityCalcResult.PartCalcEligibilityDto.EntryDate = null;

            // service requirement age date
            var serviceRequiredMetDate = EligibilityCalculatorUtil.CalculateServiceRequirementMeetDate(
                dateOfHire: _dateOfHired,
                servicePeriodCode: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodCode,
                servicePeriodUnit: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodUnits
                );

            if (serviceRequiredMetDate > _eligibilityCalcContext.CalculationDate)
            {
                _eligibilityCalcResult.HasError = true;
                _eligibilityCalcResult.ErrorMessage = $"Service requirement date is -${serviceRequiredMetDate}";
                return;
            }

            var eligibilityMetDate = ageRequirementMetDate > serviceRequiredMetDate ? ageRequirementMetDate : serviceRequiredMetDate;

            // entry date calculation

            var planEntryDate = EligibilityCalculatorUtil.CalculateEntryDate(
                    eligibilityMetDateAsOf: eligibilityMetDate,
                    planYearStartDate: _eligibilityCalcContext.PlanYear.PlanYearStartDate,
                    planYearEndDate: _eligibilityCalcContext.PlanYear.PlanYearStartDate,
                    planEntryDateCode: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.PlanEntryDateCode,
                    planEntryTimingCode: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.PlanEntryTimeCode
                    );


            var datesConsiderAgeAndWaiver = EligibilityCalculatorUtil.CalculateAgeAndServiceWaiver(
                    eligibilityMetDate_Calculated: eligibilityMetDate,
                    planEntryDate_Calculated: planEntryDate,
                    immedaiteEntryDate: DateTime.Now,
                    dateOfHire: DateTime.Now,
                    minAgeWaiverYN: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeWaiverYN,
                    minServiceWaiverYN: _eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceWaiverYN
                    );

            var planEntryDateConsideredExcludedType = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                    planYearBeginDate: _eligibilityCalcContext.PlanYear.PlanYearStartDate,
                    planYearEndDate: _eligibilityCalcContext.PlanYear.PlanYearStartDate,
                    eligibilityProcessingDate: _eligibilityCalcContext.CalculationDate,
                    planEntryDateCalculated: datesConsiderAgeAndWaiver.planEntryDate,
                    employeeStatusHistoryList: _eligibilityCalcContext.EmployeeStatusHistoryList,
                    employeeTypeHistoryList: _eligibilityCalcContext.EmployeeTypeHistoryList,
                    planSpecEligibilityInfo: _eligibilityCalcContext.PlanSpecEligibilityDetailDto,
                    partTimeEmployeeTypeId: Utils.Constants.EmployeeType_PartTime
                    );

            _eligibilityCalcResult.PartCalcEligibilityDto.MetEligibilityCode = "YS";
            _eligibilityCalcResult.PartCalcEligibilityDto.MetEligibilityAsOf = eligibilityMetDate;
            _eligibilityCalcResult.PartCalcEligibilityDto.EntryDate = planEntryDateConsideredExcludedType;


            await Task.CompletedTask;
        }

        //private static async Task<EligibilityCalcResult> CalculateEntryDate(EligibilityCalcContext eligibilityCalcContext)
        //{
        //    var result = new EligibilityCalcResult
        //    {
        //        ParticipantId = eligibilityCalcContext.ParticipantDetailDto.ParticipantId,
        //        PlanId = eligibilityCalcContext.PlanId,
        //        CalculationDate = eligibilityCalcContext.CalculationDate,
        //        EligibilityDateAsOf = null,
        //        EntryDate = null
        //    };

        //    var ageRequirementMeetDate = EligibilityCalculatorUtil
        //        .CalculateAgeRequrementMeetDate(
        //        dateOfBirth: eligibilityCalcContext.ParticipantDetailDto.DateOfBirth,
        //        minAgeREquiredYears: eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequirementYears,
        //        minAgeRequriedMonths: eligibilityCalcContext.PlanSpecEligibilityDetailDto.AgeRequrementMonths
        //        );

        //    if (ageRequirementMeetDate > eligibilityCalcContext.CalculationDate)
        //    {
        //        return await Task.FromResult(result);
        //    }

        //    var employmentStatusCurrent = (eligibilityCalcContext.EmployeeStatusHistoryList
        //        .Where(e => e.DeletedYN == false && e.EndDate >= eligibilityCalcContext.CalculationDate)
        //        .OrderBy(e => e.StartDate)
        //        .First());

        //    if (employmentStatusCurrent == null)
        //    {
        //        return await Task.FromResult(result);
        //    }

        //    var isHired = employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Hired;
        //    var isRehired = employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Rehired;
        //    var isTerminated = employmentStatusCurrent.EmployeeStatusTypeCode == Utils.Constants.EmployeeStatusTypeCode_Terminated;


        //    if (isTerminated && eligibilityCalcContext.LastEPD > employmentStatusCurrent.StartDate)
        //    {
        //        return await Task.FromResult(result);
        //    }
        //    else if (isHired || (isTerminated && eligibilityCalcContext.LastEPD <= employmentStatusCurrent.StartDate))
        //    {
        //        var serviceRequiredMeetDate = EligibilityCalculatorUtil
        //            .CalculateServiceRequirementMeetDate(
        //            dateOfHire: eligibilityCalcContext.ParticipantDetailDto.DateOfHired,
        //            servicePeriodCode: eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodCode,
        //            servicePeriodUnit: eligibilityCalcContext.PlanSpecEligibilityDetailDto.ServiceRequirementPeriodUnits
        //            );

        //        if (serviceRequiredMeetDate > eligibilityCalcContext.CalculationDate)
        //        {
        //            return await Task.FromResult(result);
        //        }
        //    }
        //    else if (isRehired)
        //    {

        //    }




        //    var eligibilityMeetDate = ageRequirementMeetDate > serviceRequiredMeetDate ? ageRequirementMeetDate : serviceRequiredMeetDate;

        //    var planEntryDate = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDate, planYearBegin, planYearEnd, planEntryDateCode, planEntryTimeCode);

        //    var datesConsiderAgeAndWaiver = EligibilityCalculatorUtil.CalculateAgeAndServiceWaiver(eligibilityMeetDate, planEntryDate, DateTime.Now, DateTime.Now, ageWaiverYN, serviceWaiverYN);

        //    var planEntryDateConsiderExcludedType = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
        //                                                planYearBeginDate: eligibilityCalcContext.PlanYearBegin,
        //                                                planYearEndDate: planYearEnd,
        //                                                eligibilityProcessingDate: calculationDate,
        //                                                planEntryDateCalculated: datesConsiderAgeAndWaiver.planEntryDate,
        //                                                employeeStatusHistoryList: employeeStatusHistoryList,
        //                                                employeeTypeHistoryList: employeeTypeHistoryList,
        //                                                planSpecEligibilityInfo: planSpecEligibilityDetailDto,
        //                                                partTimeEmployeeTypeId: Utils.Constants.EmployeeType_PartTime
        //                                            );


        //    result.EligibilityDateAsOf = datesConsiderAgeAndWaiver.eligibilityMeetDateAsOf;
        //    result.EntryDate = planEntryDateConsiderExcludedType;

        //    return await Task.FromResult(result);
        //}
        //private static void ReCalculateEntryDate(EligibilityCalcContext eligibilityCalcContext)
        //{

        //    if (eligibilityCalcContext.PlanSpecService.BreakingServiceOneYearYN)
        //    {
        //        var isRehired = eligibilityCalcContext.EmployeeTypeHistoryList.Where(x => x.DeletedYN == false && x.EndDate >= eligibilityCalcContext.CalculationDate).Any();

        //        if (isRehired)
        //        {
        //            // TODO: Handle rehired employee
        //        }
        //    }

        //    // TODO: Handle date of birth and date of hire since last EPD

        //}
    }
}