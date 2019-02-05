# Populater.CreateRandomIListOf(*T*) Method 
 

Create <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> and populate with random test data.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.3

## Syntax

**C#**<br />
``` C#
public virtual IList<T> CreateRandomIListOf<T>(
	int count,
	Action<T> callback = null
)

```


#### Parameters
&nbsp;<dl><dt>count</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The desired list count.</dd><dt>callback (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />Optional "callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList</a>(*T*)<br />The created and populated <a href="http://msdn2.microsoft.com/en-us/library/6sh2ey19" target="_blank">List(T)</a>.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />