# .NET Core installation

We will work with SDK 3.1.403

### Windows

https://dotnet.microsoft.com/download/dotnet-core/3.1

### Linux

https://docs.microsoft.com/en-us/dotnet/core/install/linux

# Visual Studio Code installation

https://code.visualstudio.com/download.

# C# Programming with Visual Studio Code

C# language support in Vs Code is an optional, so we need to install [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) .
[Installing C# support](https://code.visualstudio.com/docs/languages/csharp#_installing-c35-support)

## Setting up a project

With .net core sdk installed and Vs Code supporting C# language. We can start programming!

**Initialize a C# project**

- Open a terminal/command prompt and navigate to the folder in which you'd like to create the app.
- Enter the following command in the command shell:

```
dotnet new console
```

**Open the project folder in Visual Studio Code.**

- For example enter the following command in the command shell:

```
code .
```

**When the project folder is first opened in Visual Studio Code**

- A "Required assets to build and debug are missing. Add them?" notification appears at the bottom right of the window.
- Select "Yes"

**Run the app by entering the following command in the command shell:**

```
dotnet run
```

[Using .NET Core in Visual Studio Code](https://code.visualstudio.com/docs/languages/dotnet)
[.NET Core CLI overview](https://docs.microsoft.com/en-us/dotnet/core/tools/)

# using NuGet;

NuGet is already installed alongside with .NET Core.

[What is NuGet?](https://www.youtube.com/watch?v=WW3bO1lNDmo)

## Install and use a NuGet package with the .NET CLI

To install and use a NuGet package in your project use the [following command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package).

```
dotnet add package <PACKAGE_NAME>
```

Here is the NuGet gallery: https://www.nuget.org/

[YouTube tutorial](https://www.youtube.com/watch?v=oM-G7un2GkI)
[MS docs](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-using-the-dotnet-cli)

## Create and Publish a NuGet Package with the .NET CLI

[YouTube tutorial](https://www.youtube.com/watch?v=f8JyT6J4b1Q)
[MS docs](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli)