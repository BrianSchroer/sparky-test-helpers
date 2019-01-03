# RandomValuesHelper.CreateRandom(*T*) Method 
 

Create an instance of the specified type and populate its properties with random values.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

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
&nbsp;<dl><dt>T</dt><dd>The type of the instance for which properties are to be updated.</dd></dl>

#### Return Value
Type: *T*<br />New instance.

## Remarks
"Pseudonym" of <a href="M_SparkyTestHelpers_Mapping_RandomValuesHelper_CreateInstanceWithRandomValues__1.md">CreateInstanceWithRandomValues(T)(Action(T))</a>

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_RandomValuesHelper.md">RandomValuesHelper Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />