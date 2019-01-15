# MapTesterExtensions.AssertMappedValues(*TSource*, *TDestination*) Method 
 

Assert that all defined properties were successfully maped from *TSource* to *TDestination*.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_AutoMapper.md">SparkyTestHelpers.AutoMapper</a><br />**Assembly:**&nbsp;SparkyTestHelpers.AutoMapper (in SparkyTestHelpers.AutoMapper.dll) Version: 2.1.0

## Syntax

**C#**<br />
``` C#
public static void AssertMappedValues<TSource, TDestination>(
	this MapTester<TSource, TDestination> mapTester,
	TSource source,
	TDestination dest
)

```


#### Parameters
&nbsp;<dl><dt>mapTester</dt><dd>Type: MapTester(*TSource*, *TDestination*)<br />"This" MapTester instance.</dd><dt>source</dt><dd>Type: *TSource*<br />An instance of type *TDestination*.</dd><dt>dest</dt><dd>Type: *TDestination*<br />An instance of type *TDestination*.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TSource</dt><dd>\[Missing <typeparam name="TSource"/> documentation for "M:SparkyTestHelpers.AutoMapper.MapTesterExtensions.AssertMappedValues``2(SparkyTestHelpers.Mapping.MapTester{``0,``1},``0,``1)"\]</dd><dt>TDestination</dt><dd>\[Missing <typeparam name="TDestination"/> documentation for "M:SparkyTestHelpers.AutoMapper.MapTesterExtensions.AssertMappedValues``2(SparkyTestHelpers.Mapping.MapTester{``0,``1},``0,``1)"\]</dd></dl>

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type MapTester(*TSource*, *TDestination*). When you use instance method syntax to call this method, omit the first parameter. For more information, see <a href="http://msdn.microsoft.com/en-us/library/bb384936.aspx">Extension Methods (Visual Basic)</a> or <a href="http://msdn.microsoft.com/en-us/library/bb383977.aspx">Extension Methods (C# Programming Guide)</a>.

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td>MapTesterException</td><td>if dest properties don't have expected values.</td></tr></table>

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_AutoMapper_MapTesterExtensions.md">MapTesterExtensions Class</a><br /><a href="N_SparkyTestHelpers_AutoMapper.md">SparkyTestHelpers.AutoMapper Namespace</a><br />