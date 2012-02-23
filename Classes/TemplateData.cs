// 
// TemplateData.cs
//  
// Author:
//       Jonathan Pobst <jpobst@xamarin.com>
// 
// Copyright (c) 2012 Xamarin Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Xml.Linq;

namespace MonoDevelop.Ide.Templates.VisualStudio.Classes
{
	public class TemplateData
	{
		private static XNamespace ns = "http://schemas.microsoft.com/developer/vstemplate/2005";

		public string Name { get; private set; }
		public string Description { get; private set; }
		public string Icon { get; private set; }
		public string ProjectType { get; private set; }
		public string ProjectSubType { get; private set; }
		public string TemplateID { get; private set; }
		public string TemplateGroupID { get; private set; }
		public int SortOrder { get; private set; }
		public bool CreateNewFolder { get; private set; }
		public string DefaultName { get; private set; }
		public bool ProvideDefaultName { get; private set; }
		public bool PromptForSaveOnCreation { get; private set; }
		public bool EnableLocationBrowseButton { get; private set; }
		public bool Hidden { get; private set; }
		public int NumberOfParentCategoriesToRollUp { get; private set; }
		public string LocationFieldMRUPrefix { get; private set; }
		public string LocationField { get; private set; }
		public string RequiredFrameworkVersion { get; private set; }
		public bool SupportsMasterPage { get; private set; }
		public bool SupportsCodeSeparation { get; private set; }
		public bool SupportsLanguageDropDown { get; private set; }

		private TemplateData ()
		{
		}

		internal static TemplateData Parse (XDocument doc)
		{
			var data = new TemplateData ();

			data.Name = GetTemplateDataString (doc, "Name");
			data.Description = GetTemplateDataString (doc, "Description");
			data.Icon = GetTemplateDataString (doc, "Icon");
			data.ProjectType = GetTemplateDataString (doc, "ProjectType");
			data.ProjectSubType = GetTemplateDataString (doc, "ProjectSubType");
			data.TemplateID = GetTemplateDataString (doc, "TemplateID");
			data.TemplateGroupID = GetTemplateDataString (doc, "TemplateGroupID");
			data.SortOrder = GetTemplateDataInteger (doc, "SortOrder", 100);
			data.CreateNewFolder = GetTemplateDataBoolean (doc, "CreateNewFolder", true);
			data.DefaultName = GetTemplateDataString (doc, "DefaultName");
			data.ProvideDefaultName = GetTemplateDataBoolean (doc, "ProvideDefaultName", true);
			data.PromptForSaveOnCreation = GetTemplateDataBoolean (doc, "PromptForSaveOnCreation", false);
			data.EnableLocationBrowseButton = GetTemplateDataBoolean (doc, "EnableLocationBrowseButton", true);
			data.Hidden = GetTemplateDataBoolean (doc, "Hidden", false);
			data.NumberOfParentCategoriesToRollUp = GetTemplateDataInteger (doc, "NumberOfParentCategoriesToRollUp", 0);
			data.LocationFieldMRUPrefix = GetTemplateDataString (doc, "LocationFieldMRUPrefix");
			data.LocationField = GetTemplateDataString (doc, "LocationField");
			data.RequiredFrameworkVersion = GetTemplateDataString (doc, "RequiredFrameworkVersion");
			data.SupportsMasterPage = GetTemplateDataBoolean (doc, "SupportsMasterPage", false);
			data.SupportsCodeSeparation = GetTemplateDataBoolean (doc, "SupportsCodeSeparation", false);
			data.SupportsLanguageDropDown = GetTemplateDataBoolean (doc, "SupportsLanguageDropDown", false);

			return data;
		}

		private static string GetTemplateDataString (XDocument doc, string name)
		{
			var element = doc.Root.Element (ns + "TemplateData").Element (ns + name);

			if (element != null)
				return element.Value;

			return null;
		}

		private static int GetTemplateDataInteger (XDocument doc, string name, int defaultValue)
		{
			var element = doc.Root.Element (ns + "TemplateData").Element (ns + name);

			if (element != null)
				return int.Parse (element.Value);

			return defaultValue;
		}

		private static bool GetTemplateDataBoolean (XDocument doc, string name, bool defaultValue)
		{
			var element = doc.Root.Element (ns + "TemplateData").Element (ns + name);

			if (element != null)
				return bool.Parse (element.Value);

			return defaultValue;
		}
	}
}
