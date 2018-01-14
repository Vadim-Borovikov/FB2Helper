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

            XElement element = fb2.Root?.Elements().FirstOrDefault(x => x.Name.LocalName == "binary");
            XElement elementByLocal = fb2.Root?.ElementByLocal("binary");

            Assert.AreEqual(element, elementByLocal);
        }

        [TestMethod]
        public void ElementsByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            List<XElement> elements = fb2.Root?.Elements().Where(x => x.Name.LocalName == "binary").ToList();
            List<XElement> elementsByLocal = fb2.Root?.ElementsByLocal("binary").ToList();

            CollectionAssert.AreEqual(elements, elementsByLocal);
        }

        [TestMethod]
        public void AttributeByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement element = fb2.Root?.ElementByLocal("binary");

            XAttribute attribute = element?.Attributes().FirstOrDefault(x => x.Name.LocalName == "id");
            XAttribute attributeByLocal = element?.AttributeByLocal("id");

            Assert.AreEqual(attribute, attributeByLocal);
        }

        [TestMethod]
        public void AttributesByLocalTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement element = fb2.Root?.ElementByLocal("binary");

            List<XAttribute> attributes = element?.Attributes().Where(x => x.Name.LocalName == "id").ToList();
            List<XAttribute> attributesByLocal = element.AttributesByLocal("id").ToList();

            CollectionAssert.AreEqual(attributes, attributesByLocal);
        }

        private const string InputPath = @"D:\Test\fb2.fb2";
    }
}