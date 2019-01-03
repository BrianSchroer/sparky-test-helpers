# CreateRandom.InstanceOf(*T*) Method (Action(*T*))
 

Create an instance of the specified type and populate its properties with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public static T InstanceOf<T>(
	Action<T> callback
)
where T : class

```


#### Parameters
&nbsp;<dl><dt>callback</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(*T*)<br />"Callback" function to perform additional property assignments.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>The type of the instance for which properties are to be updated.</dd></dl>

#### Return Value
Type: *T*<br />New instance.

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_CreateRandom.md">CreateRandom Class</a><br /><a href="Overload_SparkyTestHelpers_Mapping_CreateRandom_InstanceOf.md">InstanceOf Overload</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />