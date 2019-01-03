# MapTester(*TSource*, *TDestination*).AssertMappedValues Method 
 

Assert that all defined properties were successfully maped from *TSource* to *TDestination*.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public void AssertMappedValues(
	TSource source,
	TDestination dest
)
```


#### Parameters
&nbsp;<dl><dt>source</dt><dd>Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TSource*</a><br />An instance of type *TDestination*.</dd><dt>dest</dt><dd>Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TDestination*</a><br />An instance of type *TDestination*.</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="T_SparkyTestHelpers_Mapping_MapTesterException.md">MapTesterException</a></td><td>if dest properties don't have expected values.</td></tr></table>

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination) Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />