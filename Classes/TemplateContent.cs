// 
// TemplateContent.cs
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
using System.Xml.Linq;
using Ionic.Zip;

namespace MonoDevelop.Ide.Templates.VisualStudio.Classes
{
	public class TemplateContent <T> where T : ProjectItem, new ()
	{
		public List<Project<T>> Projects { get; private set; }
		public List<Reference> References { get; private set; }
		public List<ProjectItem> ProjectItems { get; private set; }
		public List<CustomParameter> CustomParameters { get; private set; }

		private TemplateContent ()
		{
		}

		internal void Export (string directory, TemplateParameters parameters)
		{
			foreach (var proj in Projects)
				proj.Export (directory, parameters);

			foreach (var item in ProjectItems)
				item.Export (directory, parameters);
		}

		internal static TemplateContent<T> Parse (XElement xe)
		{
			var tc = new TemplateContent<T> ();

			tc.Projects = new List<Project<T>> ();
			tc.References = new List<Reference> ();
			tc.ProjectItems = new List<ProjectItem> ();
			tc.CustomParameters = new List<CustomParameter> ();

			foreach (var elem in xe.Elements (Utilities.ns + "Project"))
				tc.Projects.Add (Project<T>.Parse (elem));

			foreach (var elem in xe.Elements (Utilities.ns + "Reference"))
				tc.References.Add (Reference.Parse (elem));

			foreach (var elem in xe.Elements (Utilities.ns + "ProjectItem")) {
				var item = new T ();
				item.Parse (elem);
				tc.ProjectItems.Add (item);
			}

			foreach (var elem in xe.Elements (Utilities.ns + "CustomParameter"))
				tc.CustomParameters.Add (new CustomParameter (elem));

			return tc;
		}

		internal static TemplateContent<T> Load (XElement xe, ZipFile zip)
		{
			var tc = new TemplateContent<T> ();

			tc.Projects = new List<Project<T>> ();
			tc.References = new List<Reference> ();
			tc.ProjectItems = new List<ProjectItem> ();
			tc.CustomParameters = new List<CustomParameter> ();

			foreach (var elem in xe.Elements (Utilities.ns + "Project"))
				tc.Projects.Add (Project<T>.Load (elem, zip));

			foreach (var elem in xe.Elements (Utilities.ns + "Reference"))
				tc.References.Add (Reference.Parse (elem));

			foreach (var elem in xe.Elements (Utilities.ns + "ProjectItem")) {
				var item = new T ();
				item.Load (elem, zip);
				tc.ProjectItems.Add (item);
			}

			foreach (var elem in xe.Elements (Utilities.ns + "CustomParameter"))
				tc.CustomParameters.Add (new CustomParameter (elem));

			return tc;
		}
	}
}
