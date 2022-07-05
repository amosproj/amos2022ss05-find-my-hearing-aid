// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Dominik Pysch <domi.pysch@gmail.com>
// SPDX-FileCopyrightText: 2022 Marib Aldoais <marib.aldoais@gmail.com>

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


        /*
            
            Important sources:
                https://docs.microsoft.com/en-us/appcenter/test-cloud/frameworks/uitest/xamarin-forms?tabs=windows
                https://docs.microsoft.com/en-us/appcenter/test-cloud/frameworks/uitest/
                
            Important Notes: 
                Mind that you always have to have installed the latest version of the app!
                Running tests does NOT push the app to the running emulator, it justs starts the existing version.
                Set the following system environment-variables:
                    ANDROID_HOME: <your path to your android sdk> (e.g. "C:\Program Files (x86)\Android\android-sdk")
                    JAVA_HOME: <your path to your jdk> (e.g. "C:\Program Files\Java\jdk-17.0.2")
                Set "AutomationId" on the UI-elements in .xaml-files when developing tests
                
            Some examples:

            Interactive mode: useful for setting up tests.
            `app.Repl();`

            Search ("query") (until it shows up, in case of animations etc.) for UI-Elements via their "AutomationId". 
            `AppResult[] results = app.WaitForElement(c => c.Marked("Page_About"));`

            Search ("query") for UI-Elements via their "AutomationId". 
            `appResult[] results2 = app.Query(c => c.Marked("Page_About"));`
            `Assert.IsTrue(results.Any());`

            Mimick user actions
            `app.Tap(c => c.Marked("Btn_ID"));`

         */

        [Test]
        public void AppLaunch()
        {

            // Assert "About"-Screen is shown on app launch
            AppResult[] results = app.Query(c => c.Marked("Page_About"));
            Assert.IsTrue(results.Any());

        }
        
    }
}
