using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;


namespace WebAddressbookTests
{
    public class ContactHelper : HelperBase
    {
        public ContactHelper(ApplicationManager manager) : base(manager) { }

        public ContactHelper Create(ContactData contact)
        {
            InitNewContactCreation();
            FillContactForm(contact);
            SubmitContactCreation();
            ReturnToMainPage();
            return this;
        }
        public ContactHelper Check()
        {
            CheckContacts();
            return this;
        }
        public ContactHelper Modify(int v, ContactData newData)
        {
            InitContactModification(v);
            FillContactForm(newData);
            SubmitContactModification();
            ReturnToMainPage();
            return this;
        }
        public ContactHelper Remove(int v)
        {
            SelectContact(v);
            RemoveContact();
            return this;
        }
        public ContactHelper InitNewContactCreation()
        {
            driver.FindElement(By.LinkText("add new")).Click();
            return this;
        }
        public ContactHelper FillContactForm(ContactData contact)
        {
            Type(By.Name("firstname"), contact.Firstname);
            Type(By.Name("lastname"), contact.Lastname);
            return this;
        }
        public ContactHelper SubmitContactCreation()
        {
            driver.FindElement(By.Name("update")).Click();
            contactCash = null;
            return this;
        }
        public ContactHelper ReturnToMainPage()
        {
            driver.FindElement(By.LinkText("home page")).Click();
            return this;
        }
        public ContactHelper SelectContact(int index)
        {
            driver.FindElement(By.XPath("//div[@id='content']/form/table/tbody/tr/td[" + (index + 1) + "]/input")).Click();
            return this;
        }
        public ContactHelper CheckContacts()
        {
            if (OpenContactList())
            {
                ContactData contact = new ContactData("Имя", "Фамилия");
                Create(contact);
            }
            return this;
        }
        public bool OpenContactList()
        {
            return !IsElementPresent(By.Name("selected[]"));
        }

        public ContactHelper RemoveContact()
        {
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            driver.FindElement(By.LinkText("home")).Click();
            contactCash = null;
            return this;
        }
        public ContactHelper InitContactModification(int index)
        {
            driver.FindElement(By.XPath("//table[@id='maintable']/tbody/tr[" + (index + 1) +"]/td[8]/a/img")).Click(); 
            return this;
        }
        public ContactHelper SubmitContactModification()
        {
            driver.FindElement(By.Name("update")).Click();
            contactCash = null;
            return this;
        }
        private List<ContactData> contactCash = null;
        public List<ContactData> GetContactList()
        {
            if (contactCash == null)
            {
                contactCash = new List<ContactData>();
                ICollection<IWebElement> elements = driver.FindElements(By.XPath("//*[@id=\"maintable\"]/tbody/tr[@name=\"entry\"]"));
                foreach (IWebElement element in elements)
                {
                    String collectLastname = element.FindElement(By.XPath("td[2]")).Text;
                    String collectFirstname = element.FindElement(By.XPath("td[3]")).Text;

                    contactCash.Add(new ContactData(collectFirstname, collectLastname));
                }
            }
            return new List<ContactData>(contactCash);
        }

        public int GetContactCount()
        {
            return driver.FindElements(By.XPath("//*[@id=\"maintable\"]/tbody/tr[@name=\"entry\"]")).Count;
        }
    }
}
