using System.IO;

namespace Fb2Helper.Logic
{
    public static class DataManager
    {
        public static string Process(string path)
        {
            return File.ReadAllText(path);
        }

        public static void Save(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
