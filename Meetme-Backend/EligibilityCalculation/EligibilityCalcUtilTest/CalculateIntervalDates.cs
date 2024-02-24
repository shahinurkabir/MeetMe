using EligibilityCalculation;

namespace EligibilityCalcUtilTest
{
    [TestClass]
    public class CalculateIntervalDates
    {
        [TestMethod("Calculate Intervals:Monthly")]
        public void Calculate_Intervals_Monthly()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 1, 1),
                new DateTime(2023, 2, 1),
                new DateTime(2023, 3, 1),
                new DateTime(2023, 4, 1),
                new DateTime(2023, 5, 1),
                new DateTime(2023, 6, 1),
                new DateTime(2023, 7, 1),
                new DateTime(2023, 8, 1),
                new DateTime(2023, 9, 1),
                new DateTime(2023, 10, 1),
                new DateTime(2023, 11, 1),
                new DateTime(2023, 12, 1)
            }, result);
        }

        [TestMethod("Calculate Intervals:Monthly_Odd_Year")]
        public void Calculate_Intervals_Monthly_Odd_Year()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 10, 16);
            DateTime endDate = new DateTime(2024, 10, 15);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 11, 1),
                new DateTime(2023, 12, 1),
                new DateTime(2024, 1, 1),
                new DateTime(2024, 2, 1),
                new DateTime(2024, 3, 1),
                new DateTime(2024, 4, 1),
                new DateTime(2024, 5, 1),
                new DateTime(2024, 6, 1),
                new DateTime(2024, 7, 1),
                new DateTime(2024, 8, 1),
                new DateTime(2024, 9, 1),
                new DateTime(2024, 10, 1)
            }, result);
        }

        [TestMethod("Calculate Intervals:Quarterly")]
        public void Calculate_Intervals_Quarterly()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_Quarterly;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 1, 1),
                new DateTime(2023, 4, 1),
                new DateTime(2023, 7, 1),
                new DateTime(2023, 10, 1),
            }, result);
        }

        [TestMethod("Calculate Intervals:Semi-Annual")]
        public void Calculate_Intervals_SemiAnnual()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_SemiAnnual;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
            new DateTime(2023, 1, 1),
            new DateTime(2023, 7, 1),
            }, result);
        }

        [TestMethod("Calculate Intervals:Yearly")]
        public void Calculate_Intervals_Yearly()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfPlanYear;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 1, 1),
            }, result);
        }

        [TestMethod("Calculate Intervals:monthly for short plan year")]
        public void Calculate_Intervals_Monthly_ShortPlanYear()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 5, 15);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfCalendarMonth;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 5, 15),
                new DateTime(2023, 6, 1),
                new DateTime(2023, 7, 1),
                new DateTime(2023, 8, 1),
                new DateTime(2023, 9, 1),
                new DateTime(2023, 10, 1),
                new DateTime(2023, 11, 1),
                new DateTime(2023, 12, 1)
            }, result);
        }

        [TestMethod("Calculate intervals:Quarterly for short plan year")]
        public void Calculate_Intervals_Quarley_ShortPlanYear()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 5, 15);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_Quarterly;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 5, 15),
                new DateTime(2023, 7, 1),
                new DateTime(2023, 10, 1),
            }, result);
        }

        [TestMethod("Calculate intervals:Semi-Annualy for short plan year")]
        public void Calculate_Intervals_SemiAnnual_ShortPlanYear()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 5, 15);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_SemiAnnual;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 5, 15),
                new DateTime(2023, 7, 1),
            }, result);
        }

        [TestMethod("Calculate intervals:Annualy for short plan year")]
        public void Calculate_Intervals_Annual_ShortPlanYear()
        {
            // Arrange
            DateTime beginDate = new DateTime(2023, 5, 15);
            DateTime endDate = new DateTime(2023, 12, 31);
            string intervalName = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_FirstDayOfPlanYear;

            // Act
            List<DateTime> result = EligibilityCalculatorUtil.CalculateIntervalDates(beginDate, endDate, intervalName);

            // Assert
            CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2023, 5, 15),
            }, result);
        }


    }

}
