# Populater.CreateRandom(*T*) Method 
 

Create new instance of *T* and populate with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.0.1

## Syntax

**C#**<br />
``` C#
public T CreateRandom<T>(
	Action<T> callback = null
)

```


#### Parameters
&nbsp;<dl><dt>callback (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />Optional "callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The class type.</dd></dl>

#### Return Value
Type: *T*<br />The created and populated instance of *T*.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_Populater.md">Populater Class</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />