# MapMemberTester(*TSource*, *TDestination*).IsTestedBy Method (Action(*TSource*, *TDestination*))
 

Specify custom test for property mapping.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapTester<TSource, TDestination> IsTestedBy(
	Action<TSource, TDestination> customTest
)
```


#### Parameters
&nbsp;<dl><dt>customTest</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/bb549311" target="_blank">System.Action</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />Action that examines the source and destination instances and asserts property map success.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />"Parent" <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a>.

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .WhereMember(dest => dest.IsValid)
        .IsTestedBy((src, dest) => Assert.AreEqual(src.Num + 1, dest.Num))
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination) Class</a><br /><a href="Overload_SparkyTestHelpers_Mapping_MapMemberTester_2_IsTestedBy.md">IsTestedBy Overload</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />