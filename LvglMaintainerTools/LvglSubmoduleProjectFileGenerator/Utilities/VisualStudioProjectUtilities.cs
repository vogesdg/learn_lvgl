﻿using System.Xml;

namespace LvglSubmoduleProjectFileGenerator
{
    public class VisualStudioProjectUtilities
    {
        private static string DefaultNamespaceString =
            @"http://schemas.microsoft.com/developer/msbuild/2003";

        private static XmlElement CreateFilterElement(
            XmlDocument Document,
            string Name)
        {
            XmlElement Element = Document.CreateElement(
                "Filter",
                DefaultNamespaceString);
            Element.InnerText = Name;
            return Element;
        }

        private static XmlElement CreateItemElement(
            XmlDocument Document,
            string Type,
            string Target)
        {
            XmlElement Element = Document.CreateElement(
                Type,
                DefaultNamespaceString);
            Element.SetAttribute(
                "Include",
                Target);
            return Element;
        }

        public static XmlDocument CreateCppSharedProject(
            Guid ProjectGuid,
            List<(string Target, string Filter)> HeaderNames,
            List<(string Target, string Filter)> SourceNames,
            List<(string Target, string Filter)> OtherNames)
        {
            XmlDocument Document = new XmlDocument();

            Document.InsertBefore(
                Document.CreateXmlDeclaration("1.0", "utf-8", null),
                Document.DocumentElement);

            XmlElement Project = Document.CreateElement(
                "Project",
                DefaultNamespaceString);
            Project.SetAttribute(
                "ToolsVersion",
                "4.0");

            XmlElement ItemsProjectGuid = Document.CreateElement(
                "ItemsProjectGuid",
                DefaultNamespaceString);
            ItemsProjectGuid.InnerText =
                string.Format("{{{0}}}", ProjectGuid);
            XmlElement GlobalPropertyGroup = Document.CreateElement(
                "PropertyGroup",
                DefaultNamespaceString);
            GlobalPropertyGroup.SetAttribute(
                "Label",
                "Globals");
            GlobalPropertyGroup.AppendChild(ItemsProjectGuid);
            Project.AppendChild(GlobalPropertyGroup);

            XmlElement HeaderItems = Document.CreateElement(
                "ItemGroup",
                DefaultNamespaceString);
            foreach (var Name in HeaderNames)
            {
                XmlElement Item = CreateItemElement(
                    Document,
                    "ClInclude",
                    Name.Target);
                HeaderItems.AppendChild(Item);
            }
            Project.AppendChild(HeaderItems);

            XmlElement SourceItems = Document.CreateElement(
               "ItemGroup",
               DefaultNamespaceString);
            foreach (var Name in SourceNames)
            {
                XmlElement Item = CreateItemElement(
                    Document,
                    "ClCompile",
                    Name.Target);
                SourceItems.AppendChild(Item);
            }
            Project.AppendChild(SourceItems);

            XmlElement OtherItems = Document.CreateElement(
               "ItemGroup",
               DefaultNamespaceString);
            foreach (var Name in OtherNames)
            {
                XmlElement Item = CreateItemElement(
                    Document,
                    "None",
                    Name.Target);
                OtherItems.AppendChild(Item);
            }
            Project.AppendChild(OtherItems);

            Document.AppendChild(Project);

            return Document;
        }

        public static XmlDocument CreateCppSharedFilters(
            List<string> FilterNames,
            List<(string Target, string Filter)> HeaderNames,
            List<(string Target, string Filter)> SourceNames,
            List<(string Target, string Filter)> OtherNames)
        {
            XmlDocument Document = new XmlDocument();

            Document.InsertBefore(
                Document.CreateXmlDeclaration("1.0", "utf-8", null),
                Document.DocumentElement);

            XmlElement Project = Document.CreateElement(
                "Project",
                DefaultNamespaceString);
            Project.SetAttribute(
                "ToolsVersion",
                "4.0");

            XmlElement FilterItems = Document.CreateElement(
                "ItemGroup",
                DefaultNamespaceString);
            foreach (var FilterName in FilterNames)
            {
                XmlElement FilterItem = Document.CreateElement(
                    "Filter",
                    DefaultNamespaceString);
                if (FilterItem != null)
                {
                    FilterItem.SetAttribute(
                        "Include",
                        FilterName);
                    XmlElement UniqueIdentifier = Document.CreateElement(
                        "UniqueIdentifier",
                        DefaultNamespaceString);
                    if (UniqueIdentifier != null)
                    {
                        UniqueIdentifier.InnerText =
                            string.Format("{{{0}}}", Guid.NewGuid());
                        FilterItem.AppendChild(UniqueIdentifier);
                    }
                    FilterItems.AppendChild(FilterItem);
                }
            }
            Project.AppendChild(FilterItems);

            XmlElement HeaderItems = Document.CreateElement(
                "ItemGroup",
                DefaultNamespaceString);
            foreach (var Name in HeaderNames)
            {
                XmlElement Item = CreateItemElement(
                    Document,
                    "ClInclude",
                    Name.Target);
                Item.AppendChild(CreateFilterElement(
                    Document,
                    Name.Filter));
                HeaderItems.AppendChild(Item);
            }
            Project.AppendChild(HeaderItems);

            XmlElement SourceItems = Document.CreateElement(
               "ItemGroup",
               DefaultNamespaceString);
            foreach (var Name in SourceNames)
            {
                XmlElement Item = CreateItemElement(
                    Document,
                    "ClCompile",
                    Name.Target);
                Item.AppendChild(CreateFilterElement(
                    Document,
                    Name.Filter));
                SourceItems.AppendChild(Item);
            }
            Project.AppendChild(SourceItems);

            XmlElement OtherItems = Document.CreateElement(
               "ItemGroup",
               DefaultNamespaceString);
            foreach (var Name in OtherNames)
            {
                XmlElement Item = CreateItemElement(
                    Document,
                    "None",
                    Name.Target);
                Item.AppendChild(CreateFilterElement(
                    Document,
                    Name.Filter));
                OtherItems.AppendChild(Item);
            }
            Project.AppendChild(OtherItems);

            Document.AppendChild(Project);

            return Document;
        }
    }
}
