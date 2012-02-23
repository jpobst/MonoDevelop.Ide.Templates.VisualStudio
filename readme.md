About
========

MonoDevelop.Ide.Templates.VisualStudio is a library for parsing and creating
new projects and items from Visual Studio templates:

http://msdn.microsoft.com/en-us/library/6db0hwky.aspx

Usage
==================

There are 2 possible operations this library can do:

* Parsing Templates

Parsing templates is designed to be quick and lightweight, ideal for populating
a "Add New Project/Item" dialog box.  This only provides the included metadata
about the template, and cannot be used to create a template.

	// Parsing a Project template
	var template_file = @"C:\MyTemplate.zip";
	var template = VSProjectTemplate.Parse (template_file);
  
	Console.WriteLine (template.Data.Name);
	Console.WriteLine ("- " + template.Data.Description);

* Creating new Projects/Items

Once a template has been chosen, it can be Loaded so a new project or item
can be created from it.

	// Creating an Item from a template
	var template_file = @"C:\MyItemTemplate.zip";
	var output_dir = @"C:\ItemOutput";
	var template = VSItemTemplate.Load (template_file);
  
	var parameters = template.CreateTemplateParameters ("4.0.0", "MyItem", "MyItem.Namespace");
  
	template.Export (output_dir, parameters);