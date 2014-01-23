Dump an object in tests and sandbox apps

1. use nuget to add ObjectPrinter to your project(s)
2. use the static class ObjectPrinter.Config to set default behavior for your application
3. print to console: Console.Out.WriteLine(someObj.Dump())
4. print to output stream: someObj.DumpTo(Console.Out)


Dump an object using log4net

1. add to the first line of the log4net config section
```
   <renderer renderingClass="ObjectPrinter.Log4Net.Log4NetObjectRenderer, ObjectPrinter" renderedClass="System.Object" />
```
2. register Log4NetTypeInspector
```
   Config.Inspectors.Registration = new TypeInspectorRegistration().RegisterInspector(new Log4NetTypeInspector()).GetRegisteredInspectors();
```
3. Log.Error(ex);


Capturing more context in an exception

```csharp
try
{
...
}
catch(Exception ex)
{
    ex.SetContex("userId",userId);
	
	throw;
	//or
	Log.Error(ex);
}
```
