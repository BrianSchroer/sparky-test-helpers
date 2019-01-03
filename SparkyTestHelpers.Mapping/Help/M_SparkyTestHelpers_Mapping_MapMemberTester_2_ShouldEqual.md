# MapMemberTester(*TSource*, *TDestination*).ShouldEqual Method 
 

Specify *TSource* property that should match the *TDestination* property for which mapping is being tested.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapTester<TSource, TDestination> ShouldEqual(
	Expression<Func<TSource, Object>> sourceExpression
)
```


#### Parameters
&nbsp;<dl><dt>sourceExpression</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/bb335710" target="_blank">System.Linq.Expressions.Expression</a>(<a href="http://msdn2.microsoft.com/en-us/library/bb549151" target="_blank">Func</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>))<br />Expression to get source property name.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />"Parent" <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a>.

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .WhereMember(dest => dest.Status).ShouldEqual(src => src.StatusCode)
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination) Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />