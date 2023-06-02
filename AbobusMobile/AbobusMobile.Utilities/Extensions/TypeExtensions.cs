using AbobusMobile.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AbobusMobile.Utilities.Extensions
{
    public static class TypeExtensions
    {
        public static void ValidateEmptyConstructor(this Type type)
        {
            if (!type.GetConstructors().Any(ctor => ctor.GetParameters().Length.Equals(0)))
            {
                throw new ValidationException($"Type {type.Name} does not have empty constuctors");
            }
        }

        public static void ValidateBaseType(this Type type, Type baseType)
        {
            if (!type.IsSubclassOf(baseType))
            {
                throw new ValidationException($"Type {type.Name} does not derive from {baseType.Name}");
            }
        }

        public static void ValidateTypeMethod(this Type type, Func<MethodInfo, bool> validatorFunction)
        {
            var availableMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (!availableMethods.Any(validatorFunction))
            {
                throw new ValidationException($"Type {type.Name} does not contain method that matches {nameof(validatorFunction)}");
            }
        }
    }
}
