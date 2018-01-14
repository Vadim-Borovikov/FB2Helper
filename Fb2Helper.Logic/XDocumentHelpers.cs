using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fb2Helper.Logic
{
    public static class XDocumentHelpers
    {
        public static XElement ElementByLocal(this XContainer xContainer, string localName)
        {
            return xContainer.ElementsByLocal(localName).FirstOrDefault();
        }

        public static IEnumerable<XElement> ElementsByLocal(this XContainer xContainer, string localName)
        {
            return xContainer.Elements().Where(x => x.Name.LocalName == localName);
        }

        public static XAttribute AttributeByLocal(this XElement xElement, string localName)
        {
            return xElement.AttributesByLocal(localName).FirstOrDefault();
        }

        public static IEnumerable<XAttribute> AttributesByLocal(this XElement xElement, string localName)
        {
            return xElement.Attributes().Where(x => x.Name.LocalName == localName);
        }
    }
}
