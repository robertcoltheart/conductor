# Conductor
A simple framework for building modular applications.

## Usage
```csharp
// Use any dependency injection container, using the IServiceProvider as a go-between
var container = new Container();

var loader = new ModuleLoader(x => container.Resolve(x));

// Register types in the loader
loader.Add(typeof (DataModule))
	  .Add(typeof (ServicesModule));

// Load all modules
loader.Initialize();
```
