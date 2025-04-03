using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace mantis_tests
{
    [TestFixture]
    public class ProjectCreationTests : AuthTestBase
    {
        [Test]
        public void ProjectCreationTest()
        {
            ProjectData project = new ProjectData("Проект1");

            List<ProjectData> oldProjects = app.Projects.GetProjectList();
            //AccountData existingProjects = project.Find(x => x.Name == project.Name);
            //if (existingProjects != null)
            //{
            //    app.Projects.Remove(existingProjects);
            //}

            app.Projects.Create(project);

            Assert.AreEqual(oldProjects.Count + 1, app.Projects.GetProjectCount());

            List<ProjectData> newProjects = app.Projects.GetProjectList();
            oldProjects.Add(project);
            oldProjects.Sort();
            newProjects.Sort();
            Assert.AreEqual(oldProjects, newProjects);
        }
    }
}
