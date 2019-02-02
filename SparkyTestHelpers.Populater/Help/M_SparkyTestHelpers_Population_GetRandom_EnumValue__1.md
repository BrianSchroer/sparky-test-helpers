# GetRandom.EnumValue(*TEnum*) Method 
 

Get random <a href="http://msdn2.microsoft.com/en-us/library/1zt1ybx4" target="_blank">Enum</a> value.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.2.1

## Syntax

**C#**<br />
``` C#
public static TEnum EnumValue<TEnum>(
	RandomValueProvider randomValueProvider = null
)
where TEnum : Enum

```


#### Parameters
&nbsp;<dl><dt>randomValueProvider (Optional)</dt><dd>Type: <a href="T_SparkyTestHelpers_Population_RandomValueProvider.md">SparkyTestHelpers.Population.RandomValueProvider</a><br />(Optional) <a href="T_SparkyTestHelpers_Population_RandomValueProvider.md">RandomValueProvider</a> override.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TEnum</dt><dd>The <a href="http://msdn2.microsoft.com/en-us/library/1zt1ybx4" target="_blank">Enum</a> type.</dd></dl>

#### Return Value
Type: *TEnum*<br />Random *TEnum* value.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_GetRandom.md">GetRandom Class</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />