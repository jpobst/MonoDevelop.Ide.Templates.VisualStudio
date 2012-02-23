// 
// Project.cs
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
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Ionic.Zip;

namespace MonoDevelop.Ide.Templates.VisualStudio.Classes
{
	public class Project<T> where T : ProjectItem, new ()
	{
		public string File { get; private set; }
		public bool ReplaceParameters { get; private set; }
		public string TargetFileName { get; private set; }

		public string ContentText { get; private set; }
		public byte[] ContentBytes { get; private set; }

		public List<Folder<T>> Folders { get; private set; }
		public List<T> ProjectItems { get; private set; }

		private Project ()
		{
		}

		internal void Export (string directory, TemplateParameters parameters)
		{
			var filename = Path.Combine (directory, parameters.ProjectName);

			if (ReplaceParameters)
				System.IO.File.WriteAllText (filename, parameters.Replace (ContentText));
			else
				System.IO.File.WriteAllBytes (filename, ContentBytes);

			foreach (var folder in Folders)
				folder.Export (directory, parameters);

			foreach (var item in ProjectItems)
				item.Export (directory, parameters);
		}

		internal static Project<T> Parse (XElement xe)
		{
			var project = new Project<T> ();

			project.Folders = new List<Folder<T>> ();
			project.ProjectItems = new List<T> ();

			project.File = xe.GetAttributeString ("File");
			project.ReplaceParameters = xe.GetAttributeBoolean ("ReplaceParameters", false);
			project.TargetFileName = xe.GetAttributeString ("TargetFileName");

			foreach (var elem in xe.Elements (Utilities.ns + "Folder"))
				project.Folders.Add (Folder<T>.Parse (elem));

			foreach (var elem in xe.Elements (Utilities.ns + "ProjectItem")) {
				var item = new T ();
				item.Parse (elem);
				project.ProjectItems.Add (item);
			}

			return project;
		}

		internal static Project<T> Load (XElement xe, ZipFile zip)
		{
			var project = new Project<T> ();

			project.Folders = new List<Folder<T>> ();
			project.ProjectItems = new List<T> ();

			project.File = xe.GetAttributeString ("File");
			project.ReplaceParameters = xe.GetAttributeBoolean ("ReplaceParameters", false);
			project.TargetFileName = xe.GetAttributeString ("TargetFileName");

			var zf = zip[project.File];

			if (project.ReplaceParameters) {
				using (var sr = new StreamReader (zf.OpenReader ()))
					project.ContentText = sr.ReadToEnd ();
			} else {
				using (var reader = zf.OpenReader ()) {
					byte[] buffer = new byte[reader.Length];

					reader.Read (buffer, 0, buffer.Length);

					project.ContentBytes = buffer;
				}
			}

			foreach (var elem in xe.Elements (Utilities.ns + "Folder"))
				project.Folders.Add (Folder<T>.Load (elem, zip));

			foreach (var elem in xe.Elements (Utilities.ns + "ProjectItem")) {
				var item = new T ();
				item.Load (elem, zip);
				project.ProjectItems.Add (item);
			}

			return project;
		}
	}
}
