# Populater.Populate(*T*) Method 
 

Populate existing instance of *T* with test data.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Populater">SparkyTestHelpers.Populater</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.0.0

## Syntax

**C#**<br />
``` C#
public T Populate<T>(
	T instance,
	IPopulaterValueProvider valueProvider = null
)

```


#### Parameters
&nbsp;<dl><dt>instance</dt><dd>Type: *T*<br />The instance of *T*.</dd><dt>valueProvider (Optional)</dt><dd>Type: <a href="T_SparkyTestHelpers_Populater_IPopulaterValueProvider">SparkyTestHelpers.Populater.IPopulaterValueProvider</a><br />The <a href="T_SparkyTestHelpers_Populater_IPopulaterValueProvider">IPopulaterValueProvider</a> (defaults to <a href="T_SparkyTestHelpers_Populater_SequentialValueProvider">SequentialValueProvider</a>.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: *T*<br />The populated *T* instance.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Populater_Populater">Populater Class</a><br /><a href="N_SparkyTestHelpers_Populater">SparkyTestHelpers.Populater Namespace</a><br />