using EligibilityCalculation;
using EligibilityCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityCalcUtilTest
{
    public class EligibilityCalculatorTest
    {
        [TestMethod("Test Calc")]
        public void TestCalc()
        {
            // Arrange

            var context=new EligibilityCalcContext();
            context.CalculationDate = new DateTime(2023, 1, 1);
            context.UserId = 1;
            context.PartCalcService = new PartCalcService();
            context.PlanSpecService = new PlanSpecService();
            context.PlanSpecEligibilityDetailDto = new PlanSpecEligibilityDetailDto
            {
                PlanSourceId = 1,
                 AgeRequirementYears = 18,
                 AgeRequrementMonths = 0,
                 PlanEntryDateCode = EligibilityCalculation.Utils.Constants.Elig_PlanEntryDateCode_Immediate,
                 PlanEntryTimeCode=EligibilityCalculation.Utils.Constants.Elig_PLanEntryTimeCode_NotApplicable,
                  AgeWaiverYN =false,
                   CalculationMethodCode=EligibilityCalculation.Utils.Constants.Elig_Calc_Method_ElapsedTime,
                    ComputationMethodCode=EligibilityCalculation.Utils.Constants.Elig_Computation_Method_PlanYear,
                     ServiceRequirementPeriodCode=EligibilityCalculation.Utils.Constants.Elig_Service_Requirment_PeriodCode_Months,
                      ServiceRequirementPeriodUnits=3,
                       ServiceWaiverYN=false,
                        SourceCategoryCode="EE",
                         ExcludedEEs=new List<int> {},





            };
            context.ParticipantDetailDto = new ParticipantDetailDto
            {
                ParticipantId = 1,
                FullName = "Test",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            context.PlanYear = new PlanYear
            {
                PlanYearID = 1,
                PlanID = 1,
                PlanYearStartDate = new DateTime(2023, 1, 1),
                PlanYearEndDate = new DateTime(2023, 12, 31)
            };
           
            // Act
            var eligibilityCalculator = new EligibilityCalculator(context);

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
    }
}
