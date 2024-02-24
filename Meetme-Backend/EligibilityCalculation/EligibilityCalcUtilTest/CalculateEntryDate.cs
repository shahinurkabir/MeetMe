using EligibilityCalculation;

namespace EligibilityCalcUtilTest
{
    [TestClass]
    public class CalculateEntryDate
    {

        [TestMethod("EntryDateCode:Immedaitly and EntryTiming:Not Applicable")]
        public void CalculateEntryDate_EntryDateCode_Immediate_And_EntryTiming_NotApplicable()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 5, 14);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_Immediate;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_NotApplicable;

            var expectedResult=new DateTime(2023,5,14);
            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:After")]
        public void CalculateEntryDate_EntryDateCode_Monthly_And_EntryTiming_After()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 5, 14);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_After;
           
            var expectedResult = new DateTime(2023, 6, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:Preceding")]
        public void CalculateEntryDate_EntryDateCode_Monthly_And_EntryTiming_Preceding()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 5, 14);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_Preceding;
           
            var expectedResult = new DateTime(2023, 5, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:On")]
        public void CalculateEntryDate_EntryDateCode_Monthly_And_EntryTiming_ON()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 4, 1);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_ON_OR_After;
            var expectedResult = new DateTime(2023, 4, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:On or After")]
        public void CalculateEntryDate_EntryDate_Monthly_And_EntryTiming_OnOrAfter()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 4, 10);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_ON_OR_After;
            var expectedResult = new DateTime(2023, 5, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:Nearest")]
        public void CalculateEntryDate_EntryDate_Monthly_And_EntryTiming_Nearest()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 4, 1);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_Quarterly;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_ON_OR_After;
            var expectedResult = new DateTime(2023, 4, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:Nearest_Preceding")]
        public void CalculateEntryDate_EntryDate_Monthly_And_EntryTiming_Nearest_Preceding()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 4, 10);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_Nearest;
            var expectedResult = new DateTime(2023, 4, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("EntryDate:Monthly and EntryTiming:Nearest_Equal")]
        public void CalculateEntryDate_EntryDate_Monthly_And_EntryTiming_Nearest_Equal()
        {
            // Arrange
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 5,16);
            DateTime planYearStartDate = new DateTime(2023, 1, 1);
            DateTime planYearEndDate = new DateTime(2023, 12, 31);
            string planEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;
            string planEntryTimingCode = EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_Nearest;
            var expectedResult = new DateTime(2023, 5, 1);

            // Act
            var result = EligibilityCalculatorUtil.CalculateEntryDate(eligibilityMeetDateAsOf, planYearStartDate, planYearEndDate, planEntryDateCode, planEntryTimingCode);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod("To select nearest entry date")]
        public void CalculateNearestDate_Return_nearest_date()
        {
            // Arrange
            List<DateTime> dates = new List<DateTime>
            {
                new DateTime(2023, 1, 1),
                new DateTime(2023, 7, 1),
            };
            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 3, 1);
            var expectedResult = new DateTime(2023, 1, 1);

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateNearestDate(dates, eligibilityMeetDateAsOf);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod("To select earlier date if two dates are in same differnece")]
        public void CalculateNearestDate_ReturnsNearestDate()
        {
            // Arrange
            List<DateTime> dates = new List<DateTime>
            {
                new DateTime(2023, 1, 1),
                new DateTime(2023, 7, 1),
            };

            DateTime eligibilityMeetDateAsOf = new DateTime(2023, 4, 1);
            var expectedResult = new DateTime(2023, 1, 1);

            // Act
            DateTime result = EligibilityCalculatorUtil.CalculateNearestDate(dates, eligibilityMeetDateAsOf);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

    }

}
