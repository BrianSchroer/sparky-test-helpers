# MapMemberTester(*TSource*, *TDestination*) Constructor 
 

Creates a new <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination)</a> instance.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapMemberTester(
	MapTester<TSource, TDestination> mapTester,
	Func<TDestination, Object> getActualValue = null,
	Func<TSource, Object> getExpectedValue = null
)
```


#### Parameters
&nbsp;<dl><dt>mapTester</dt><dd>Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">SparkyTestHelpers.Mapping.MapTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>)<br />"Parent" <a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination)</a>.</dd><dt>getActualValue (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/bb549151" target="_blank">System.Func</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TDestination*</a>, <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>)<br />Function to get the actual "mapped to" value.</dd><dt>getExpectedValue (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/bb549151" target="_blank">System.Func</a>(<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">*TSource*</a>, <a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">Object</a>)<br />Function to get the expected "mapped to" value.</dd></dl>

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapMemberTester_2.md">MapMemberTester(TSource, TDestination) Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />