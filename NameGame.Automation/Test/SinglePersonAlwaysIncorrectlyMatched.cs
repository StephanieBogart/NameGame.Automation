using NameGame.Automation.Helpers;
using NameGame.Automation.PageObjects;
using NUnit.Framework;
using System.Collections.Generic;

namespace NameGame.Automation.Test
{
    [TestFixture]
    public class SinglePersonAlwaysIncorrectlyMatched : TestBase
    {
        // Write a test to verify that failing to select one person's name correctly makes that person appear more frequently tan other "correctly selected" people
        [Test]
        public void IncorrectMatchShouldShowUpMoreOften()
        {
            string EmployeeNameToNeverMatch = HomePage.GetEmployeeNameToMatch();
            Logging.Log($"Employee to not Match: {EmployeeNameToNeverMatch}");

            Dictionary<string, int> EmployeesAndNumberOfTimesSeen = new Dictionary<string, int>();

            EmployeesAndNumberOfTimesSeen = HomePage.AlwaysFailOnTargetEmployeeAndSucceedOnOthers(EmployeeNameToNeverMatch, EmployeesAndNumberOfTimesSeen);

            HomePage.VerifyEmployeeNeverToMatchIsSeenMoreOftenThanOtherCorrectlyChosenEmployees(EmployeeNameToNeverMatch, EmployeesAndNumberOfTimesSeen);
        }


    }
}
