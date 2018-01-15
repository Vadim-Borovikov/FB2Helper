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

        private static List<string> GetBinaryIds(XDocument fb2)
        {
            return fb2.Root?.ElementsByLocal("binary")
                            .Select(e => e.AttributeByLocal("id").Value)
                            .ToList();
        }

        private const string InputPath = @"D:\Test\fb2.fb2";

        private const string BookAuthorFirstNameInitial = "1";
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