using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestWebSelenium
{
    [TestClass]
    public class UnitTest2
    {
        public static IWebDriver driver;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://cursomcsd2015.esy.es/");
        }

        [ClassCleanup]
        public static void Teardown()
        {
            driver.Quit();
        }


        [TestMethod]
        public void Links()
        {
            IList<IWebElement> links = driver.FindElements(By.TagName("a")).ToList();

            bool validacion = true;

            foreach (var item in links)
            {
                try
                {
                    HttpWebRequest httReq = (HttpWebRequest)WebRequest.Create(item.GetAttribute("href"));
                    HttpWebResponse httpRes = (HttpWebResponse)httReq.GetResponse();
                    var stC = httpRes.StatusCode;
                    Debug.WriteLine("--> " + item.Text + " / " + item.GetAttribute("href") + " / " + stC + " / " + (Int32)stC);
                    httpRes.Close();
                }
                catch (Exception e)
                {
                    validacion = false;
                    Debug.WriteLine("--> " + item.Text + " / " + item.GetAttribute("href") + " / " + e.Message);
                }
            }
            Assert.IsTrue(validacion);
        }
    }
}
