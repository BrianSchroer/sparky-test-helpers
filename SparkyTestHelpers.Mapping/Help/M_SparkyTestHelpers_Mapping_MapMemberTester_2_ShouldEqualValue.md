# MapMemberTester(*TSource*, *TDestination*).ShouldEqualValue Method 
 

Specify expected value that should match the *TDestination* property for which mapping is being tested.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapTester<TSource, TDestination> ShouldEqualValue(
	Object value
)
```


#### Parameters
&nbsp;<dl><dt>value</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />The expected value.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />"Parent" <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a>.

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .WhereMember(dest => dest.IsValid).ShouldEqual(true)
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination) Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />