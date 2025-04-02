﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            //CheckNameProject(project);
            ProjectCreation();
            FillProjectForm(project);
            SubmitProjectCreation();
            return this;
        }

        //private ProjectManagementHelper CheckNameProject(ProjectData project)
        //{
        //    if (OpenNameList())
        //    {
        //        if (OpenNameList(project))
        //        {
        //            Remove();
        //        }
        //    }
        //    return this;
        //}
        //private bool OpenNameList()
        //{
        //    return IsElementPresent(By.XPath("//div[@id='main-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div/table/tbody/tr"));
        //}
        //private bool OpenNameList(ProjectData project)
        //{
        //    return OpenNameList() && GetProjectName() == project.Name;
        //}
        //private string GetProjectName()
        //{
        //    string text = driver.FindElement(By.XPath("//div[@id='main-container']/div[2]/div[2]/div/div/div[2]/div[2]/div/div/table/tbody/tr/td/a")).Text;
        //    return text;
        //}
        public ProjectManagementHelper Remove(int v)
        {
            OpenPageProjectManagement();
            CheckProjects();
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
            return this;
        }
        public void APICreate(AccountData account, ProjectData project)
        {
            Mantis.MantisConnectPortTypeClient client = new Mantis.MantisConnectPortTypeClient();
            Mantis.ProjectData projectData = new Mantis.ProjectData();
            projectData.name = project.Name;
            client.mc_project_add(account.Name, account.Password, projectData);
        }

        private ProjectManagementHelper CheckProjects()
        {
            OpenProjectList();
            driver.FindElement(By.LinkText("Проекты")).Click();
            return this;
        }
        private void OpenProjectList()
        {
            Mantis.MantisConnectPortTypeClient client = new Mantis.MantisConnectPortTypeClient();
            var projects = client.mc_projects_get_user_accessible("administrator", "root");
            if (projects == null || projects.Length == 0)
            {
                AccountData accountName = new AccountData("administrator", "root");
                ProjectData project = new ProjectData("Новый проект");
                APICreate(accountName, project);
            }
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
            return this;
        }
    }
}
