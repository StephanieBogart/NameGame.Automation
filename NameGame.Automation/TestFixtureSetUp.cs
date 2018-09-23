using NUnit.Framework;

namespace NameGame.Automation
{

    [SetUpFixture]
    public class TestFixtureSetUp
    {
        public static string NameGame = "http://www.ericrochester.com/name-game/";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Browser.LaunchAndGoToURL(NameGame);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Browser.Quit();
        }
    }
}