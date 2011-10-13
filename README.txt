ObjectPrinter is the .Net answer for printing an object graph, similar to print in PHP & python.

with a "using ObjectPrinter;" statement, you can use the DumpToString extension method on any object to get a formatted string representation of the object.  The formatting indents every member and puts it on it's own line.

e.g.
[NullReferenceException]: hashcode { 47242145 }
{
    Message : Object reference not set to an instance of an object.
    StackTrace : at some.project.class.get_Data() in C:\Source\some\project\class.cs:line 103
    Source : some
    TargetSite : System.String get_Data()
    HelpLink : {NULL}
    Data : 
  {
		UserID: 122
		CompanyID: 15
	}
    InnerException : {NULL}
}

*******************************************

I've found this to be most beneficial in two main areas: 
1) Capturing context for an exception.  In a typical logging scenario, when capturing context of an exception, 
you catch the exception, log data and then throw the exception again.  Imagine instead adding the significant 
state to the ex.Data element to travel with the exception where ever it will be logged.  No more hunting for 
relevant lines in the logs.  There's a SetContext extension method for Exceptions to make it easy to add objects 
to the Data member.  The Data member requires all objects being added to be serializable.  If the object you're 
adding is not serializable, it will be wrapped in a class that can be set to print the object when the exception 
is serialized.  This prevents the occasional bug from slipping in when you add objects that aren't serializable 
but forget to test them.  Of course that has never happened on my teams... ever....

2) Inspecting objects from 3rd party libraries.  Every once in a great while, you may find the documentation of 
a 3rd party library to be wanting.  I know, I know, it rarely happens, but when it does...  Being able to print 
out an object with data can bring more clarity to the implementation.

*******************************************

Type Inspectors (the tweakability of the ObjectPrinter): 
ObjectPrinter enumerates a list of inspectors to determine which inspector should be used for a given type.  
The chosen inspector then returns a list of members for the ObjectPrinter to print.

An example is the ExceptionTypeInspector which makes sure the base exception properties are always printed in 
the same order, followed by inherited fields and properties, then the stack trace, and finally the Data dictionary.  
This gives a consistent view of exceptions so you'll always know where to look.

The type inspectors can be overridden by setting the ObjectPrinterConfig.GetInspectors delegate.  
The default implementation is:

    public static Func<IEnumerable<ITypeInspector>> GetInspectors =
			() => new List<ITypeInspector>
			      	{
			      		new EnumTypeInspector(),
			      		new ExceptionTypeInspector(),
					new Log4NetTypeInspector(),
			      		new MsBuiltInTypeInspector(),
			      		DefaultTypeInspector
			      	};
    public static ITypeInspector DefaultTypeInspector = new DefaultTypeInspector();

A type inspector can define a specific type inspector to use for any members it returns.

Another example of a custom type inspector would be for your ORM entities.  The inspector could be 
smart enough to prevent the entity from lazy loading a property and printing out your entire database.

*******************************************

Log4Net:
add the following as the first element in the log4net config section to use the ObjectPrinter 
for all objects passed to the logger

	<renderer renderingClass="ObjectPrinter.Log4Net.Log4NetObjectRenderer, ObjectPrinter" renderedClass="System.Object" />
or
	<renderer renderingClass="ObjectPrinter.Log4Net.Log4NetObjectRenderer, ObjectPrinter" renderedClass="System.Exception" />

It may not work if it's not the first line in the config.

*******************************************

Performance:
* This has not been built with performance in mind.  
* It's intended to be used in exceptional cases where the time spent rendering the object will save significant
  time in debugging.
* Interogating objects requires reflection which is expensive.  
* If used on lazy-load objects, you could end up with a huge object graph, which is probably not what you want.
* When logging to log4net, use the object renderer to prevent the ObjectPrinter from being used until it needs to be.  
* For logging frameworks that don't support custom renderers (including Common.Logging), ensure the logging level is 
  enabled before calling DumpToString()
