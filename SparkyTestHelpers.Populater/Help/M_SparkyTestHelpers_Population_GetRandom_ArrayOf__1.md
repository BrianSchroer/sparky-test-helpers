# GetRandom.ArrayOf(*T*) Method (RandomValueProvider, Int32, Nullable(Int32), Action(*T*))
 

Create an array of *T* with properties populated with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.3

## Syntax

**C#**<br />
``` C#
public static T[] ArrayOf<T>(
	RandomValueProvider randomValueProvider,
	int count,
	Nullable<int> maximumDepth = null,
	Action<T> callback = null
)
where T : class

```


#### Parameters
&nbsp;<dl><dt>randomValueProvider</dt><dd>Type: <a href="T_SparkyTestHelpers_Population_RandomValueProvider.md">SparkyTestHelpers.Population.RandomValueProvider</a><br />The <a href="T_SparkyTestHelpers_Population_RandomValueProvider.md">RandomValueProvider</a>.</dd><dt>count</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The desired array count.</dd><dt>maximumDepth (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum "depth" of "child" class instances to create.</dd><dt>callback (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />Optional "callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The type of the instance for which properties are to be updated.</dd></dl>

#### Return Value
Type: *T*[]<br />New array of *T*.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_GetRandom.md">GetRandom Class</a><br /><a href="Overload_SparkyTestHelpers_Population_GetRandom_ArrayOf.md">ArrayOf Overload</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />