using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace mantis_tests
{
    [TestFixture]
    public class ProjectRemovalTests : AuthTestBase
    {
        [Test]
        public void ProjectRemovalTest()
        {
            app.Projects.CheckProjects();

            List<ProjectData> oldProjects = app.Projects.GetProjectListAPI();

            app.Projects.Remove(1);

            Assert.AreEqual(oldProjects.Count - 1, app.Projects.GetProjectCount());

            List<ProjectData> newProjects = app.Projects.GetProjectListAPI();
            oldProjects.RemoveAt(0);
            oldProjects.Sort();
            newProjects.Sort();
            Assert.AreEqual(oldProjects, newProjects);
        }
    }
}
