# CreateRandom.InstanceOf(*T*) Method (Nullable(Int32), Nullable(Int32), Action(*T*))
 

Create an instance of the specified type and populate its properties with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public static T InstanceOf<T>(
	Nullable<int> maximumDepth = null,
	Nullable<int> maximumIEnumerableSize = null,
	Action<T> callback = null
)
where T : class

```


#### Parameters
&nbsp;<dl><dt>maximumDepth (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum "depth" of "child" class instances to create.</dd><dt>maximumIEnumerableSize (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/b3h38hb0" target="_blank">System.Nullable</a>(<a href="http://msdn2.microsoft.com/en-us/library/td2s409d" target="_blank">Int32</a>)<br />Optional maximum number of items to generate for arrays / lists / IEnumerables.</dd><dt>callback (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />Optional "callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The type of the instance for which properties are to be updated.</dd></dl>

#### Return Value
Type: *T*<br />New instance.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_CreateRandom.md">CreateRandom Class</a><br /><a href="Overload_SparkyTestHelpers_Mapping_CreateRandom_InstanceOf.md">InstanceOf Overload</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />