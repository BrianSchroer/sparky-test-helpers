# MapTesterExtensions.AssertAutoMappedValues(*TSource*, *TDestination*) Method (MapTester`2(*TSource*, *TDestination*), *TSource*)
 

Using static Mapper, maps *source* to new *TDestination* instance and calls AssertMappedValues(UTP, UTP).

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_AutoMapper.md">SparkyTestHelpers.AutoMapper</a><br />**Assembly:**&nbsp;SparkyTestHelpers.AutoMapper (in SparkyTestHelpers.AutoMapper.dll) Version: 2.1.0

## Syntax

**C#**<br />
``` C#
public static TDestination AssertAutoMappedValues<TSource, TDestination>(
	this MapTester<TSource, TDestination> mapTester,
	TSource source
)

```


#### Parameters
&nbsp;<dl><dt>mapTester</dt><dd>Type: MapTester(*TSource*, *TDestination*)<br />The MapTester.</dd><dt>source</dt><dd>Type: *TSource*<br />The source object.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TSource</dt><dd>The type of the source.</dd><dt>TDestination</dt><dd>The type of the destination.</dd></dl>

#### Return Value
Type: *TDestination*<br />The mapped *TDestination* instance.

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type MapTester(*TSource*, *TDestination*). When you use instance method syntax to call this method, omit the first parameter. For more information, see <a href="http://msdn.microsoft.com/en-us/library/bb384936.aspx">Extension Methods (Visual Basic)</a> or <a href="http://msdn.microsoft.com/en-us/library/bb383977.aspx">Extension Methods (C# Programming Guide)</a>.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_AutoMapper_MapTesterExtensions.md">MapTesterExtensions Class</a><br /><a href="Overload_SparkyTestHelpers_AutoMapper_MapTesterExtensions_AssertAutoMappedValues.md">AssertAutoMappedValues Overload</a><br /><a href="N_SparkyTestHelpers_AutoMapper.md">SparkyTestHelpers.AutoMapper Namespace</a><br />