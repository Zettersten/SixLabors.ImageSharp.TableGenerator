using System.Reflection;
using BlazorFast.ImageSharp.TableGenerator.Models;

namespace BlazorFast.ImageSharp.TableGenerator.Extensions;

/// <summary>
/// Configuration options for automatic table generation from collections.
/// </summary>
public class TableGeneratorOptions
{
    /// <summary>
    /// Gets or sets whether to include a header row with property names.
    /// Default is true.
    /// </summary>
    public bool IncludeHeaders { get; set; } = true;

    /// <summary>
    /// Gets or sets the visual theme mode.
    /// Default is Light.
    /// </summary>
    public ThemeMode Theme { get; set; } = ThemeMode.Light;

    /// <summary>
    /// Gets or sets a filter function to determine which properties to include.
    /// If null, all public properties are included.
    /// </summary>
    public Func<PropertyInfo, bool>? PropertyFilter { get; set; }

    /// <summary>
    /// Gets or sets a function to format property names for header display.
    /// If null, uses PropertyReflector.FormatPropertyName (e.g., "FirstName" -> "First Name").
    /// </summary>
    public Func<PropertyInfo, string>? PropertyNameFormatter { get; set; }

    /// <summary>
    /// Gets or sets a function to format property values as strings.
    /// If null, uses ToString() with null coalescing to empty string.
    /// </summary>
    public Func<object?, string>? ValueFormatter { get; set; }

    /// <summary>
    /// Gets or sets custom table style to override theme defaults.
    /// </summary>
    public TableStyle? CustomStyle { get; set; }

    /// <summary>
    /// Gets or sets the order of properties by name.
    /// Properties not in this list appear after, in their original order.
    /// </summary>
    public IEnumerable<string>? PropertyOrder { get; set; }

    /// <summary>
    /// Gets or sets whether to use alternating row colors.
    /// Default is true.
    /// </summary>
    public bool AlternatingRows { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum width constraint for the table.
    /// If null, no constraint is applied.
    /// </summary>
    public float? MaxWidth { get; set; }

    /// <summary>
    /// Creates default options with Light theme.
    /// </summary>
    public static TableGeneratorOptions Default => new();
}
