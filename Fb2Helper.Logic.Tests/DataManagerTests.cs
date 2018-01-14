using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fb2Helper.Logic.Tests
{
    [TestClass]
    public class DataManagerTests
    {
        [TestMethod]
        public void ProcessTest()
        {
            string content = DataManager.Process(InputPath);
            Assert.IsNotNull(content);
            Assert.AreNotEqual(0, content.Length);
        }

        [TestMethod]
        public void SaveTest()
        {
            File.Delete(OutputPath);

            string content = DataManager.Process(InputPath);

            Assert.IsFalse(File.Exists(OutputPath));

            DataManager.Save(OutputPath, content);
            Assert.IsTrue(File.Exists(OutputPath));
            content = File.ReadAllText(OutputPath);
            Assert.IsNotNull(content);
            Assert.AreNotEqual(0, content.Length);
        }

        private const string InputPath = @"D:\Test\fb2.fb2";
        private const string OutputPath = @"D:\Test\fb2.result.fb2";
    }
}