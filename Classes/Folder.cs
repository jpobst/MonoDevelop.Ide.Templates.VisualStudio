// 
// Folder.cs
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
	public class Folder<T> where T : ProjectItem, new ()
	{
		public string Name { get; private set; }
		public string TargetFolderName { get; private set; }
		public List<Folder<T>> Folders { get; private set; }
		public List<T> ProjectItems { get; private set; }

		private Folder ()
		{
		}

		internal static Folder<T> Parse (XElement xe)
		{
			var folder = new Folder<T> ();

			folder.Folders = new List<Folder<T>> ();
			folder.ProjectItems = new List<T> ();

			folder.Name = xe.GetAttributeString ("Name");
			folder.TargetFolderName = xe.GetAttributeString ("TargetFolderName");

			foreach (var elem in xe.Elements (Utilities.ns + "Folder"))
				folder.Folders.Add (Folder<T>.Parse (elem));

			foreach (var elem in xe.Elements (Utilities.ns + "ProjectItem")) {
				var item = new T ();
				item.Parse (elem);
				folder.ProjectItems.Add (item);
			}

			return folder;
		}

		internal static Folder<T> Load (XElement xe, ZipFile zip)
		{
			var folder = new Folder<T> ();

			folder.Folders = new List<Folder<T>> ();
			folder.ProjectItems = new List<T> ();

			folder.Name = xe.GetAttributeString ("Name");
			folder.TargetFolderName = xe.GetAttributeString ("TargetFolderName");

			foreach (var elem in xe.Elements (Utilities.ns + "Folder"))
				folder.Folders.Add (Folder<T>.Load (elem, zip));

			foreach (var elem in xe.Elements (Utilities.ns + "ProjectItem")) {
				var item = new T ();
				item.Load (elem, zip);
				folder.ProjectItems.Add (item);
			}

			return folder;
		}

		internal void Export (string directory, TemplateParameters parameters)
		{
			var this_folder = Path.Combine (directory, GetFinalFolderName (parameters));

			Directory.CreateDirectory (this_folder);

			foreach (var item in ProjectItems)
				item.Export (this_folder, parameters);

			foreach (var folder in Folders)
				folder.Export (this_folder, parameters);
		}

		internal string GetFinalFolderName (TemplateParameters parameters)
		{
			// If no TargetFolderName is specified, use Name
			if (string.IsNullOrWhiteSpace (TargetFolderName))
				return Name;

			// Use TargetFolderName
			return parameters.Replace (TargetFolderName);
		}
	}
}
