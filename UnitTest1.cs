using System;
using System.IO;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using System.Runtime;
using ImageMagick;

namespace JunitXML
{
    public class Tests
    {
        IWebDriver driver;
        JWriter jw = new JWriter("App 1 Tests", "Section 1 Tests");

        [Test, Order(0)]
        public void TestSetup()
        {
            try
            {
                driver = new ChromeDriver(@"C:\ChromeDriver");

                jw.logTestResult("Driver Setup Test", "pass","Setup Test - pass");
            }
            catch (Exception ee) {
                jw.logTestResult("Driver Setup Test", "failure",  "Setup Test - failed", ee.Message);
                Assert.Fail(ee.Message);
            }
            Assert.Pass();
        }

        [Test, Order(1)]
        public void TestGotoURL()
        {
            try
            {
                driver.Navigate().GoToUrl("https://google.com/");
                System.Threading.Thread.Sleep(9999);                                // 10 secdonds
                jw.logTestResult("Goto URL Test", "pass","URL Test - pass");
            }
            catch (Exception ee)
            {
                jw.logTestResult("Goto URL Test", "failure",  "URL Test - failed", ee.Message);
                Assert.Fail(ee.Message);
            }
            Assert.Pass();
        }


        [Test, Order(101)]
        public void Test_Compare_PNG()
        {
            double difFactor=-1;

            try
            {
                using (driver)
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile("screenshot.png", ScreenshotImageFormat.Png);

                    using (MagickImage rref = new MagickImage("refpng.png"))
                    using (MagickImage sshot = new MagickImage("screenshot.png"))
                    {
                        sshot.Compare(rref, ErrorMetric.RootMeanSquared);
                        difFactor = sshot.Compare(rref, ErrorMetric.MeanErrorPerPixel);
                        if ( difFactor > 0 )
                        {
                            throw new ApplicationException("The images are not exactly the same.  Difference factor = " + difFactor.ToString());
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                jw.logTestResult("PNG Compare", "failure", "PNG Compare 1 fail", ee.Message);
                Assert.Fail (ee.Message);
            }
            jw.logTestResult("PNG Compare", "pass", "PNG Compare Test - pass");
            Assert.Pass();
        }

        [Test, Order(999)]
        public void OneTimeTearDown()
        {
            jw.Close();
            driver.Quit();
        }
    }
}