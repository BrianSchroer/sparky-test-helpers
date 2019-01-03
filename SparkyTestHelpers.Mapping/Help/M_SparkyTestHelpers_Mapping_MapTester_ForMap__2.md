# MapTester.ForMap(*TSource*, *TDestination*) Method 
 

"Factory" method that creates a new <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a> instance and initializes it to automatically test properties that have the same name on the "from" and "to" *TSource* and *TDestination* types.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public static MapTester<TSource, TDestination> ForMap<TSource, TDestination>()

```


#### Type Parameters
&nbsp;<dl><dt>TSource</dt><dd>The "map from" type.</dd><dt>TDestination</dt><dd>The "map to" type.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester</a>(*TSource*, *TDestination*)<br />New <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a> instance.

## Examples

```
MapTester
    .ForMap<Foo, Bar>()
    .AssertMappedValues(foo, bar);
```


## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapTester.md">MapTester Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />