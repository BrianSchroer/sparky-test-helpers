# Populater.CreateRandomIEnumerableOf(*T*) Method 
 

Create <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable(T)</a> and populate with test data.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Populater.md">SparkyTestHelpers.Populater</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.0.0

## Syntax

**C#**<br />
``` C#
public IEnumerable<T> CreateRandomIEnumerableOf<T>(
	int count,
	Action<T> callback = null
)

```


#### Parameters
&nbsp;<dl><dt>count</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The desired <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable(T)</a> count.</dd><dt>callback (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />Optional "callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/9eekhta0" target="_blank">IEnumerable</a>(*T*)<br />The created and populated instance of *T*.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Populater_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Populater.md">SparkyTestHelpers.Populater Namespace</a><br />