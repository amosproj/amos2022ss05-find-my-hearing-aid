using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FindMyBLEDevice.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {

            // Interactive mode: useful for setting up tests.
            // app.Repl();

            // Search ("query") for UI-Elements via their "AutomationId". 
            // AppResult[] results = app.WaitForElement(c => c.Marked("Btn_LearnMore"));
            AppResult[] results = app.Query(c => c.Marked("Btn_LearnMore"));
            Assert.IsTrue(results.Any());

            // Mimick user actions
            app.Tap(c => c.Marked("Btn_LearnMore"));

            // More asserts...

            
        }
    }
}
