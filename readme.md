ObjectPrinter dumps the object graph for a given object, 
similar to print in php & python or console.log in nodejs.

#### Why ObjectPrinter vs. json

1. system types can be output using ToString()
2. use custom type inspectors to prevent inspection of lazy load properties
3. view hashcodes to identify sameness

#### Configuration
All configuration is handled via the static Config class.  Nested sub classes are used to categorize the different configs.

#### Inspecting objects from 3rd party libraries.  
When documentation of a 3rd party library may be found wanting. Dumping an object with data can bring clarity to the implementation.

#### Capturing context for an exception.  
In a typical logging scenario, when capturing context of an exception, you catch the exception, log data and then throw the exception again.  

With the ex.SetContext(name, object) extension method, the relevant data can be added to the ex.Data element to be printed with the exception when it's logged.  Logging the data context with the exception reduces the need to track down related log messages.

The ex.Data member requires all objects being added to be serializable. It's easy for a property to be added within the graph of a logged object that is not serializable.  To handle this ex.SetContext will wrap the object in a class that will print the object when the exception is serialized.

#### Type Inspectors (the tweakability of the ObjectPrinter)
ObjectPrinter enumerates a list of inspectors to determine which inspector should be used for a given type.  The chosen inspector then returns a list of members for the ObjectPrinter to print.

An example is the ExceptionTypeInspector which makes sure the base exception properties are always printed in the same order, followed by inherited fields and properties, then the stack trace, and finally the Data dictionary. This gives a consistent view of exceptions so you'll always know where to look.

A type inspector can define a specific type inspector to use for any members it returns.

Another example of a custom type inspector would be for your ORM entities.  The inspector could be smart enough to prevent the entity from lazy loading a property and printing out your entire database.

#### log4net
add the following as the first element in the log4net config section to use the ObjectPrinter 
for all objects passed to the logger
```
	<renderer renderingClass="ObjectPrinter.Log4Net.Log4NetObjectRenderer, ObjectPrinter.Log4Net.v1212" renderedClass="System.Object" />
```
or
```
	<renderer renderingClass="ObjectPrinter.Log4Net.Log4NetObjectRenderer, ObjectPrinter.Log4Net.v1212" renderedClass="System.Exception" />
```

It may not work if it's not the first line in the config.

When using the log4net appender, you'll need to register Log4NetTypeInspector

#### Caveats

Performance:
* ObjectPrinter is intended to be used in exceptional cases where the time 
  spent rendering the object will save significant time in debugging.
* Interogating objects requires reflection, which is expensive.
  Caching can be disabled via InspecAlltypeInspector.DefaultEnableCaching.
  Caching could hurt performance if a large number of types are cached.
  Determine what's best for your application.
* If used on lazy-load objects, you could end up with a huge object 
  graph, which is probably not what you want.
* When logging to log4net, use the object renderer to prevent the 
  ObjectPrinter from being used until it needs to be.  
* For logging frameworks that don't support custom renderers 
  (including Common.Logging), ensure the logging level is enabled 
  before calling Dump()
