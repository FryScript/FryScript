# Introduction 
FryScript is a dynamic scripting language built upon the .NET DLR that can be used to expose a scriptable API from within .NET applications.

# Getting Started
You'll need the latest version of the dotnet sdk installed.

All versions of the .NET framework and dotnet core are supported from version 4.5.2 of the .NET framework onwards. You may need to install the necessary targeting packages to ensure the build completes correctly.

# Build and Test
## Build
Set CWD to the FryScript directory containg the FryScript.sln file. Then run the following command:

```
dotnet build
```

## Test
There are currently two test projects. One for unit testing individual classes, and integration tests that test against the full script runtime.

To run all tests set CWD to the FryScript directory containg the FryScript.sln file. Then run the following command:

```
dotnet test
```
