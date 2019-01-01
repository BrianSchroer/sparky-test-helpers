# Populater.CreateIEnumerableOf(*T*) Method 
 

Create <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable(T)</a> and populate with test data.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.0.2

## Syntax

**C#**<br />
``` C#
public IEnumerable<T> CreateIEnumerableOf<T>(
	int count,
	IPopulaterValueProvider valueProvider = null
)

```


#### Parameters
&nbsp;<dl><dt>count</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The desired <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable(T)</a> count.</dd><dt>valueProvider (Optional)</dt><dd>Type: <a href="T_SparkyTestHelpers_Population_IPopulaterValueProvider.md">SparkyTestHelpers.Population.IPopulaterValueProvider</a><br />The <a href="T_SparkyTestHelpers_Population_IPopulaterValueProvider.md">IPopulaterValueProvider</a> (defaults to <a href="T_SparkyTestHelpers_Population_SequentialValueProvider.md">SequentialValueProvider</a>).</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable</a>(*T*)<br />The created and populated instance of *T*.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />