using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

/// <summary>
/// Helper class for providing expressions to be used by <see cref="MoqExtensions"/>.
/// </summary>
public static class MoqExpressionBuilder
{
    private static readonly Dictionary<Type, PropertyInfo[]> _propertyCache 
        = new Dictionary<Type, PropertyInfo[]>();

    private static readonly Dictionary<Type, MethodInfo[]> _methodCache
        = new Dictionary<Type, MethodInfo[]>();
}
