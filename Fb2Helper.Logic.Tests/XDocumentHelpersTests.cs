using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fb2Helper.Logic.Tests
{
    [TestClass]
    public class XDocumentHelpersTests
    {
        [TestMethod]
        public void ElementByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            XElement binary = fb2.Root?.Elements().FirstOrDefault(x => x.Name.LocalName == "binary");
            XElement binaryByLocal = fb2.Root?.ElementByLocal("binary");

            Assert.AreEqual(binary, binaryByLocal);
        }

        [TestMethod]
        public void ElementsByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            List<XElement> binaries = fb2.Root?.Elements().Where(x => x.Name.LocalName == "binary").ToList();
            List<XElement> binariesByLocal = fb2.Root?.ElementsByLocal("binary").ToList();

            CollectionAssert.AreEqual(binaries, binariesByLocal);
        }

        [TestMethod]
        public void DescendantsByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            List<XElement> binaries = fb2.Root?.Descendants().Where(x => x.Name.LocalName == "binary").ToList();
            List<XElement> binariesByLocal = fb2.Root?.DescendantsByLocal("binary").ToList();

            CollectionAssert.AreEqual(binaries, binariesByLocal);
        }

        [TestMethod]
        public void AttributeByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement binary = fb2.Root?.ElementByLocal("binary");

            XAttribute id = binary?.Attributes().FirstOrDefault(x => x.Name.LocalName == "id");
            XAttribute idLocal = binary?.AttributeByLocal("id");

            Assert.AreEqual(id, idLocal);
        }

        [TestMethod]
        public void AttributesByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement binary = fb2.Root?.ElementByLocal("binary");

            List<XAttribute> ids = binary?.Attributes().Where(x => x.Name.LocalName == "id").ToList();
            List<XAttribute> idsByLocal = binary.AttributesByLocal("id").ToList();

            CollectionAssert.AreEqual(ids, idsByLocal);
        }

        [TestMethod]
        public void CreateElementTest()
        {
            const string Name = "name";
            const string NamepaceName = "namespaceName";
            XElement element = XDocumentHelpers.CreateElement("name", "namespaceName");
            CheckName(element, Name, NamepaceName);
        }

        [TestMethod]
        public void CreateElementWithContentTest()
        {
            const string Name = "name";
            const string NamepaceName = "namespaceName";
            var content = new XText("content");
            XElement element = XDocumentHelpers.CreateElement("name", "namespaceName", content);
            CheckName(element, Name, NamepaceName);
            List<XNode> nodes = element.Nodes().ToList();
            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(content, nodes.Single());
        }

        [TestMethod]
        public void GetSequenceChildTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            var names = new List<string>
            {
                "description",
                "title-info",
                "genre"
            };

            XText genre = fb2.Root?.ElementByLocal(names.First()).GetSequenceChild(names);
            Assert.IsNotNull(genre);
            Assert.AreEqual("antique", genre.Value);
        }

        [TestMethod]
        public void FindElementSequencesTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement body = fb2.Root?.ElementByLocal("body");

            var names = new List<string>
            {
                "p",
                "strong",
                "emphasis"
            };

            var paragraphs = new List<XElement>();
            foreach (XElement section in body.ElementsByLocal("section"))
            {
                List<XElement> paragraphsInSection = section.FindElementSequences(names).ToList();
                paragraphs.AddRange(paragraphsInSection);
            }

            Assert.AreEqual(ParagrahsAmount, paragraphs.Count);
        }

        private static void CheckName(XElement element, string name, string namespaceName)
        {
            Assert.IsNotNull(element);
            Assert.AreEqual(name, element.Name.LocalName);
            Assert.AreEqual(namespaceName, element.Name.NamespaceName);
        }

        private const string InputPath = @"D:\Test\fb2.fb2";
        private const int ParagrahsAmount = 71;
    }
}