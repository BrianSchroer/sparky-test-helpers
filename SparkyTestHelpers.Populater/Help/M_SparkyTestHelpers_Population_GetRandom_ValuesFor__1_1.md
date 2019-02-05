# GetRandom.ValuesFor(*T*) Method (*T*, Nullable(Int32), Nullable(Int32))
 

Populate existing instance of *T* with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Populater (in SparkyTestHelpers.Populater.dll) Version: 1.3

## Syntax

**C#**<br />
``` C#
public static T ValuesFor<T>(
	T instance,
	Nullable<int> maximumDepth = null,
	Nullable<int> maximumIEnumerableSize = null
)
where T : class

```


#### Parameters
&nbsp;<dl><dt>instance</dt><dd>Type: *T*<br />The instance.</dd><dt>maximumDepth (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum "depth" of "child" class instances to create (default value is 5).</dd><dt>maximumIEnumerableSize (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum number of items to generate for arrays / lists / IEnumerables (default value is 3).</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The instance type.</dd></dl>

#### Return Value
Type: *T*<br />Populated instance of *T*.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Population_GetRandom.md">GetRandom Class</a><br /><a href="Overload_SparkyTestHelpers_Population_GetRandom_ValuesFor.md">ValuesFor Overload</a><br /><a href="N_SparkyTestHelpers_Population.md">SparkyTestHelpers.Population Namespace</a><br />