using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using S22.Imap;

namespace TestWebSelenium
{
    [TestClass]
    public class UnitTest1
    {

        public static IWebDriver driver;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://cursomcsd2015.esy.es/contact.html");
        }

        [ClassCleanup]
        public static void Teardown()
        {
            driver.Close();
        }


        private string nombre = "Juan";

        [TestMethod]
        public void Email()
        {
            try
            {
                IWebElement name = driver.FindElement(By.Id("name"));
                name.SendKeys(nombre);

                IWebElement email = driver.FindElement(By.Id("email"));
                email.SendKeys("juan@juan.com");

                IWebElement subject = driver.FindElement(By.Id("subject"));
                subject.SendKeys("Selenium");

                IWebElement text = driver.FindElement(By.Id("text"));
                text.SendKeys("Envio de mail con pruebas unitarias y selenium");

                IWebElement btn = driver.FindElement(By.Id("submit"));
                btn.Click();

                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();

                Assert.AreEqual("Curso MCSD 2015 - Free Bootstrap Theme", driver.Title);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        [TestMethod]
        public void VerificarMail()
        {
            bool encontado = false;
            string hostname = "imap.gmail.com", username = "cursomcsd2015@gmail.com", password = "Password";
            using (ImapClient client = new ImapClient(hostname, 993, username, password, AuthMethod.Login, true))
            {
                IEnumerable<uint> uids = client.Search(SearchCondition.All());
                IEnumerable<MailMessage> messages = client.GetMessages(uids);

                string subject = "Formulario de Contacto - " + nombre;

                MailMessage aux = messages.First(o => o.Subject == subject);

                if (messages != null)
                {
                    encontado = true;
                }
            }
            Assert.IsTrue(encontado);
        }

    }
}
