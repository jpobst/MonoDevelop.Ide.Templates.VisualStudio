// 
// VSTemplate.cs
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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ionic.Zip;

namespace MonoDevelop.Ide.Templates.VisualStudio.Classes
{
	public class VSTemplate<T> where T : ProjectItem, new ()
	{
		public TemplateType Type { get; set; }
		public string Version { get; set; }

		public TemplateData Data { get; private set; }
		public TemplateContent<T> Content { get; internal set; }

		public void Export (string directory, TemplateParameters parameters)
		{
			Content.Export (directory, parameters);
		}

		protected void ParseOrLoad (string filename, out ZipFile zipfile, out XDocument doc)
		{
			if (string.IsNullOrWhiteSpace (filename))
				throw new ArgumentNullException ();

			// Find the .vstemplate, and read it as xml
			zipfile = ZipFile.Read (filename);
			var template = FindVsTemplate (zipfile);
			doc = GetXDocumentFromZip (template);

			Version = doc.Root.Attribute ("Version").Value;
			Type = GetTemplateType (doc);
			Data = TemplateData.Parse (doc);
		}

		private static ZipEntry FindVsTemplate (ZipFile zip)
		{
			return zip.Entries.Where (z => Path.GetExtension (z.FileName) == ".vstemplate").FirstOrDefault ();
		}

		private static XDocument GetXDocumentFromZip (ZipEntry file)
		{
			using (var stream = file.OpenReader ())
				using (var sr = new StreamReader (stream))
					return XDocument.Parse (sr.ReadToEnd ());
		}

		private static TemplateType GetTemplateType (XDocument doc)
		{
			switch (doc.Root.Attribute ("Type").Value) {
				case "Project":
					return TemplateType.Project;
				case "Item":
					return TemplateType.Item;
			}

			throw new ArgumentOutOfRangeException ();
		}
	}
}
