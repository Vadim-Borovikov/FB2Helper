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

            List<string> ids = GetBinaryIds(fb2).ToList();
            List<string> orderedIds = ids.OrderBy(x => x).ToList();
            CollectionAssert.AreNotEqual(ids, orderedIds);

            DataManager.OrderBinaries(fb2);
            ids = GetBinaryIds(fb2).ToList();
            CollectionAssert.AreEqual(ids, orderedIds);
        }

        private static IEnumerable<string> GetBinaryIds(XDocument fb2)
        {
            if (fb2.Root?.Nodes() == null)
            {
                yield break;
            }

            foreach (XElement element in fb2.Root?.Nodes().Cast<XElement>()
                                                          .Where(x => x.Name.LocalName == "binary"))
            {
                yield return element.Attributes().FirstOrDefault(a => a.Name.LocalName == "id")?.Value;
            }
        }

        private const string InputPath = @"D:\Test\fb2.fb2";
    }
}