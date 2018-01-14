using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fb2Helper.Logic.Tests
{
    [TestClass]
    public class DataManagerTests
    {
        [TestMethod]
        public void OrderBinariesTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            List<string> ids = GetBinaryIds(fb2);
            List<string> orderedIds = ids?.OrderBy(x => x).ToList();
            CollectionAssert.AreNotEqual(ids, orderedIds);

            DataManager.OrderBinaries(fb2);
            ids = GetBinaryIds(fb2);
            CollectionAssert.AreEqual(ids, orderedIds);
        }

        private static List<string> GetBinaryIds(XDocument fb2)
        {
            return fb2.Root?.ElementsByLocal("binary")
                            .Select(e => e.AttributeByLocal("id").Value)
                            .ToList();
        }

        private const string InputPath = @"D:\Test\fb2.fb2";
    }
}