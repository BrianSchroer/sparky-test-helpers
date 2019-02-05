# Populater.GetValue Method 
 

Get value.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.3

## Syntax

**C#**<br />
``` C#
protected Object GetValue(
	IPopulaterValueProvider valueProvider,
	Type type,
	string prefix,
	int depth
)
```


#### Parameters
&nbsp;<dl><dt>valueProvider</dt><dd>Type: <a href="T_SparkyTestHelpers_Population_IPopulaterValueProvider.md">SparkyTestHelpers.Population.IPopulaterValueProvider</a><br />The <a href="T_SparkyTestHelpers_Population_IPopulaterValueProvider.md">IPopulaterValueProvider</a>.</dd><dt>type</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/42892f65" target="_blank">System.Type</a><br />The value type.</dd><dt>prefix</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Prefix for string values.</dd><dt>depth</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />Depth of value within object hierarchy.</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a><br />The value.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />