using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fb2Helper.Logic
{
    public static class DataManager
    {
        public static void Process(XDocument fb2)
        {
            OrderBinaries(fb2);
        }

        public static void OrderBinaries(XDocument fb2)
        {
            List<XElement> orderedBinaries = fb2.Root?.Nodes().Cast<XElement>()
                                                              .Where(x => x.IsBinary())
                                                              .OrderBy(x => x.GetBinaryId())
                                                              .ToList();
            if (orderedBinaries == null)
            {
                return;
            }

            foreach (XElement binary in orderedBinaries)
            {
                binary.Remove();
                fb2.Root?.Add(binary);
            }
        }

        private static bool IsBinary(this XElement element) { return element.Name.LocalName == "binary"; }
        private static string GetBinaryId(this XElement element)
        {
            return element.Attributes().FirstOrDefault(a => a.Name.LocalName == "id")?.Value;
        }
    }
}
