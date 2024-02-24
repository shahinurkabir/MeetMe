using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityCalculation;
using EligibilityCalculation.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EligibilityCalcUtilTest
{
    [TestClass()]
    public class CalculateExcludedEmployeeType : EligibilityCalcBase
    {
        [TestMethod("There is no exclusion")]
        public void CalculatePlanEntryDate_Never_Excluded_ShouldReturnCorrectDate()
        {
            // Arrange
            var eligibilityProcessingDate = new DateTime(2022, 1, 1);
            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            var planEntryDate = new DateTime(2022, 1, 1);

            var expectedResult = new DateTime(2022, 1, 1);


            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();

            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2022, 1, 1), DateTime.MaxValue);
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2022, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();

            // Act

            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            partTimeEmployeeTypeId: EligibilityCalculation.Utils.Constants.EmployeeType_PartTime
                        );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Value);
        }

        [TestMethod("Currently not excluded but was previously and left from excluded date less than plan entry date")]
        public void CalculatePlanEntryDate_Excluded_Previously_EntryDate_Less_Than_LeftDate_ShouldReturnCorrectDate()
        {
            // Arrange
            var eligibilityProcessingDate = new DateTime(2022, 4, 10);
            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            var planEntryDate = new DateTime(2022, 5, 1);


            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_Other;

            var participantLeftDateOfExclusion = new DateTime(2022, 4, 5);

            var expectedResult = new DateTime(2022, 5, 1); ;

            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();


            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2022, 1, 1), participantLeftDateOfExclusion);
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2022, 4, 6), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2021, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };


            // Act

            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            EligibilityCalculation.Utils.Constants.EmployeeType_PartTime
                        );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Value);
        }

        [TestMethod("Currently not excluded but was previously and left from excluded date greater than plan entry date")]
        public void CalculatePlanEntryDate_Excluded_Previously_LeftDate_Greater_Than_EntryDate_ShouldReturnCorrectDate()
        {
            // Arrange
            var eligibilityProcessingDate = new DateTime(2022, 4, 10);
            var planEntryDate = new DateTime(2022, 4, 1);
            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            var expectedResult = new DateTime(2022, 4, 6);

            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_Other;

            var participantLeftDateOfExclusion = new DateTime(2022, 4, 6);

            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();


            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2022, 1, 1), participantLeftDateOfExclusion);
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2022, 4, 6), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2021, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };


            // Act

            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            EligibilityCalculation.Utils.Constants.EmployeeType_PartTime
                        );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Value);
        }

        [TestMethod("Currently Excluded but not parttime and entry date is less than start date")]
        public void CalculatePlanEntryDate_Excluded_EntryDate_LessThan_StartDate_Not_PartTime_ShouldReturnCorrectDate()
        {
            var eligibilityProcessingDate = new DateTime(2022, 4, 1);

            var planEntryDate = new DateTime(2022, 1, 1);

            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);


            var expectedResult = new DateTime(2022, 1, 1);

            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_Other;
            var partTimeEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_PartTime;
            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();

            // Arrange

            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2022, 1, 1), new DateTime(2022, 3, 31));
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2022, 4, 1), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2022, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };

            // Act


            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            partTimeEmployeeTypeId
                          );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Value);
        }

        [TestMethod("Currently Excluded but not parttime and entry date is greater  than or equal to start date")]
        public void CalculatePlanEntryDate_Excluded_EntryDate_GreaterThanOrEqual_StratDate_Not_PartTime_ShouldReturnCorrectDate()
        {
            var eligibilityProcessingDate = new DateTime(2022, 4, 1);
            var planEntryDate = new DateTime(2022, 3, 1);
            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            DateTime? exptectedResult = null;

            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_Other;


            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();

            // Arrange

            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2022, 1, 1), new DateTime(2022, 2, 28));
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2022, 3, 1), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2022, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };


            // Act
            var expectedResult = planEntryDate;

            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            excludedEmployeeTypeId
                         );

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(exptectedResult, result);
        }

        [TestMethod("Parttime and not completed a year of service")]
        public void CalculatePlanEntryDate_Excluded_PartTime_YIS_Not_Complete()
        {
            var eligibilityProcessingDate = new DateTime(2022, 3, 15);

            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            var planEntryDate = new DateTime(2022, 3, 1);

            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_Other;

            DateTime? exptectedResult = null;

            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();


            // Arrange

            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2022, 1, 1), new DateTime(2022, 1, 31));
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2022, 2, 1), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2022, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };


            // Act
            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            excludedEmployeeTypeId
                         );

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(exptectedResult, result);
        }

        [TestMethod("Part Time and completed Just a year of service")]
        public void CalculatePlanEntryDate_Excluded_PartTime_YOS_Completed()
        {
            var eligibilityProcessingDate = new DateTime(2022, 3, 15);

            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            var planEntryDate = new DateTime(2021, 12, 31);

            var expectedResult = new DateTime(2021, 12, 31);

            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_PartTime;


            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();

            // Arrange

            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2021, 1, 1), new DateTime(2021, 2, 28));
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2021, 3, 1), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2021, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };


            // Act

            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            excludedEmployeeTypeId
                         );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod("Part Time and completed more that a year of service")]
        public void CalculatePlanEntryDate_Excluded_PartTime_More_Than_YOS_Completed()
        {
            var eligibilityProcessingDate = new DateTime(2022, 3, 15);

            var planYearStartDate = new DateTime(2022, 1, 1);
            var planYearEndDate = new DateTime(2022, 12, 31);

            var planEntryDate = new DateTime(2021, 12, 31);

            var expectedResult = new DateTime(2021, 12, 31);

            var excludedEmployeeTypeId = EligibilityCalculation.Utils.Constants.EmployeeType_PartTime;


            var employeeTypeHistoryList = new List<EmployeeTypeHistoryDto>();
            var employeeStatusHistoryList = new List<EmployeeStatusHistoryDto>();

            // Arrange

            // Employee Type History
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeType_Active_FullTime, new DateTime(2021, 1, 1), new DateTime(2021, 2, 27));
            AddEmployeeTypeHistory(employeeTypeHistoryList, 1, excludedEmployeeTypeId, new DateTime(2021, 2, 28), DateTime.MaxValue);

            // Employee Status History
            AddEmployeeStatusHistory(employeeStatusHistoryList, 1, EligibilityCalculation.Utils.Constants.EmployeeStatusTypeCode_Hired, new DateTime(2021, 1, 1), DateTime.MaxValue);

            var planSpecEligibilityInfo = GetEligibilityInstance();
            planSpecEligibilityInfo.ExcludedEEs = new List<int> { excludedEmployeeTypeId };


            // Act

            var result = EligibilityCalculatorUtil.CalculatePlanEntryDateForExcludedType(
                            planYearStartDate,
                            planYearEndDate,
                            eligibilityProcessingDate,
                            planEntryDate,
                            employeeStatusHistoryList,
                            employeeTypeHistoryList,
                            planSpecEligibilityInfo,
                            excludedEmployeeTypeId
                         );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }
    }

}
