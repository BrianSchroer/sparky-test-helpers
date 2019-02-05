# GetRandom.IListOf(*T*) Method (Int32, Nullable(Int32), Nullable(Int32), Action(*T*))
 

Create an <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> with properties populated with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.3

## Syntax

**C#**<br />
``` C#
public static IList<T> IListOf<T>(
	int count,
	Nullable<int> maximumDepth = null,
	Nullable<int> maximumIEnumerableSize = null,
	Action<T> callback = null
)
where T : class

```


#### Parameters
&nbsp;<dl><dt>count</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">System.Int32</a><br />The desired <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a> count.</dd><dt>maximumDepth (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum "depth" of "child" class instances to create.</dd><dt>maximumIEnumerableSize (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum number of items to generate for "child" arrays / lists / ILists.</dd><dt>callback (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />Optional "callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The type of the instance for which properties are to be updated.</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList</a>(*T*)<br />New <a href="http://msdn2.microsoft.com/en-us/library/5y536ey6" target="_blank">IList(T)</a>.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_GetRandom.md">GetRandom Class</a><br /><a href="Overload_SparkyTestHelpers_Population_GetRandom_IListOf.md">IListOf Overload</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />