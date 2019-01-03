# MapTester(*TSource*, *TDestination*).WhereMember Method 
 

Specify *TDestination* property to be tested.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapMemberTester<TSource, TDestination> WhereMember(
	Expression<Func<TDestination, Object>> destExpression
)
```


#### Parameters
&nbsp;<dl><dt>destExpression</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/bb335710" target="_blank">System.Linq.Expressions.Expression</a>(<a href="http://msdn2.microsoft.com/en-us/library/bb549151" target="_blank">Func</a>(<a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TDestination*</a>, <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>))<br />Expression to get property name.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TDestination*</a>)<br />New <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination)</a> instance.

## Examples
MapTester.ForMap<Foo, Bar>() .WhereMember(dest => dest.Baz).ShouldEqual(src => src.Qux) .AssertMappedValues(foo, bar);

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination) Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />