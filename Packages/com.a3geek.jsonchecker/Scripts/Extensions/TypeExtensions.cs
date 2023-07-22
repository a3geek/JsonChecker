using System;
using System.Collections.Generic;

namespace JsonChecker.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsStruct(this Type type)
            => type.IsValueType && !type.IsPrimitive && !type.IsEnum;

        public static bool IsClassOrStruct(this Type type, bool stringIsPrimitive = false)
            => type.IsClass ? !type.IsString() || !stringIsPrimitive : type.IsStruct();

        public static bool IsString(this Type type)
            => type.IsEquivalentTo(typeof(string));

        public static bool IsList(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }
}
