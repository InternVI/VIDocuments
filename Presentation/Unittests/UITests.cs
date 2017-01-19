using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.WindowsAPI;

using ViLabels.Backend;
using ViLabels.Objects;

namespace ViLabelsTests
{
    [TestFixture]
    public class UITests
    {
        private Application application;
        private string applicationDirectory;

        [Test]
        [Ignore("Ignore this test. It is an UI test")]
        public void UITest()
        {
            OpenWinAddProject();
            SaveNewProject();
            CreateNewLanguage("EN");

            Window showLabels = this.application.GetWindow("Show labels - BigTestFun");
            OpenAddLanguage(showLabels);

            AddFastLabels(showLabels);
            AddSlowLabel();
            EditLabel();

            var tbSearch = showLabels.Get<TextBox>("TB_SearchQuery");
            tbSearch.Text = "newlabel";
            tbSearch.Text = string.Empty;

            var bSortLabelName = showLabels.Get<Button>("B_Name");
            bSortLabelName.Click();
            bSortLabelName.Click();

            MenuBar mEdit = showLabels.Get<MenuBar>("projectMenu");
            var bAddSubject = mEdit.MenuItem("Edit").ChildMenus[4];
            bAddSubject.Click();

            CreateNewModule();
            RemoveLabels(showLabels);
            SelectFilter(showLabels);

            MenuBar mProject = showLabels.Get<MenuBar>("projectMenu");
            var bBackProject = mProject.MenuItem("File").ChildMenus.First();
            bBackProject.Click();
        }

        private void PreperationTest()
        {
            this.applicationDirectory = TestContext.CurrentContext.TestDirectory;
            var applicationPath = Path.Combine(this.applicationDirectory, "ViLabels.exe");
            this.application = Application.Launch(applicationPath);
            Directory.CreateDirectory(Path.Combine(this.applicationDirectory, "projects", "qwertyuioplkjhgfdsazxcvbnm"));
        }

        private void OpenWinAddProject()
        {
            Window controlProjects = application.GetWindow("Select label project", InitializeOption.NoCache);
            Button bNewProject = controlProjects.Get<Button>("B_GoToAddProject");
            bNewProject.Click();
        }

        private void SaveNewProject()
        {
            Window newProjects = application.GetWindow("Add Project");
            TextBox tbProjectName = newProjects.Get<TextBox>("TB_ProjectName");
            tbProjectName.Text = "BigTestFun";
            TextBox tbProjectPath = newProjects.Get<TextBox>("TB_ProjectPath");
            tbProjectPath.Text = @"projects/qwertyuioplkjhgfdsazxcvbnm";
            Button bSaveProjectPath = newProjects.Get<Button>("B_SaveProject");
            bSaveProjectPath.Click();
        }

        private void CreateNewLanguage(string language)
        {
            Window addNewLanguage = application.GetWindow("Create new language");
            TextBox tbNewLanguage = addNewLanguage.Get<TextBox>("TB_Language");
            tbNewLanguage.Text = language;
            Button bNewLanguage = addNewLanguage.Get<Button>("B_SaveLanguage");
            bNewLanguage.Click();
        }

        private void AddFastLabels(Window showLabels)
        {
            Button bSubject = showLabels.Get<Button>("B_SelectModule");
            bSubject.Click();
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            for (int i = 0; i < 10; i++)
            {
                TextBox tbLabelName = showLabels.Get<TextBox>("TB_LabelName");
                tbLabelName.Text = "value" + i;

                Button bAddFast = showLabels.Get<Button>("B_AddFast");
                bAddFast.Click();
            }
        }

        private void AddSlowLabel()
        {
            Window showLabels = application.GetWindow("Show labels - BigTestFun");

            var bAdd = showLabels.Get<Button>("B_Add");
            bAdd.Click();

            Window winAddLabel = this.application.GetWindow("Add label or parent");
            var listBoxParents = winAddLabel.Get<ListBox>("LB_TreeLabels");
            listBoxParents.Select(0);

            var labelName = winAddLabel.Get<TextBox>("TB_LabelName");
            labelName.Text = "newlabel";

            var bSaveLabel = winAddLabel.Get<Button>("B_SaveLabel");
            bSaveLabel.Click();
        }

        private void EditLabel()
        {
            Window winEditLabel = this.application.GetWindow("Edit Label");
            var tbTranslation = winEditLabel.Get<TextBox>("EN");
            tbTranslation.Text = "Translation";

            var bSaveChangesLabel = winEditLabel.Get<Button>("B_editButton");
            bSaveChangesLabel.Click();
        }

        private void OpenAddLanguage(Window showLabels)
        {
            MenuBar mEdit = showLabels.Get<MenuBar>("projectMenu");
            var bAddSubject = mEdit.MenuItem("Edit").ChildMenus[3];
            bAddSubject.Click();

            CreateNewLanguage("FR");
            var bLanguage = showLabels.Get<Button>("FR");
            bLanguage.Click();
        }

        private void CreateNewModule()
        {
            Window addSubject = this.application.GetWindow("Create new Subject");
            TextBox tbSubject = addSubject.Get<TextBox>("TB_Module");
            tbSubject.Text = "newSubject";
            Button bSaveSubject = addSubject.Get<Button>("B_SaveModule");
            bSaveSubject.Click();
        }

        private void SelectFilter(Window showLabels)
        {
            // filtert op onderwerp, markdown en dubbele labels
            var bFilter = showLabels.Get<Button>("B_SelectModules");

            bFilter.Click();
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            bFilter.Click();
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            bFilter.Click();
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.UP);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.UP);
            showLabels.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
        }

        private void RemoveLabels(Window showLabels)
        {
            ListBox lbAllLabels = showLabels.Get<ListBox>("LB_AllLabels");
            for (int i = 0; i < 2; i++)
                {
                    lbAllLabels.Select((i + 1) * 2);
                    Button bDeleteLabel = showLabels.Get<Button>("B_Delete");
                    bDeleteLabel.Click();

                    var messageBox = showLabels.MessageBox("Delete Confirmation");
                    messageBox.Get<Button>(SearchCriteria.ByText("Yes")).Click();
                }
         }

        private void ClearProject()
        {
            Window controlProjects = application.GetWindow("Select label project");
            ListBox lbAllProjects = controlProjects.Get<ListBox>("LB_AllSavedProjects");

            int indexToSelect = FindMyStringInList(lbAllProjects, "BigTestFun", 0);
            lbAllProjects.Select(indexToSelect);

            Button bDeleteProject = controlProjects.Get<Button>("B_DeleteProjectReference");
            bDeleteProject.Click();

            application.Close();

            ClearFolder(Path.Combine(applicationDirectory, "projects"));
        }

        private int FindMyStringInList(ListBox lb, string searchString, int startIndex)
        {
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                string lbString = lb.Items[i].ToString();
                if (lbString.Contains(searchString))
                {
                    return i;
                }
            }
            return -1;
        }

        private void ClearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }

        [SetUp]
        public void Init()
        {
            PreperationTest();
        }

        [TearDown]
        public void Dispose()
        {
            ClearProject();
        }
    }
}
