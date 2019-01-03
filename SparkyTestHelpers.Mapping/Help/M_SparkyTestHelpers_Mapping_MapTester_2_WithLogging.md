# MapTester(*TSource*, *TDestination*).WithLogging Method 
 

Log destination property values when <a href="M_SparkyTestHelpers_Mapping_MapTester_2_AssertMappedValues.md">AssertMappedValues(TSource, TDestination)</a> is called.

**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public MapTester<TSource, TDestination> WithLogging(
	Action<string> action = null
)
```


#### Parameters
&nbsp;<dl><dt>action (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/018hxwa8" target="_blank">System.Action</a>(<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>)<br />"Callback" function to receive/log messages. (If null, <a href="http://msdn2.microsoft.com/en-us/library/zdf6yhx5" target="_blank">WriteLine()</a> is used.</dd></dl>

#### Return Value
Type: <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester</a>(<a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TSource*</a>, <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">*TDestination*</a>)<br />"This" <a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination)</a>

## See Also


#### Reference
<a href="T_SparkyTestHelpers_Mapping_MapTester_2.md">MapTester(TSource, TDestination) Class</a><br /><a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />