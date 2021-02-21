# Hello World
FryScript is designed to make exposing a scriptable API layer within a .NET application relatively trivial. This guide will walk you through writing a "Hello World" application in C# and FryScript by creating a simple API and integrating it into a console application host.

# Requirements
- The .NET 5.0 SDK.
- Visual Studio Code.

# let's get started!
## Setting up the API
Firstly let's spin up a new C# project in a new directory named "FryHelloWorld" in the working directory of your choice.

```
dotnet new console
```

Now we've got a directory containing ```FryHelloWorld.csproj``` and ```Program.cs```, before we start writing any code let's install the FryScript NuGet package so we have something to work with!

```
dotnet add package FryScript
```

Great job! Now we've got all we need to start writing our host app. let's start by spinning up Visual Studio Code and adding our API layer.

```
code .
```

Once Visual Studio Code has launched add a new file with Ctrl + N and then save it with Ctrl + S, name it ```ConsoleApi.cs``` and then setup the following class.

```C#
using FryScript;

namespace FryHelloWorld
{
    [ScriptableType("console")]
    public class ConsoleApi
    {

    }
}
```

Here we've created an empty class definition for ```ConsoleApi```, the important part is the value ```"console"``` being passed into the ```ScriptableType``` attribute. This names our class so that it can be imported into the Fry Script runtime and then be used by importing ```console``` in to our scripts.

Now that we've got a simple API class setup let's add some basic functionality that we can call from within our scripts. Add the following code to the ```ConsoleApi``` class.

```C#
using System;
using FryScript;

namespace FryHelloWorld
{
    [ScriptableType("console")]
    public class ConsoleApi
    {
        [ScriptableMethod("writeLine")]
        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}
```

We've now added an API method named ```writeLine``` to our ```ConsoleApi``` using the ```ScriptableMethod``` attribute. This method simply wraps the .NET provided ```System.Console.WriteLine``` method. So far so good, let's setup the script runtime and expose our API class.

## Setting up the script runtime
Now that we've got a ```ConsoleApi``` we need to expose it to a script runtime so that we can create scritps that can use it. Open the ```Program.cs``` file in our console application, remove ```Console.WriteLine("Hello World!")``` from within the ```Main``` method and add the following code.

```C#
using FryScript;

namespace FryHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtime = new ScriptRuntime();

            runtime.Import<ConsoleApi>();
        }
    }
}
```

That's it we've now got a script runtime and an API that we can use to write some scripts, not too shabby!

## Writing our first script
Let's create our first script and get it doing something, use Ctrl + N and then Ctrl + S to save a file called ```HelloWorld.fry```. Now let's import our console API and use it from within a script. Add the following code to ```HelloWorld.fry```.

```
@import "console" as consoleApi;
```

Notice that we've imported the ```console``` using the name we used in the ```ScriptableType``` attribute decorating our ```ConsoleApi``` class. The API is imported and assigned to the variable named ```consoleApi```, this is the name we will use within our script to interact with our API. Now that we've got access to the console API let's create a method within the script that will use it. Add the following code to ```HelloWorld.fry```.

```
@import "console" as consoleApi;

this.sayHello = () => {
    consoleApi.writeLine("Hello world!");
};
```

In Fry Script each script represents a single object, here we've added a method named ```sayHello``` to our ```HelloWorld.fry``` script.

## Calling our script from the host application

Before we can call our script from within our host application we'll need to add the following XML to the ```FryHelloWorld.csproj``` file within the root ```<Project>``` node of the XML.

```XML
<ItemGroup>
    <None Update="*.fry">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

This will ensure that our script files get copied to the bin directory when we build our application and that the script runtime will be able to locate them.

Now we can get a reference to our ```HelloWorld.fry``` script from within our host application and invoke it's ```sayHello``` method from C#, let's expand our ```Progam.cs``` file with the following code.

```C#
using FryScript;

namespace FryHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtime = new ScriptRuntime();

            runtime.Import<ConsoleApi>();

            dynamic helloWorld = runtime.Get("HelloWorld");

            helloWorld.sayHello();
        }
    }
}
```

We use ```runtime.Get("HelloWorld")``` to import and compile our ```HelloWorld.fry``` script (note that we don't supply the file extension when importing scripts). The imported script is referenced as a ```dynamic``` object which allows us to call the ```sayHello``` method from within C# using the .NET dynamic language runtime. Let's use ```dotnet run``` to run our application and see what happens.

All being well we should see ```Hello world!``` in the terminal. We did it! We wrote our first Fry Script and integrated it into a .NET host application!