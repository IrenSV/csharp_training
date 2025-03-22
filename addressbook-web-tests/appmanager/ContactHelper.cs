using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;



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
            InitContactModification(v - 1);
            FillContactForm(newData);
            SubmitContactModification();
            ReturnToMainPage();
            return this;
        }
        public ContactHelper Modify(ContactData contact, ContactData newData)
        {
            InitContactModification(contact.Id);
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
        public ContactHelper Remove(ContactData contact)
        {
            SelectContact(contact.Id);
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
            driver.FindElement(By.Name("submit")).Click();
            contactCash = null;
            return this;
        }
        public ContactHelper ReturnToMainPage()
        {
            driver.FindElement(By.LinkText("home")).Click();
            return this;
        }
        public ContactHelper SelectContact(int index)
        {
            driver.FindElement(By.XPath("//div[@id='content']/form/table/tbody/tr/td[" + (index + 1) + "]/input")).Click();
            return this;
        }
        public ContactHelper SelectContact(String id)
        {
            driver.FindElement(By.XPath("(//input[@name='selected[]' and @value='" + id + "'])")).Click();
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
            driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"))[7].FindElement(By.TagName("a")).Click();
            return this;
        }
        public ContactHelper InitContactModification(String id)
        {
            driver.FindElement(By.XPath("//a[@href='edit.php?id=" + id + "']")).Click();
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

        public ContactData GetContactInformationFromTable(int index)
        {
            manager.Navigator.GoToHomePage();
            IList<IWebElement> cells = driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"));
            string lastName = cells[1].Text;
            string firstName = cells[2].Text;
            string address = cells[3].Text;
            string allEmails = cells[4].Text;
            string allPhones = cells[5].Text;

            return new ContactData(firstName, lastName)
            {
                Address = address,
                AllEmails = allEmails,
                AllPhones = allPhones
            };
        }

        public ContactData GetContactInformationFromEditForm(int index)
        {
            manager.Navigator.GoToHomePage();
            InitContactModification(0);
            string firstName = driver.FindElement(By.Name("firstname")).GetAttribute("value");
            string lastName = driver.FindElement(By.Name("lastname")).GetAttribute("value");
            string address = driver.FindElement(By.Name("address")).GetAttribute("value");
            string homePhone = driver.FindElement(By.Name("home")).GetAttribute("value");
            string mobilePhone = driver.FindElement(By.Name("mobile")).GetAttribute("value");
            string workPhone = driver.FindElement(By.Name("work")).GetAttribute("value");
            string fax = driver.FindElement(By.Name("fax")).GetAttribute("value");
            string email = driver.FindElement(By.Name("email")).GetAttribute("value");
            string email2 = driver.FindElement(By.Name("email2")).GetAttribute("value");
            string email3 = driver.FindElement(By.Name("email3")).GetAttribute("value");
            
            return new ContactData(firstName, lastName)
            {
                Address = address,
                HomePhone = homePhone,
                MobilePhone = mobilePhone,
                WorkPhone = workPhone,
                Fax = fax,
                Email = email,
                Email2 = email2,
                Email3 = email3
            };
        }
        public int GetNumberOfSearchResults()
        {
            manager.Navigator.GoToHomePage();
            string text = driver.FindElement(By.TagName("label")).Text;
            Match m = new Regex(@"\d+").Match(text);
            return Int32.Parse(m.Value);
        }

        public string GetContactInformationFromDetails(int index)
        {
            manager.Navigator.GoToHomePage();
            driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"))[6].
                FindElement(By.TagName("a")).Click();

            string allInfoToContact = driver.FindElement(By.CssSelector("div#content")).Text;
            return allInfoToContact.Replace("H: ", "").Replace("M: ", "").Replace("W: ", "").Replace("F: ", "");
        }

        public void AddContactToGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.GoToHomePage();
            ClearGroupFilter();
            SelectContactToAdd(contact.Id);
            SelectGroupToAdd(group.Name);
            CommitAddingContactToGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count > 0);
        }
        private void ClearGroupFilter()
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText("[all]");
        }

        private void SelectContactToAdd(string contactId)
        {
            driver.FindElement(By.Id(contactId)).Click();
        }

        private void SelectGroupToAdd(string name)
        {
            new SelectElement(driver.FindElement(By.Name("to_group"))).SelectByText(name);
        }

        private void CommitAddingContactToGroup()
        {
            driver.FindElement(By.Name("add")).Click();
        }

        public void RemovalContactFromGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.GoToHomePage();
            SelectGroupToRemoval(group.Name);
            SelectContactToRemoval(contact.Id);
            CommitRemovalContactFromGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count > 0);
        }

        private void SelectGroupToRemoval(string name)
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText(name);
        }

        private void SelectContactToRemoval(string contactId)
        {
            driver.FindElement(By.Id(contactId)).Click();
        }

        private void CommitRemovalContactFromGroup()
        {
            driver.FindElement(By.Name("remove")).Click();
        }
    }
}
