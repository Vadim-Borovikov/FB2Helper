using System;
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
        public void GetDescriutionTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            BookDescription description = fb2.GetDescription();
            Assert.AreEqual(BookAuthorFirstNameInitial, description.BookAuthorFirstName);
            Assert.AreEqual(BookAuthorFamilyNameInitial, description.BookAuthorFamilyName);
            Assert.AreEqual(TitleInitial, description.Title);
            Assert.AreEqual(FileAuthorFirstNameInitial, description.FileAuthorFirstName);
            Assert.AreEqual(FileAuthorFamilyNameInitial, description.FileAuthorFamilyName);
            CollectionAssert.AreEqual(_programsUsedInitial, description.ProgramsUsed);
            Assert.AreEqual(_dateInitial, description.Date);
        }

        [TestMethod]
        public void FillDescriutionTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            BookDescription description = fb2.GetDescription();
            description.Fill(BookAuthorFirstName, BookAuthorFamilyName, Title, FileAuthorFirstName,
                             FileAuthorFamilyName, ProgramName);

            Assert.AreEqual(BookAuthorFirstName, description.BookAuthorFirstName);
            Assert.AreEqual(BookAuthorFamilyName, description.BookAuthorFamilyName);
            Assert.AreEqual(Title, description.Title);
            Assert.AreEqual(FileAuthorFirstName, description.FileAuthorFirstName);
            Assert.AreEqual(FileAuthorFamilyName, description.FileAuthorFamilyName);
            CollectionAssert.AreEqual(_programsUsed, description.ProgramsUsed);
            Assert.AreEqual(_date, description.Date);
        }

        [TestMethod]
        public void SetDescriutionTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            BookDescription description = fb2.GetDescription();
            description.Fill(BookAuthorFirstName, BookAuthorFamilyName, Title, FileAuthorFirstName,
                FileAuthorFamilyName, ProgramName);

            fb2.SetDescription(description);
            description = fb2.GetDescription();

            Assert.AreEqual(BookAuthorFirstName, description.BookAuthorFirstName);
            Assert.AreEqual(BookAuthorFamilyName, description.BookAuthorFamilyName);
            Assert.AreEqual(Title, description.Title);
            Assert.AreEqual(FileAuthorFirstName, description.FileAuthorFirstName);
            Assert.AreEqual(FileAuthorFamilyName, description.FileAuthorFamilyName);
            CollectionAssert.AreEqual(_programsUsed, description.ProgramsUsed);
            Assert.AreEqual(_date, description.Date);
        }

        [TestMethod]
        public void FixDashesTest()
        {
            TestReplace(DataManager.FixDashes, new[] { "- ", "– " }, "— ");
        }

        [TestMethod]
        public void FixDotsTest()
        {
            TestReplace(DataManager.FixDots, "...", "…");
        }

        [TestMethod]
        public void FixQuotesTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement bodyElement = fb2.Root?.ElementByLocal("body");
            Assert.IsNotNull(bodyElement);
            string text = bodyElement.Value;

            int oldIndexLeft = text.IndexOf("\"", StringComparison.Ordinal);
            Assert.AreNotEqual(-1, oldIndexLeft);
            int oldIndexRight = text.LastIndexOf("\"", StringComparison.Ordinal);
            Assert.AreNotEqual(-1, oldIndexRight);
            Assert.AreNotEqual(oldIndexLeft, oldIndexRight);

            fb2.FixQuotes();
            text = bodyElement.Value;

            Assert.IsFalse(text.Contains("\""));
            Assert.AreEqual('«', text[oldIndexLeft]);
            Assert.AreEqual('»', text[oldIndexRight]);
        }

        [TestMethod]
        public void FixSymbolsTest()
        {
            TestRemove(DataManager.FixSymbols, new[] { '\u2028', '\u0306' });
        }

        [TestMethod]
        public void SplitToSectionsByNamesTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement bodyElement = fb2.Root?.ElementByLocal("body");
            Assert.IsNotNull(bodyElement);
            int oldSectionsAmount = bodyElement.DescendantsByLocal("section").Count();
            var newSectionNames = new List<string> { "p", "strong", "emphasis" };
            int pseAmount = bodyElement.ElementsByLocal("section").Sum(s => s.FindElementSequences(newSectionNames).Count());

            var titleNames = new List<string> { "title", "p" };
            fb2.SplitToSectionsByElementsNames(newSectionNames, titleNames);
            int newSectionsAmount = bodyElement.DescendantsByLocal("section").Count();
            fb2.Save(OutputPath);
            Assert.IsTrue(newSectionsAmount >= (oldSectionsAmount + pseAmount));
            Assert.IsTrue(newSectionsAmount <= (2 * oldSectionsAmount + pseAmount));
        }

        [TestMethod]
        public void OrderBinariesTest()
        {
            XDocument fb2 = XDocument.Load(InputPath);

            List<string> ids = GetBinaryIds(fb2);
            List<string> orderedIds = ids?.OrderBy(x => x).ToList();
            CollectionAssert.AreNotEqual(orderedIds, ids);

            fb2.OrderBinaries();
            ids = GetBinaryIds(fb2);
            CollectionAssert.AreEqual(orderedIds, ids);
        }

        private static void TestReplace(Action<XDocument> replacer, string oldValue, string newValue)
        {
            TestReplace(replacer, new[] { oldValue }, newValue);
        }

        private static void TestReplace(Action<XDocument> replacer, string[] oldValues, string newValue)
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement bodyElement = fb2.Root?.ElementByLocal("body");
            Assert.IsNotNull(bodyElement);
            string text = bodyElement.ToString();
            text = text.Replace(newValue, oldValues.First());

            HashSet<int> oldIndexes = GetMatchingIndexes(text, oldValues);
            Assert.AreNotEqual(0, oldIndexes.Count);
            int minOldIndex = oldIndexes.Min();
            int newIndex = text.IndexOf(newValue, StringComparison.Ordinal);
            Assert.AreEqual(-1, newIndex);

            replacer(fb2);
            text = bodyElement.ToString();
            oldIndexes = GetMatchingIndexes(text, oldValues);
            Assert.AreEqual(0, oldIndexes.Count);

            newIndex = text.IndexOf(newValue, StringComparison.Ordinal);
            Assert.AreEqual(minOldIndex, newIndex);
        }

        private static void TestRemove(Action<XDocument> remover, char[] chars)
        {
            XDocument fb2 = XDocument.Load(InputPath);
            XElement bodyElement = fb2.Root?.ElementByLocal("body");
            Assert.IsNotNull(bodyElement);
            string text = bodyElement.ToString();

            HashSet<int> oldIndexes = GetMatchingIndexes(text, chars);
            Assert.AreNotEqual(0, oldIndexes.Count);

            remover(fb2);
            text = bodyElement.ToString();
            oldIndexes = GetMatchingIndexes(text, chars);
            Assert.AreEqual(0, oldIndexes.Count);
        }

        private static HashSet<int> GetMatchingIndexes(string text, IEnumerable<string> values)
        {
            IEnumerable<int> indexes = values.Select(ov => text.IndexOf(ov, StringComparison.Ordinal))
                                             .Where(i => i != -1);
            return new HashSet<int>(indexes);
        }

        private static HashSet<int> GetMatchingIndexes(string text, IEnumerable<char> values)
        {
            IEnumerable<int> indexes = values.Select(ov => text.IndexOf(ov))
                                             .Where(i => i != -1);
            return new HashSet<int>(indexes);
        }

        private static List<string> GetBinaryIds(XDocument fb2)
        {
            return fb2.Root?.ElementsByLocal("binary")
                            .Select(e => e.AttributeByLocal("id").Value)
                            .ToList();
        }

        private const string InputPath = @"D:\Test\fb2.fb2";
        private const string OutputPath = @"D:\Test\fb2.result.fb2";

        private const string BookAuthorFirstNameInitial = "";
        private const string BookAuthorFamilyNameInitial = "Unknown";
        private const string TitleInitial = "Unknown";
        private const string FileAuthorFirstNameInitial = "";
        private const string FileAuthorFamilyNameInitial = "Unknown";
        private readonly DateTime _dateInitial = new DateTime(2017, 12, 31);
        private readonly List<string> _programsUsedInitial = new List<string>
        {
            "calibre 2.30.0",
            "FictionBook Editor Release 2.6.7"
        };

        private const string BookAuthorFirstName = "Eliezer";
        private const string BookAuthorFamilyName = "Yudkowsky";
        private const string Title = "Harry Potter and the Methods of Rationality";
        private const string FileAuthorFirstName = "Vadim";
        private const string FileAuthorFamilyName = "Borovikov";
        private readonly DateTime _date = DateTime.Today;
        private readonly List<string> _programsUsed = new List<string>
        {
            "calibre 2.30.0",
            "FictionBook Editor Release 2.6.7",
            ProgramName
        };

        private const string ProgramName = "FB2 Helper";
    }
}