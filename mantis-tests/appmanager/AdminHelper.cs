using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;

namespace mantis_tests
{
    public class AdminHelper : HelperBase
    {
        private string baseURL;

        public AdminHelper(ApplicationManager manager, String baseURL) : base(manager)
        {
            this.baseURL = baseURL;
        }

        public List<AccountData> GetAllAccounts()
        {
            List<AccountData> accounts = new List<AccountData>();
            IWebDriver driver = OpenAppAndLogin();
            driver.Url = baseURL + "/manage_user_edit_page.php";
            IList<IWebElement> rows = driver.FindElements(By.CssSelector("//table/tr"));
            foreach (IWebElement row in rows)
            {
                IWebElement link = row.FindElement(By.TagName("a"));
                string name = link.Text;
                string href = link.GetAttribute("href");
                Match m = Regex.Match(href, @"\d+$");
                string id = m.Value;

                accounts.Add(new AccountData(name, "")
                {
                    Id = id
                });
            }
            return accounts;
        }
        public void DeleteAccount(AccountData account)
        {
            IWebDriver driver = OpenAppAndLogin();
            driver.Url = baseURL + "/manage_user_edit_page.php?user_id=" + account.Id;
            driver.FindElement(By.CssSelector("input[value='Удалить учетную запись']")).Click();
            driver.FindElement(By.CssSelector("input[value='Удалить учетную запись']")).Click();

        }

        private IWebDriver OpenAppAndLogin()
        {
            driver.Url = baseURL;
            driver.FindElement(By.Name("username")).SendKeys("administrator");
            driver.FindElement(By.Name("password")).SendKeys("root");
            driver.FindElement(By.XPath("//input[@value='Войти']")).Click();
            return driver;
        }
    }
}
