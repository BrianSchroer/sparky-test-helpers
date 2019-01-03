# MapMemberTester(*TSource*, *TDestination*) Class
 

This class is for testing that a property was successfully "mapped" from one type to another.


## Inheritance Hierarchy
<a href="http://msdn2.microsoft.com/en-us/library/e5kfa45b" target="_blank">System.Object</a><br />&nbsp;&nbsp;SparkyTestHelpers.Mapping.MapMemberTester(TSource, TDestination)<br />
**Namespace:**&nbsp;<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping</a><br />**Assembly:**&nbsp;SparkyTestHelpers.Mapping (in SparkyTestHelpers.Mapping.dll) Version: 1.10.2

## Syntax

**C#**<br />
``` C#
public class MapMemberTester<TSource, TDestination>

```


#### Type Parameters
&nbsp;<dl><dt>TSource</dt><dd>The"map from" type.</dd><dt>TDestination</dt><dd>The "map to" type.</dd></dl>&nbsp;
The MapMemberTester(TSource, TDestination) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SparkyTestHelpers_Mapping_MapMemberTester_2__ctor.md">MapMemberTester(TSource, TDestination)</a></td><td>
Creates a new MapMemberTester(TSource, TDestination) instance.</td></tr></table>&nbsp;
<a href="#mapmembertester(*tsource*,-*tdestination*)-class.md">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Code example](media/CodeExample.png "Code example")</td><td><a href="M_SparkyTestHelpers_Mapping_MapMemberTester_2_IsTestedBy.md">IsTestedBy(Action(TDestination))</a></td><td>
Specify custom test for property mapping.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Code example](media/CodeExample.png "Code example")</td><td><a href="M_SparkyTestHelpers_Mapping_MapMemberTester_2_IsTestedBy_1.md">IsTestedBy(Action(TSource, TDestination))</a></td><td>
Specify custom test for property mapping.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SparkyTestHelpers_Mapping_MapMemberTester_2_ShouldBeStringMatchFor.md">ShouldBeStringMatchFor</a></td><td>
Specifies that destination property .ToString() should match source property .ToString().</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Code example](media/CodeExample.png "Code example")</td><td><a href="M_SparkyTestHelpers_Mapping_MapMemberTester_2_ShouldEqual.md">ShouldEqual</a></td><td>
Specify *TSource* property that should match the *TDestination* property for which mapping is being tested.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Code example](media/CodeExample.png "Code example")</td><td><a href="M_SparkyTestHelpers_Mapping_MapMemberTester_2_ShouldEqualValue.md">ShouldEqualValue</a></td><td>
Specify expected value that should match the *TDestination* property for which mapping is being tested.</td></tr></table>&nbsp;
<a href="#mapmembertester(*tsource*,-*tdestination*)-class.md">Back to Top</a>

## See Also


#### Reference
<a href="N_SparkyTestHelpers_Mapping.md">SparkyTestHelpers.Mapping Namespace</a><br />