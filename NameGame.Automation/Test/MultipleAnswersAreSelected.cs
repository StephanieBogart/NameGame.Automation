using NameGame.Automation.Helpers;
using NameGame.Automation.PageObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NameGame.Automation.Test
{
    class MultipleAnswersAreSelected : TestBase
    {
        // Verify the multiple "streak" counter resets after getting an incorrect answer.
        [Test]
        public void StreakCounterResetsOnIncorrectChoice()
        {
            int NumberOfSuccessfulMatches = 2;
            Tools.WaitForElementToLoad(CountersPage.CssSelectorCorrectStreak);
            Logging.Log($"Starting streak counter: {Counters.SuccessStreakCounter.Text}");

            HomePage.PerformSuccessfulMatchSpecifiedNumberOfTimes(NumberOfSuccessfulMatches);
            HomePage.ClickOnImageByDesiredOutcome(MatchType.NonMatch);

            Tools.WaitForElementToLoad(HomePage.CssSelectorPhoto);
            Logging.Log($"Ending streak counter: {Counters.SuccessStreakCounter.Text}");

            Assert.AreEqual(0, Convert.ToInt32(Counters.SuccessStreakCounter.Text));
        }

        // Verify that after 10 random selections the correct counters are being incremented for tries and correct counters
        [Test]
        public void AttemptedAndCorrectCountersIncrementCorrectly()
        {
            Random Random = new Random();

            List<string> NumbersForPhotosThatHaveBeenSelected = new List<string>();

            int TotalTrackedAttempts = Convert.ToInt32(Counters.SuccessStreakCounter.Text);
            int TotalTrackedCorrectAttempts = Convert.ToInt32(Counters.TotalCorrectAttempts.Text);

            while (TotalTrackedAttempts < 10)
            {
                Logging.Log($"Tracked Attempts {TotalTrackedAttempts}.");

                // Generate a number to select a photo on the page
                int RandomGeneratedNumber = Random.Next(0, 5);
                Logging.Log($"Random Number in int {RandomGeneratedNumber.ToString()}");

                if (!NumbersForPhotosThatHaveBeenSelected.Contains(RandomGeneratedNumber.ToString()))
                {
                    NumbersForPhotosThatHaveBeenSelected.Add(RandomGeneratedNumber.ToString());

                    HomePage.ClickOnImageDefinedByImageNumber(RandomGeneratedNumber);

                    string SelectedEmployeeName = HomePage.GetEmployeeNameByImageNumber(RandomGeneratedNumber);

                    // If a match is made, clear the list to start again on next round
                    if (Home.EmployeeNameToMatch.Text == SelectedEmployeeName)
                    {
                        NumbersForPhotosThatHaveBeenSelected.Clear();
                        Logging.Log("List of Photos that have been selected has been cleared.");
                        TotalTrackedCorrectAttempts += 1;
                        TotalTrackedAttempts += 1;
                        Logging.Log($"Total Tracked correct attempts: {TotalTrackedCorrectAttempts}.  Total Attempts: {TotalTrackedAttempts}");

                        // Sleeps of Less than 4 in local tests will result in assertion failures due to page reload
                        Thread.Sleep(TimeSpan.FromSeconds(4));

                        Assert.AreEqual(TotalTrackedCorrectAttempts.ToString(), Counters.TotalCorrectAttempts.Text);
                        Assert.AreEqual(TotalTrackedAttempts.ToString(), Counters.TotalAttempts.Text);
                    }
                    else if (Home.EmployeeNameToMatch.Text != SelectedEmployeeName)
                    {
                        TotalTrackedAttempts += 1;

                        Assert.AreEqual(TotalTrackedCorrectAttempts.ToString(), Counters.TotalCorrectAttempts.Text);
                        Assert.AreEqual(TotalTrackedAttempts.ToString(), Counters.TotalAttempts.Text);
                    }

                }
            }
        }


    }
}
