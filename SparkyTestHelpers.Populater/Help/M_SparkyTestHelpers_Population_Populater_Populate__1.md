# Populater.Populate(*T*) Method 
 

Populate existing instance of *T* with test data.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.2.1

## Syntax

**C#**<br />
``` C#
public virtual T Populate<T>(
	T instance,
	IPopulaterValueProvider valueProvider = null
)

```


#### Parameters
&nbsp;<dl><dt>instance</dt><dd>Type: *T*<br />The instance of *T*.</dd><dt>valueProvider (Optional)</dt><dd>Type: <a href="T_SparkyTestHelpers_Population_IPopulaterValueProvider.md">SparkyTestHelpers.Population.IPopulaterValueProvider</a><br />The <a href="T_SparkyTestHelpers_Population_IPopulaterValueProvider.md">IPopulaterValueProvider</a> (defaults to <a href="T_SparkyTestHelpers_Population_SequentialValueProvider.md">SequentialValueProvider</a>.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: *T*<br />The populated *T* instance.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />