using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace mantis_tests
{
    public class ProjectManagementHelper : HelperBase
    {
        public ProjectManagementHelper(ApplicationManager manager) : base(manager) { }
        public ProjectManagementHelper Create(ProjectData project)
        {
            OpenPageProjectManagement();
            ProjectCreation();
            FillProjectForm(project);
            SubmitProjectCreation();
            return this;
        }
        public ProjectManagementHelper Remove(int v)
        {
            OpenPageProjectManagement();
            SelectProject(v);
            RemoveProject();
            SubmitRemoveProject();
            return this;
        }
        private ProjectManagementHelper OpenPageProjectManagement()
        {
            driver.FindElement(By.LinkText("Управление")).Click();
            driver.FindElement(By.LinkText("Проекты")).Click();
            return this;
        }
        private ProjectManagementHelper ProjectCreation()
        {
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            return this;
        }

        private ProjectManagementHelper FillProjectForm(ProjectData project)
        {
            driver.FindElement(By.Id("project-name")).Click();
            driver.FindElement(By.Id("project-name")).Clear();
            driver.FindElement(By.Id("project-name")).SendKeys(project.Name);
            return this;
        }
        private ProjectManagementHelper SubmitProjectCreation()
        {
            driver.FindElement(By.XPath("//input[@value='Добавить проект']")).Click();
            projectCash = null;
            return this;
        }
        public void APICreate(AccountData account, ProjectData project)
        {
            Mantis.MantisConnectPortTypeClient client = new Mantis.MantisConnectPortTypeClient();
            Mantis.ProjectData projectData = new Mantis.ProjectData();
            projectData.name = project.Name;
            client.mc_project_add(account.Name, account.Password, projectData);
        }

        public ProjectManagementHelper CheckProjects()
        {
            Mantis.MantisConnectPortTypeClient client = new Mantis.MantisConnectPortTypeClient();
            var projects = client.mc_projects_get_user_accessible("administrator", "root");
            if (projects == null || projects.Length == 0)
            {
                AccountData accountName = new AccountData("administrator", "root");
                ProjectData project = new ProjectData("Новый проект");
                APICreate(accountName, project);
            }
            return this;
        }
        private ProjectManagementHelper SelectProject(int index)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//div[@id='main-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div/table/tbody/tr[" + index + "]/td/a")).Click();
            return this;
        }
        private ProjectManagementHelper RemoveProject()
        {
            driver.FindElement(By.XPath("//form[@id='manage-proj-update-form']/div/div[3]/button[2]")).Click();
            return this;
        }
        private ProjectManagementHelper SubmitRemoveProject()
        {
            driver.FindElement(By.XPath("//input[@value='Удалить проект']")).Click();
            projectCash = null;
            return this;
        }
        private List<ProjectData> projectCash = null;
        public List<ProjectData> GetProjectList()
        {
            if (projectCash == null)
            {
                projectCash = new List<ProjectData>();
                OpenPageProjectManagement();
                ICollection<IWebElement> elements = driver.FindElements(By.XPath("//div[@id='main-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div[2]/table/tbody/tr"));
                foreach (IWebElement element in elements)
                {
                    String collectName = element.FindElement(By.XPath("td[1]/a")).Text;

                    projectCash.Add(new ProjectData(collectName));
                }
            }
            return new List<ProjectData>(projectCash);
        }
        public int GetProjectCount()
        {
            return driver.FindElements(By.XPath("//div[@id='main-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div[2]/table/tbody/tr")).Count;
        }

        //public List<ProjectData> GetProjectListAPI()
        //{
        //    var projectsList = new List<ProjectData>();
        //    Mantis.MantisConnectPortTypeClient client = new Mantis.MantisConnectPortTypeClient();
        //    var projects = client.mc_projects_get_user_accessible("administrator", "root");
        //    foreach (var project in projects)
        //    {
        //        string projectName = project.name;

        //        projectsList.Add(new ProjectData(projectName));
        //    }
        //    return new List<ProjectData>(projectsList);
        //}
        ////public ProjectManagementHelper CheackNameProject(ProjectData project)
        //{
        //    List<ProjectData> projectsNameApi = GetProjectListAPI();
        //    ProjectData existingProject = projectsNameApi.Find(x => x.Name == project.Name);
        //    if (existingProject != null)
        //    {
        //        OpenPageProjectManagement();
        //        driver.FindElement(By.CssSelector(project.Name)).Click();
        //    }
        //    return this;
        //}
    }
}
