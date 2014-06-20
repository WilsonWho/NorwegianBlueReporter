using System;

namespace NorwegianBlue.Util.Common
{
    public static class ReflectionUtils
    {
        /// <summary> 
        /// Check if a type subclasses a generic type 
        /// </summary>
        /// <remarks>
        /// From: http://www.jimandkatrin.com/CodeBlog/post/Does-My-Type-Subclass-a-Generic-Type.aspx
        /// Also see: http://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// 
        /// Note - this is exclusive; it doesn't consider that type is a sub-class of, if it is that class.
        /// </remarks>
        /// <param name="type">object to check</param> 
        /// <param name="genericType">The suspected base class</param> 
        /// <returns>True if this is indeed a subclass of the given generic type</returns> 
        public static bool IsSubclassOfGeneric(this Type type, Type genericType)
        {
            var baseType = type.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && 
                    baseType.GetGenericTypeDefinition() == genericType)
                    return true;
                else baseType = baseType.BaseType;
            }
            return false;
        }

            }
}
