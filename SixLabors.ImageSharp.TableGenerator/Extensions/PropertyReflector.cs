using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorFast.ImageSharp.TableGenerator.Extensions;

/// <summary>
/// Provides cached property reflection for table generation.
/// </summary>
internal static class PropertyReflector
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();

    /// <summary>
    /// Gets the public instance properties for a type with caching.
    /// </summary>
    /// <param name="type">The type to reflect</param>
    /// <returns>Array of property info</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PropertyInfo[] GetProperties(Type type)
    {
        return _propertyCache.GetOrAdd(type, t => GetPropertiesCore(t));
    }

    private static PropertyInfo[] GetPropertiesCore(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Filter out indexed properties (e.g., Item[int])
        return properties.Where(p => p.GetIndexParameters().Length == 0).ToArray();
    }

    /// <summary>
    /// Gets ordered properties based on specified order.
    /// </summary>
    /// <param name="type">The type to reflect</param>
    /// <param name="propertyOrder">Desired property order (by name)</param>
    /// <returns>Ordered array of property info</returns>
    public static PropertyInfo[] GetOrderedProperties(Type type, IEnumerable<string> propertyOrder)
    {
        var allProperties = GetProperties(type);
        var propertyDict = allProperties.ToDictionary(
            p => p.Name,
            StringComparer.OrdinalIgnoreCase
        );
        var orderedList = new List<PropertyInfo>();

        // Add properties in specified order
        foreach (var name in propertyOrder)
        {
            if (propertyDict.TryGetValue(name, out var prop))
            {
                orderedList.Add(prop);
                propertyDict.Remove(name);
            }
        }

        // Add remaining properties in original order
        orderedList.AddRange(allProperties.Where(p => propertyDict.ContainsKey(p.Name)));

        return orderedList.ToArray();
    }

    /// <summary>
    /// Gets property value as string, handling nulls gracefully.
    /// </summary>
    /// <param name="property">The property to read</param>
    /// <param name="obj">The object instance</param>
    /// <returns>String representation of the value</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetValueAsString(PropertyInfo property, object obj)
    {
        try
        {
            var value = property.GetValue(obj);
            return value?.ToString() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Formats a property name for display (e.g., "FirstName" -> "First Name").
    /// </summary>
    /// <param name="propertyName">The property name</param>
    /// <returns>Formatted display name</returns>
    public static string FormatPropertyName(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return string.Empty;

        // Insert spaces before capital letters (PascalCase -> Pascal Case)
        var result = new System.Text.StringBuilder();
        result.Append(propertyName[0]);

        for (int i = 1; i < propertyName.Length; i++)
        {
            if (char.IsUpper(propertyName[i]) && !char.IsUpper(propertyName[i - 1]))
            {
                result.Append(' ');
            }
            result.Append(propertyName[i]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Clears the property cache (useful for testing).
    /// </summary>
    public static void ClearCache()
    {
        _propertyCache.Clear();
    }
}
