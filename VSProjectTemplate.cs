// 
// VSProjectTemplate.cs
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
using Ionic.Zip;
using MonoDevelop.Ide.Templates.VisualStudio.Classes;

namespace MonoDevelop.Ide.Templates.VisualStudio
{
	public class VSProjectTemplate : VSTemplate<ProjectTemplateProjectItem>
	{
		// No one should be able to create this without static methods
		private VSProjectTemplate ()
		{
		}

		/// <summary>
		/// Returns a template instance with just metadata.  Cannot be used to
		/// create a new item.  This is for displaying information in a dialog
		/// chooser, so it is lightweight.  Use Load () for a full template.
		/// </summary>
		public static VSProjectTemplate Parse (string filename)
		{
			ZipFile zipfile;
			XDocument doc;

			// Get the metadata for the template
			var template = new VSProjectTemplate ();
			template.ParseOrLoad (filename, out zipfile, out doc);

			// Get a metadata only copy of <TemplateContent>
			template.Content = TemplateContent<ProjectTemplateProjectItem>.Parse (doc.Root.Element (Utilities.ns + "TemplateContent"));

			// Release the zipfile
			if (zipfile != null)
				zipfile.Dispose ();

			return template;
		}

		/// <summary>
		/// Returns a full template instance that can be used to create
		/// a new templated item.  Use this instead of Parse () when the user
		/// has chosen which template item they want to use.
		/// </summary>
		public static VSProjectTemplate Load (string filename)
		{
			ZipFile zipfile;
			XDocument doc;

			// Get the metadata for the template
			var template = new VSProjectTemplate ();
			template.ParseOrLoad (filename, out zipfile, out doc);

			// Get a full copy of <TemplateContent>
			template.Content = TemplateContent<ProjectTemplateProjectItem>.Load (doc.Root.Element (Utilities.ns + "TemplateContent"), zipfile);

			// Release the zipfile
			if (zipfile != null)
				zipfile.Dispose ();

			return template;
		}

		public TemplateParameters CreateTemplateParameters (string clrVersion, string projectName)
		{
			var tp = new TemplateParameters ();

			tp.ClrVersion = clrVersion;
			tp.ProjectName = projectName;

			tp.CustomParameters["$safeprojectname$"] = tp.SafeProjectName;

			foreach (var cp in Content.CustomParameters)
				tp.CustomParameters[cp.Name] = cp.Value;

			return tp;
		}
	}
}
