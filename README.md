# Conductor [![Travis](https://img.shields.io/travis/robertcoltheart/Conductor.svg)](https://travis-ci.org/robertcoltheart/Conductor) [![NuGet](https://img.shields.io/nuget/v/Conductor.svg)](https://www.nuget.org/packages/Conductor) 
A simple framework for building modular applications.

## Usage
Install the package from NuGet with `nuget install Conductor`.

Modules can implement the `IModule` interface in order to be loaded by the module loader.

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

## Contributing
Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on how to contribute to this project.

## License
Conductor is released under the [MIT License](LICENSE)
