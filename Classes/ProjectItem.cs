// 
// ProjectItem.cs
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
using System.Xml.Linq;
using Ionic.Zip;

namespace MonoDevelop.Ide.Templates.VisualStudio.Classes
{
	public abstract class ProjectItem
	{
		public string TargetFileName { get; protected set; }
		public bool ReplaceParameters { get; protected set; }
		public string Value { get; set; }

		public string ContentText { get; protected set; }
		public byte[] ContentBytes { get; protected set; }

		internal abstract void Parse (XElement xe);

		internal void Export (string directory, TemplateParameters parameters)
		{
			var filename = Path.Combine (directory, GetFinalItemName (parameters));

			Directory.CreateDirectory (Path.GetDirectoryName (filename));

			if (ReplaceParameters)
				File.WriteAllText (filename, parameters.Replace (ContentText));
			else
				File.WriteAllBytes (filename, ContentBytes);
		}

		internal string GetFinalItemName (TemplateParameters parameters)
		{
			if (string.IsNullOrWhiteSpace (TargetFileName))
				return Value;

			return parameters.Replace (TargetFileName);
		}

		internal void Load (XElement xe, ZipFile zip)
		{
			Parse (xe);

			var zf = zip[Value];

			if (ReplaceParameters) {
				using (var sr = new StreamReader (zf.OpenReader ()))
					ContentText = sr.ReadToEnd ();
			} else {
				using (var reader = zf.OpenReader ()) {
					byte[] buffer = new byte[reader.Length];

					reader.Read (buffer, 0, buffer.Length);

					ContentBytes = buffer;
				}
			}
		}
	}
}
