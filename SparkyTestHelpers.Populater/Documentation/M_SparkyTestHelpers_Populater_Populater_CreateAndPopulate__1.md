# Populater.CreateAndPopulate(*T*) Method 
 

Create new instance of *T* and populate with test data.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Populater.md">SparkyTestHelpers.Populater</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.0.0

## Syntax

**C#**<br />
``` C#
public T CreateAndPopulate<T>(
	IPopulaterValueProvider valueProvider = null
)

```


#### Parameters
&nbsp;<dl><dt>valueProvider (Optional)</dt><dd>Type: <a href="T_SparkyTestHelpers_Populater_IPopulaterValueProvider.md">SparkyTestHelpers.Populater.IPopulaterValueProvider</a><br />The <a href="T_SparkyTestHelpers_Populater_IPopulaterValueProvider.md">IPopulaterValueProvider</a> (defaults to <a href="T_SparkyTestHelpers_Populater_SequentialValueProvider.md">SequentialValueProvider</a>).</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: *T*<br />The created and populated instance of *T*.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Populater_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Populater.md">SparkyTestHelpers.Populater Namespace</a><br />