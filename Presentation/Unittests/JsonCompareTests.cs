using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;

using NUnit.Framework;

using ViLabels.JsonSynchronize;
using ViLabels.Objects;

namespace ViLabelsTests.JsonSynchronize
{
    [TestFixture]
    public class JsonCompareTests
    {
        [Test]
        public void CompareCheckNoMissing()
        {   
            CurrentProject.SetProjectPath(@"C:/project/");
            List<string> testAllLanguages = new List<string> { "en" };
            CurrentProject.SetAllLanguage(testAllLanguages);

            MockFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                                                                   {
                { Path.Combine(CurrentProject.Path, testAllLanguages[0], "global.json"), new MockFileData("{'home':'town'}") },
                { Path.Combine(CurrentProject.Path, "Meta", "global.json"), new MockFileData("{'home':'otherstuf'}") },
                { Path.Combine(CurrentProject.Path, "Meta", "Default", "global.json"), new MockFileData("{'home':''}") }
                });

            JsonCompare jsonCompare = new JsonCompare(mockFileSystem);
            bool checkMissing = jsonCompare.Compare();
            if (!checkMissing)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CompareCheckMissingLabels()
        {
            CurrentProject.SetProjectPath(@"C:/project/");
            List<string> testAllLanguages = new List<string> { "en" };
            CurrentProject.SetAllLanguage(testAllLanguages);

            MockFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                                                                   {
                { Path.Combine(CurrentProject.Path, testAllLanguages[0], "global.json"), new MockFileData("{'home':'town', 'city':'fun'}") },
                { Path.Combine(CurrentProject.Path, "Meta", "global.json"), new MockFileData("{'home':'otherstuf'}") },
                { Path.Combine(CurrentProject.Path, "Meta", "Default", "global.json"), new MockFileData("{'home':''}") }
                });

            JsonCompare jsonCompare = new JsonCompare(mockFileSystem);
            bool checkMissing = jsonCompare.Compare();
            if (checkMissing)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CompareCheckMissingModules()
        {
            CurrentProject.SetProjectPath(@"C:/project/");
            List<string> testAllLanguages = new List<string> { "en" };
            CurrentProject.SetAllLanguage(testAllLanguages);

            MockFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                                                                   {
                { Path.Combine(CurrentProject.Path, testAllLanguages[0], "global.json"), new MockFileData("{'home':'town'}") },
                { Path.Combine(CurrentProject.Path, testAllLanguages[0], "home.json"), new MockFileData("{'living':'fun'}") },
                { Path.Combine(CurrentProject.Path, "Meta", "global.json"), new MockFileData("{'home':'otherstuf'}") },
                { Path.Combine(CurrentProject.Path, "Meta", "Default", "global.json"), new MockFileData("{'home':''}") }
                });

            JsonCompare jsonCompare = new JsonCompare(mockFileSystem);
            bool checkMissing = jsonCompare.Compare();
            if (checkMissing)
            {
                Assert.Fail();
            }
        }
    }
}