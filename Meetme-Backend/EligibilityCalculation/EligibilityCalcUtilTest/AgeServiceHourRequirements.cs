namespace EligibilityCalcUtilTest
{
    using EligibilityCalculation;
    using EligibilityCalculation.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    [TestClass]
    public class AgeServiceHourRequirements : EligibilityCalcBase
    {
        [TestMethod("Age Requirement")]
        public void Calculate_AgeRequrementMeetDate()
        {
            // Arrange
            DateTime dateOfBirth = new DateTime(1980, 4, 15);
            int minAgeRequiredYear = 21;
            int minAgeRequiredMonth = 0;

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateAgeRequrementMeetDate(dateOfBirth, minAgeRequiredYear, minAgeRequiredMonth);

            // Assert
            DateTime expected = new DateTime(2001, 4, 14);

            Assert.AreEqual(expected, result);
        }


        [TestMethod("Service Requirement:Days")]
        public void Calculate_ServiceRequirementMeetDate_Days()
        {
            // Arrange
            DateTime dateOfHire = new DateTime(2023, 1, 1);
            string servicePeriodCode = EligibilityCalculation.Utils.Constants.Elig_Service_Requirment_PeriodCode_Days;
            int servicePeriodUnit = 30;

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateServiceRequirementMeetDate(dateOfHire, servicePeriodCode, servicePeriodUnit);

            // Assert
            DateTime expected = new DateTime(2023, 1, 30);

            Assert.AreEqual(expected, result);
        }

        [TestMethod("Service Requirement:Months")]
        public void Calculate_ServiceRequirementMeetDate_Months()
        {
            // Arrange
            DateTime dateOfHire = new DateTime(2023, 1, 1);
            string servicePeriodCode = EligibilityCalculation.Utils.Constants.Elig_Service_Requirment_PeriodCode_Months;
            int servicePeriodUnit = 3;

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateServiceRequirementMeetDate(dateOfHire, servicePeriodCode, servicePeriodUnit);

            // Assert
            DateTime expected = new DateTime(2023, 3, 31);
            Assert.AreEqual(expected, result);
        }

        [TestMethod("Service Requirement:Years")]
        public void Calculate_ServiceRequirementMeetDate_Years()
        {
            // Arrange
            DateTime dateOfHire = new DateTime(2023, 1, 1);
            string servicePeriodCode = EligibilityCalculation.Utils.Constants.Elig_Service_Requirment_PeriodCode_Years;
            int servicePeriodUnit = 1;

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateServiceRequirementMeetDate(dateOfHire, servicePeriodCode, servicePeriodUnit);

            // Assert
            DateTime expected = new DateTime(2023, 12, 31);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CalculateHoursRequirementMeetDate_ReturnsCorrectDate()
        {
            // Arrange
            DateTime calculationDate = new DateTime(2023, 1, 1);
            DateTime dateOfHire = new DateTime(2020, 1, 1);
            int servicePeriodUnit = 60;

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateHoursRequirementMeetDate(calculationDate, dateOfHire, servicePeriodUnit);

            // Assert
            DateTime expected = new DateTime(2020, 2, 29);
            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_MonthlyInterval()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 1, 1);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "CM";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 1, 1),
        //    new DateTime(2023, 2, 1),
        //    new DateTime(2023, 3, 1),
        //    new DateTime(2023, 4, 1),
        //    new DateTime(2023, 5, 1),
        //    new DateTime(2023, 6, 1),
        //    new DateTime(2023, 7, 1),
        //    new DateTime(2023, 8, 1),
        //    new DateTime(2023, 9, 1),
        //    new DateTime(2023, 10, 1),
        //    new DateTime(2023, 11, 1),
        //    new DateTime(2023, 12, 1)
        //}, result);
        //}

        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_QuarterlyInterval()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 1, 1);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "QR";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 1, 1),
        //    new DateTime(2023, 4, 1),
        //    new DateTime(2023, 7, 1),
        //    new DateTime(2023, 10, 1),
        //}, result);
        //}

        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_SemiAnnualInterval()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 1, 1);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "SA";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 1, 1),
        //    new DateTime(2023, 7, 1),
        //}, result);
        //}
        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_YearlyInterval()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 1, 1);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "PY";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 1, 1),
        //}, result);
        //}

        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_MonthlyInterval_ShortPlanYear()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 5, 15);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "CM";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 5, 15),
        //    new DateTime(2023, 6, 1),
        //    new DateTime(2023, 7, 1),
        //    new DateTime(2023, 8, 1),
        //    new DateTime(2023, 9, 1),
        //    new DateTime(2023, 10, 1),
        //    new DateTime(2023, 11, 1),
        //    new DateTime(2023, 12, 1)
        //}, result);
        //}

        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_QuarterlyyInterval_ShortPlanYear()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 5, 15);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "QR";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 5, 15),
        //    new DateTime(2023, 7, 1),
        //    new DateTime(2023, 10, 1),
        //}, result);
        //}
        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_SemiAnnualInterval_ShortPlanYear()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 5, 15);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "SA";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 5, 15),
        //    new DateTime(2023, 7, 1),
        //}, result);
        //}

        //[TestMethod]
        //public void GetIntervalDates_ReturnsCorrectDates_AnnualInterval_ShortPlanYear()
        //{
        //    // Arrange
        //    DateTime beginDate = new DateTime(2023, 5, 15);
        //    DateTime endDate = new DateTime(2023, 12, 31);
        //    string intervalName = "PY";

        //    // Act
        //    List<DateTime> result = EligibilityCalcUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

        //    // Assert
        //    CollectionAssert.AreEqual(new List<DateTime>
        //{
        //    new DateTime(2023, 5, 15),
        //}, result);
        //}



       
    }

}
