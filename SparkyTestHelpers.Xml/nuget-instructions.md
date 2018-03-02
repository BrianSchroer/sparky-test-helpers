## Creating a NuGet package from a .NET Framework project

### Generate .nuspec file
From command line in folder with .csproj file:
```
> nuget spec
Created 'SparkyTestHelpers.Xml.nuspec' successfully.
```
Creates .nuspec package with [replacement tokens](https://docs.microsoft.com/en-us/nuget/reference/nuspec#replacement-tokens).

### Create .nupkg file

```
> nuget pack SparkyTestHelpers.Xml.csproj -properties Configuration=Release
Attempting to build package from 'SparkyTestHelpers.Xml.csproj'.
MSBuild auto-detection: using msbuild version '15.5.180.51428' from 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\bin'.
Packing files from 'c:\SourceCode\sparky-test-helpers\SparkyTestHelpers.Xml\bin\Release'.
Using 'SparkyTestHelpers.Xml.nuspec' for metadata.
Found packages.config. Using packages listed as dependencies
Successfully created package 'c:\SourceCode\sparky-test-helpers\SparkyTestHelpers.Xml\SparkyTestHelpers.Xml.1.0.0.nupkg'.
```
