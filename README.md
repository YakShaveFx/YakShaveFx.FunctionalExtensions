# YakShaveFx.FunctionalExtensions

Yet another C# functional extensions library

There's no shortage of libraries for C# providing some features common in functional languages. This is yet another one of those libraries, although not trying to compete with any of them. I just wanted to explore some of the concepts and produce something I can use in my projects.

## Publish status

[![NuGet](https://img.shields.io/nuget/v/YakShaveFx.FunctionalExtensions.svg?logo=nuget)](https://www.nuget.org/packages/YakShaveFx.FunctionalExtensions)

[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fyakshavefx%2Ffunctionalextensions%2Fshield%2FYakShaveFx.FunctionalExtensions%2Flatest&label=YakShaveFx.FunctionalExtensions)](https://f.feedz.io/yakshavefx/functionalextensions/packages/YakShaveFx.FunctionalExtensions/latest/download)

Feed with develop branch packages available at [Feedz.io](https://f.feedz.io/yakshavefx/functionalextensions/nuget/index.json).

## Build status

|main|develop|
|---|---|
|![CI](https://github.com/YakShaveFx/YakShaveFx.FunctionalExtensions/workflows/Continuous%20Integration/badge.svg?branch=main)|![CI](https://github.com/YakShaveFx/YakShaveFx.FunctionalExtensions/workflows/Continuous%20Integration/badge.svg?branch=develop)|

## Features

### Maybe

#### Idea

`Maybe`, also known as `Optional` or `Option`, is a way to explicitly express that a value may or may not be present, instead of using `null` for such situations, which is less obvious on its intent (hopefully this gets better with [nullable reference types](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references)).

Besides better signaling intent, it also makes the code safer, as the only way to access the value is by ensuring it's there, unlike with `nulls`, where we can forget to check, and then we're greeted with a `NullReferenceException` when we least expect.

Additionally, if we follow a more functional approach to writing code (think something among the lines of LINQ), we'll get something cleaner than doing a bunch of `ifs` checking for `null`.

#### Implementation

The core `Maybe<T>` type exposes a single public method, `TryGetValue` (ignoring some overrides for comparisons), so we can explicitly extract the value if it is present.

In addition, a bunch of helper and extension methods are provided to make it more straightforward to work with the type, like helping with its creation (e.g. `Some`, `None` or `FromNullable`) or implementing common patterns (e.g. `Match`, where we provide a couple of functions as a parameter, one is called when the value is present, the other when it's not).

#### Minimal usage example

Imagine we have some repository where we want to fetch an item by id. We can make explicit that it's possible that such a value doesn't exist by using `Maybe`, instead of returning `null` and expecting the caller to remember to check.

```csharp
Maybe<SomeItem> GetItemById(ItemId id)
    => Maybe.FromNullable(db.Set<SomeItem>().Find(id));
```

Now on the caller side, we could, for example, do something if the item is present, some other thing if not.

```csharp
var maybeItem = repo.GetItemById(id);
maybeItem.Match(
    item => logger.Info("The item is present"),
    () => logger.Info("The item is not present"));
```