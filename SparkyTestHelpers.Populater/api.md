
_see also_:
* [**Complete API Documentation**](https://github.com/BrianSchroer/sparky-test-helpers/blob/master/SparkyTestHelpers.Populater/Help/Home.md)
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
This project provides tools for populating class instance properties for testing purposes, usually with random values:

```csharp
using SparkyTestHelpers.Populater;
```
```csharp
    var populater = new Populater();

    var foo = new Foo();
    populater.PopulateWithRandomValues(foo);

    var foo2 = populater.CreateRandom<Foo>();
```

There's also a **GetRandom** static method that uses **Populater** "behind the scenes":

```csharp
using SparkyTestHelpers.Populater;
```
```csharp
    var foo = GetRandom.InstanceOf<Foo>();
```

The project also has a **SequentialValueProvider** and associated methods that populates class properties with predictable/repeatable values. I hope to use it in the future for "snapshot testing":

```csharp
using SparkyTestHelpers.Populater;
```
```csharp
    var populater = new Populater();

    var foo = new Foo();
    populater.Populate(foo, new SequentialValueProvider()); 

    // (SequentialValueProvider is the default provider for the "Populate" method:
    var foo2 = new Foo();
    populater.Populate(foo);

    var foo3 = populater.CreateAndPopulate<Foo>();
```