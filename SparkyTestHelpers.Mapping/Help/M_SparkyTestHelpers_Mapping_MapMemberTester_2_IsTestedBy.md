# MapMemberTester(*TSource*, *TDestination*).IsTestedBy Method (Action(*TDestination*))
 

Specify custom test for property mapping.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapTester<TSource, TDestination> IsTestedBy(
	Action<TDestination> customTest
)
```


#### Parameters
&nbsp;<dl><dt>customTest</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />Action that examines the destination instances and asserts property map success.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />"Parent" <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a>.

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .WhereMember(dest => dest.IsValid).IsTestedBy((dest) => Assert.IsTrue(dest.IsValid))
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination) Class</a><br /><a href="Overload_SparkyTestHelpers_Mapping_MapMemberTester_2_IsTestedBy.md">IsTestedBy Overload</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />