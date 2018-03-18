using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace SparkyTestHelpers.AspNetCore.Validation
{
    internal static class ReflectionHelper
    {
        public static string GetFieldName<TModel>(Expression<Func<TModel, object>> expression)
        {
            return GetMemberInfo(expression).Name;
        }

        public static MemberInfo GetMemberInfo<TModel>(Expression<Func<TModel, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression?.Operand as MemberExpression;
                }
            }

            var memberInfo = memberExpression.Member as MemberInfo;

            if (memberInfo == null)
            {
                throw new InvalidOperationException($"Invalid property/field expression: \"{expression}\".");
            }

            return memberInfo;
        }

        public static string GetDisplayName(MemberInfo memberInfo)
        {
            var att = memberInfo.GetCustomAttribute<DisplayAttribute>();

            return (att == null) ? memberInfo.Name : att.GetName();
        }
    }
}
