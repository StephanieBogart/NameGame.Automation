using NUnit.Framework;
using System;
using System.Threading;
using System.Collections.Generic;
using NameGame.Automation.PageObjects;

namespace NameGame.Automation.Test
{
    [TestFixture]
    class SingleCorrectAnswerIsSelected : TestBase
    {
        // Verify the streak counter is incrementing on correct selections
        [Test]
        public void StreakCounterIsIncremented()
        {
            int StartingStreakCounter = CountersPage.GetCounterValue(CounterType.Streak);

            HomePage.PerformSuccessfulMatchSpecifiedNumberOfTimes(1);

            Assert.AreEqual(StartingStreakCounter + 1, CountersPage.GetCounterValue(CounterType.Streak));
        }

        // Verify name and displayed photos change after selecting the correct answer
        [Test]
        public void NameToMatchAndDisplayedPhotosChange()
        {
            var StartingDisplayName = HomePage.GetEmployeeNameToMatch();

            List<string> StartingDisplayedPhotos = new List<string>(HomePage.GetDisplayedEmployeeChoices());
            HomePage.ClickOnImageByDesiredOutcome(MatchType.Match);

            Thread.Sleep(TimeSpan.FromSeconds(6)); // Inefficient.  Need to find a better way to detect if the dynamic elements of the page has reloaded.

            var EndingDisplayName = HomePage.GetEmployeeNameToMatch();
            List<string> EndingDisplayedPhotos = new List<string>(HomePage.GetDisplayedEmployeeChoices());

            Assert.AreNotEqual(StartingDisplayName, Home.EmployeeNameToMatch.Text);
            CollectionAssert.AreNotEqual(StartingDisplayedPhotos, EndingDisplayedPhotos);
        }
    }
}
