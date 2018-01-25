using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fb2Helper.Logic
{
    public static class XDocumentHelpers
    {
        public static XElement ElementByLocal(this XContainer container, string localName)
        {
            return container.ElementsByLocal(localName).FirstOrDefault();
        }

        public static IEnumerable<XElement> ElementsByLocal(this XContainer container, string localName)
        {
            return container.Elements().Where(x => x.Name.LocalName == localName);
        }

        public static IEnumerable<XElement> DescendantsByLocal(this XContainer container, string localName)
        {
            return container.Descendants().Where(x => x.Name.LocalName == localName);
        }

        public static XAttribute AttributeByLocal(this XElement element, string localName)
        {
            return element.AttributesByLocal(localName).FirstOrDefault();
        }

        public static IEnumerable<XAttribute> AttributesByLocal(this XElement element, string localName)
        {
            return element.Attributes().Where(x => x.Name.LocalName == localName);
        }

        public static XElement CreateElement(string name, string namespaceName)
        {
            XName xName = XName.Get(name, namespaceName);
            return new XElement(xName);
        }

        public static XElement CreateElement(string name, string namespaceName, object content)
        {
            XElement xElement = CreateElement(name, namespaceName);
            xElement.Add(content);
            return xElement;
        }

        public static XText GetSequenceChild(this XElement parent, List<string> names)
        {
            if (parent.Name.LocalName != names.First())
            {
                return null;
            }

            XElement current = parent;
            foreach (string name in names.Skip(1))
            {
                current = current?.FirstNode as XElement;
                if (current?.Name.LocalName != name)
                {
                    return null;
                }
            }

            if (current?.FirstNode.NextNode != null)
            {
                return null;
            }

            return current?.FirstNode as XText;
        }

        public static IEnumerable<XElement> FindElementSequences(this XContainer container, List<string> names)
        {
            foreach (XElement parent in container.Elements())
            {
                XText child = parent.GetSequenceChild(names);
                if (child != null)
                {
                    yield return parent;
                }
            }
        }
    }
}
