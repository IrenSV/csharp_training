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
            InitContactModification();
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
            driver.FindElement(By.XPath("//div[@id='content']/form/input[20]")).Click();
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
        private bool OpenContactList()
        {
            return !IsElementPresent(By.Name("selected[]"));
        }

        private ContactHelper RemoveContact()
        {
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            return this;
        }
        private ContactHelper InitContactModification()
        {
            if (OpenContactList())
            {
                ContactData contact = new ContactData("Имя", "Фамилия");
                Create(contact);
            }
            driver.FindElement(By.XPath("//img[@alt='Edit']")).Click();
            driver.FindElement(By.XPath("//form[@action='edit.php']")).Click();
            return this;
        }
        private ContactHelper SubmitContactModification()
        {
            driver.FindElement(By.Name("update")).Click();
            return this;
        }

        public List<ContactData> GetContactList()
        {
            List<ContactData> contacts = new List<ContactData>();
            ICollection<IWebElement> elements = driver.FindElements(By.Name("entry"));
            foreach (IWebElement element in elements)
            {
                contacts.Add(new ContactData(element.Text, element.Text));
            }
            return contacts;
        }
    }
}
