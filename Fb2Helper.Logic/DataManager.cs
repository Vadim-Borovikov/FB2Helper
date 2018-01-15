using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Fb2Helper.Logic
{
    public static class DataManager
    {
        public static BookDescription GetDescription(this XDocument fb2)
        {
            var result = new BookDescription();

            XElement descriptionElement = fb2.Root?.ElementByLocal("description");

            XElement titleElement = descriptionElement.ElementByLocal("title-info");
            result.Title = titleElement.ElementByLocal("book-title").Value;
            XElement bookAuthorElement = titleElement.ElementByLocal("author");
            result.BookAuthorFirstName = bookAuthorElement.ElementByLocal("first-name").Value;
            result.BookAuthorFamilyName = bookAuthorElement.ElementByLocal("last-name").Value;

            XElement documentElement = descriptionElement.ElementByLocal("document-info");
            XElement fileAuthorElement = documentElement.ElementByLocal("author");
            result.FileAuthorFirstName = fileAuthorElement.ElementByLocal("first-name").Value;
            result.FileAuthorFamilyName = fileAuthorElement.ElementByLocal("last-name").Value;

            string programUsed = documentElement.ElementByLocal("program-used").Value;
            string[] programs = programUsed.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            result.ProgramsUsed = new List<string>(programs);

            string date = documentElement.ElementByLocal("date").AttributeByLocal("value").Value;
            result.Date = DateTime.Parse(date);

            return result;
        }

        public static void Fill(this BookDescription description, string bookAuthorFirstName,
                                string bookAuthorFamilyName, string title, string fileAuthorFirstName,
                                string fileAuthorFamilyName, string programName)
        {
            description.BookAuthorFirstName = bookAuthorFirstName;
            description.BookAuthorFamilyName = bookAuthorFamilyName;
            description.Title = title;
            description.FileAuthorFirstName = fileAuthorFirstName;
            description.FileAuthorFamilyName = fileAuthorFamilyName;

            description.ProgramsUsed.Add(programName);

            description.Date = DateTime.Today;
        }

        public static void Process(this XDocument fb2, BookDescription description)
        {
            fb2.SetDescription(description);
            fb2.OrderBinaries();
        }

        public static void SetDescription(this XDocument fb2, BookDescription description)
        {
            XElement descriptionElement = fb2.Root?.ElementByLocal("description");

            XElement titleElement = descriptionElement.ElementByLocal("title-info");
            titleElement.ElementByLocal("book-title").Value = description.Title;
            XElement bookAuthorElement = titleElement.ElementByLocal("author");
            bookAuthorElement.ElementByLocal("first-name").Value = description.BookAuthorFirstName;
            bookAuthorElement.ElementByLocal("last-name").Value = description.BookAuthorFamilyName;

            XElement documentElement = descriptionElement.ElementByLocal("document-info");
            XElement fileAuthorElement = documentElement.ElementByLocal("author");
            fileAuthorElement.ElementByLocal("first-name").Value = description.FileAuthorFirstName;
            fileAuthorElement.ElementByLocal("last-name").Value = description.FileAuthorFamilyName;

            documentElement.ElementByLocal("program-used").Value = string.Join(", ", description.ProgramsUsed);

            XElement dateElement = documentElement.ElementByLocal("date");
            dateElement.AttributeByLocal("value").Value = description.Date.ToString("yyyy-MM-dd");
            dateElement.Value = description.Date.ToString("D");
        }

        public static void OrderBinaries(this XDocument fb2)
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
