using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute {}

    public static class DependencyInjector
    {
        static readonly object placeholder = new object();
        static readonly List<FieldInfo> injectFieldsBuffer = new List<FieldInfo>();
        static readonly Dictionary<Type, object> components = new Dictionary<Type, object>();
        static readonly Dictionary<Type, FieldInfo[]> cachedFields = new Dictionary<Type, FieldInfo[]>();

        public static void Inject(this object target)
        {
            InjectObject(target);
        }

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public static void ReplaceComponent<T>(T newComponent)
        {
            Type type = typeof(T);

            if (components.ContainsKey(type))
            {
                if (components[type] is IDisposable cleanableComponent)
                    cleanableComponent.Dispose();

                components[type] = null;
            }

            components[type] = newComponent;

            InjectObject(newComponent);

            foreach (var c in components.Values)
            {
                foreach (var f in GetFields(c.GetType()))
                {
                    if (f.FieldType == typeof(T))
                        f.SetValue(c, newComponent);
                }
            }
        }
        
        public static void RemoveComponent<T>()
        {
            RemoveComponent(components[typeof(T)]);
        }

        public static void RemoveComponent<T>(T removableComponent)
        {
            if (removableComponent is IDisposable cleanableComponent)
                cleanableComponent.Dispose();

            components.Remove(typeof(T));
        }

        public static void ClearCache()
        {
            foreach (var componentPair in components)
            {
                if (componentPair.Value is IDisposable cleanableComponent)
                    cleanableComponent.Dispose();
            }

            cachedFields.Clear();
            components.Clear();

            GC.Collect();
        }

        private static void InjectObject(object target)
        {
            FieldInfo[] fields = GetFields(target.GetType());

            for (int i = 0; i < fields.Length; i++)
                fields[i].SetValue(target, Resolve(fields[i].FieldType));
        }

        private static FieldInfo[] GetFields(Type type)
        {
            if (!cachedFields.TryGetValue(type, out var fields))
            {
                fields = GetInjectFields(type);
                cachedFields.Add(type, fields);
            }

            return fields;
        }

        private static FieldInfo[] GetInjectFields(Type type)
        {
            while (type != null)
            {
                FieldInfo[] typeFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                for (int i = 0; i < typeFields.Length; i++)
                {
                    if (typeof(InjectAttribute).IsDefined(typeFields[i]))
                        injectFieldsBuffer.Add(typeFields[i]);
                }

                type = type.BaseType;
            }

            var resultFields = injectFieldsBuffer.ToArray();
            injectFieldsBuffer.Clear();

            return resultFields;
        }

        private static object Resolve(Type type)
        {
            if (components.TryGetValue(type, out object component))
            {
                if (placeholder == component)
                    throw new Exception("Cyclic dependency detected in " + type);
            }
            else
            {
                components[type] = placeholder;
                component = components[type] = CreateComponent(type);
                InjectObject(component);
            }

            return component;
        }

        private static object CreateComponent(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        private static bool IsDefined(this Type attribute, MemberInfo m)
        {
            return m.GetCustomAttributes(attribute, true).Any();
        }
    }
}