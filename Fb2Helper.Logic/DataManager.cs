using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fb2Helper.Logic
{
    public static class DataManager
    {
        public static void Process(this XDocument fb2)
        {
            OrderBinaries(fb2);
        }

        public static void OrderBinaries(XDocument fb2)
        {
            List<XElement> orderedBinaries = fb2.Root?.ElementsByLocal("binary")
                                                      .OrderBy(x => x.Attribute("id")?.Value)
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
    }
}
