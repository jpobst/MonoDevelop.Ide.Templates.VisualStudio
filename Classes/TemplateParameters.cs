// 
// TemplateParameters.cs
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

namespace MonoDevelop.Ide.Templates.VisualStudio.Classes
{
	public class TemplateParameters
	{
		public Dictionary<string, string> CustomParameters { get; private set; }

		public string ClrVersion { get { return Get ("$clrversion$"); } set { CustomParameters["$clrversion$"] = value; } }
		public string Guid1 { get { return Get ("$guid1$"); } set { CustomParameters["$guid1$"] = value; } }
		public string Guid2 { get { return Get ("$guid2$"); } set { CustomParameters["$guid2$"] = value; } }
		public string Guid3 { get { return Get ("$guid3$"); } set { CustomParameters["$guid3$"] = value; } }
		public string Guid4 { get { return Get ("$guid4$"); } set { CustomParameters["$guid4$"] = value; } }
		public string Guid5 { get { return Get ("$guid5$"); } set { CustomParameters["$guid5$"] = value; } }
		public string Guid6 { get { return Get ("$guid6$"); } set { CustomParameters["$guid6$"] = value; } }
		public string Guid7 { get { return Get ("$guid7$"); } set { CustomParameters["$guid7$"] = value; } }
		public string Guid8 { get { return Get ("$guid8$"); } set { CustomParameters["$guid8$"] = value; } }
		public string Guid9 { get { return Get ("$guid9$"); } set { CustomParameters["$guid9$"] = value; } }
		public string ItemName { get { return Get ("$itemname$"); } set { CustomParameters["$itemname$"] = value; } }
		public string MachineName { get { return Get ("$machinename$"); } set { CustomParameters["$machinename$"] = value; } }
		public string ProjectName { get { return Get ("$projectname$"); } set { CustomParameters["$projectname$"] = value; } }
		public string RegisteredOrganization { get { return Get ("$resgisteredorganization$"); } set { CustomParameters["$resgisteredorganization$"] = value; } }
		public string RootNamespace { get { return Get ("$rootnamespace$"); } set { CustomParameters["$rootnamespace$"] = value; } }
		public string Time { get { return Get ("$time$"); } set { CustomParameters["$time$"] = value; } }
		public string UserDomain { get { return Get ("$userdomain$"); } set { CustomParameters["$userdomain$"] = value; } }
		public string UserName { get { return Get ("$username$"); } set { CustomParameters["$username$"] = value; } }
		public string WebNamespace { get { return Get ("$webnamespace$"); } set { CustomParameters["$webnamespace$"] = value; } }
		public string Year { get { return Get ("$year$"); } set { CustomParameters["$year$"] = value; } }

		public string SafeItemName { get { return Path.GetFileNameWithoutExtension (ItemName); } }
		public string SafeProjectName { get { return Path.GetFileNameWithoutExtension (ProjectName); } }

		// We don't want users to be able to create parameters without specifying
		// the parameters needs for project or item templates
		internal TemplateParameters ()
		{
			CustomParameters = new Dictionary<string,string> ();

			// Set up some default parameters
			Guid1 = Guid.NewGuid ().ToString ();
			Guid2 = Guid.NewGuid ().ToString ();
			Guid3 = Guid.NewGuid ().ToString ();
			Guid4 = Guid.NewGuid ().ToString ();
			Guid5 = Guid.NewGuid ().ToString ();
			Guid6 = Guid.NewGuid ().ToString ();
			Guid7 = Guid.NewGuid ().ToString ();
			Guid8 = Guid.NewGuid ().ToString ();
			Guid9 = Guid.NewGuid ().ToString ();

			Time = DateTime.Now.ToLongDateString ();
			Year = DateTime.Now.Year.ToString ();

			MachineName = Environment.MachineName;
			UserDomain = Environment.UserDomainName;
			UserName = Environment.UserName;

			RegisteredOrganization = string.Empty;
		}

		private string Get (string name)
		{
			if (CustomParameters.ContainsKey (name))
				return CustomParameters[name];

			return null;
		}

		internal string Replace (string input)
		{
			foreach (var pair in CustomParameters)
				input = input.Replace (pair.Key, pair.Value);

			return input;
		}
	}
}
