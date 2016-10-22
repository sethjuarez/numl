# Getting Started

The newest version of numl is designed to work on the .NET platform 
(as much as we could reach). This tutorial is designed to get you started
on (hopefully) any platform. This is all CLI based.

## Installing .NET Core

[Installing .NET Core](http://dotnet.github.io/getting-started/) should 
not be too difficult if you follow the instructions outlined on the 
[Getting Started](http://dotnet.github.io/getting-started/) page. 
There are several links for installing the bits depending on your OS.

## Initializing a Project

1. Create a folder for your project
2. cd into it and `dotnet new` - this will create a basic
   console application
3. Next we will edit the `project.json` file
   in order to get `numl` (and its dependencies) referenced
   in the project. Here is the what the file should look like:

```json
{
    "version": "1.0.0-*",
    "compilationOptions": {
        "emitEntryPoint": true
    },

    "dependencies": {
        "Microsoft.NETCore.Runtime": "1.0.1-beta-*",
        "System.IO": "4.0.11-beta-*",
        "System.Console": "4.0.0-*",
        "System.Runtime": "4.0.21-beta-*",
        "System.Reflection.TypeExtensions": "4.1.0-*",
        "System.Linq": "4.0.1-*",
        "System.Collections.Concurrent": "4.0.11-*",
        "System.Linq.Expressions": "4.0.11-*",
        "numl": "0.9.9-*"
    },

    "frameworks": {
        "dnxcore50": { }
    }
}
```

These were lines added:

```json
        "System.Reflection.TypeExtensions": "4.1.0-*",
        "System.Linq": "4.0.1-*",
        "System.Collections.Concurrent": "4.0.11-*",
        "System.Linq.Expressions": "4.0.11-*",
        "numl": "0.9.9-*"
```

If you are using Visual Studio Code, as soon as you make the changes to the
`project.json` file it will ask you to update the dependencies.
   
# Writing Some Code

Add the sample [Iris.cs](..\data\Iris.zip) code to your project folder. I added some
code to my `Program.cs` file to test things out (it ended up looking like this):

```csharp
using System;
using numl.Model;
using numl.Supervised.DecisionTree;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var description = Descriptor.Create<Iris>();
            Console.WriteLine(description);
            var generator = new DecisionTreeGenerator();
            var data = Iris.Load();
            var model = generator.Generate(description, data);
            Console.WriteLine("Generated model:");
            Console.WriteLine(model);
        }
    }
}
```

# Running the code

1. From the comand line run `dotnet restore`
2. Once the process is complete run `dotnet run`

![numl running](..\images\firstrun.png)

