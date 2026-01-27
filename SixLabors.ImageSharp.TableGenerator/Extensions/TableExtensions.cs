using System.Runtime.CompilerServices;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.TableGenerator.Builders;
using SixLabors.ImageSharp.TableGenerator.Extensions.Themes;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Extensions;

/// <summary>
/// Extension methods for converting collections to tables.
/// </summary>
public static class TableExtensions
{
    /// <summary>
    /// Converts a collection to a Table with default options.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <returns>A Table instance</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Table ToTable<T>(this IEnumerable<T> source)
    {
        return ToTable(source, TableGeneratorOptions.Default);
    }

    /// <summary>
    /// Converts a collection to a Table with specified options.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="options">Configuration options</param>
    /// <returns>A Table instance</returns>
    public static Table ToTable<T>(this IEnumerable<T> source, TableGeneratorOptions options)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(options);

        var items = source.ToList();
        if (items.Count == 0)
        {
            // Return empty table with just headers if no data
            return CreateEmptyTable<T>(options);
        }

        var theme = ThemeFactory.Create(options.Theme);
        var builder = new TableBuilder();

        // Apply theme base style or custom style using style builder
        var baseStyle = options.CustomStyle ?? theme.TableStyle;
        ApplyTableStyle(builder, baseStyle);

        // Set max width if specified
        if (options.MaxWidth.HasValue)
        {
            builder.Width(options.MaxWidth.Value);
        }

        // Get properties to display
        var properties = GetPropertiesToDisplay<T>(options);

        // Add header row if requested
        if (options.IncludeHeaders && properties.Length > 0)
        {
            builder.Header(header =>
            {
                ApplySectionStyle(header, theme.HeaderStyle);
                var headerValues = properties
                    .Select(prop =>
                        options.PropertyNameFormatter?.Invoke(prop)
                        ?? PropertyReflector.FormatPropertyName(prop.Name)
                    )
                    .ToArray();
                header.Row(headerValues);
            });
        }

        // Add data rows
        builder.Body(body =>
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var rowIndex = i;

                body.Row(row =>
                {
                    // Apply alternating row style
                    var rowStyle =
                        options.AlternatingRows && rowIndex % 2 == 1
                            ? theme.AlternatingRowStyle
                            : theme.RowStyle;
                    ApplyRowStyle(row, rowStyle);

                    // Add cell for each property
                    foreach (var prop in properties)
                    {
                        var value =
                            options.ValueFormatter != null
                                ? options.ValueFormatter(prop.GetValue(item!))
                                : PropertyReflector.GetValueAsString(prop, item!);

                        row.Cell(value);
                    }
                });
            }
        });

        return builder.Build();
    }

    /// <summary>
    /// Converts a collection directly to an Image with default options.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="renderOptions">Optional render options</param>
    /// <returns>A rendered table image</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Image<Rgba32> ToTableImage<T>(
        this IEnumerable<T> source,
        RenderOptions? renderOptions = null
    )
    {
        return ToTableImage(source, TableGeneratorOptions.Default, renderOptions);
    }

    /// <summary>
    /// Converts a collection directly to an Image with specified options.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="options">Configuration options</param>
    /// <param name="renderOptions">Optional render options</param>
    /// <returns>A rendered table image</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Image<Rgba32> ToTableImage<T>(
        this IEnumerable<T> source,
        TableGeneratorOptions options,
        RenderOptions? renderOptions = null
    )
    {
        var table = source.ToTable(options);
        return table.Render(renderOptions);
    }

    private static System.Reflection.PropertyInfo[] GetPropertiesToDisplay<T>(
        TableGeneratorOptions options
    )
    {
        var type = typeof(T);

        // Get properties with optional ordering
        var properties =
            options.PropertyOrder != null
                ? PropertyReflector.GetOrderedProperties(type, options.PropertyOrder)
                : PropertyReflector.GetProperties(type);

        // Apply filter if specified
        if (options.PropertyFilter != null)
        {
            properties = properties.Where(options.PropertyFilter).ToArray();
        }

        return properties;
    }

    private static Table CreateEmptyTable<T>(TableGeneratorOptions options)
    {
        var theme = ThemeFactory.Create(options.Theme);
        var builder = new TableBuilder();

        var baseStyle = options.CustomStyle ?? theme.TableStyle;
        ApplyTableStyle(builder, baseStyle);

        if (options.IncludeHeaders)
        {
            var properties = GetPropertiesToDisplay<T>(options);
            builder.Header(header =>
            {
                ApplySectionStyle(header, theme.HeaderStyle);
                var headerValues = properties
                    .Select(prop =>
                        options.PropertyNameFormatter?.Invoke(prop)
                        ?? PropertyReflector.FormatPropertyName(prop.Name)
                    )
                    .ToArray();
                header.Row(headerValues);
            });
        }

        return builder.Build();
    }

    private static void ApplyTableStyle(TableBuilder builder, TableStyle style)
    {
        builder.Style(sb =>
        {
            if (style.Background.HasValue)
                sb.Background(style.Background.Value);
            if (style.TextColor.HasValue)
                sb.TextColor(style.TextColor.Value);
            if (style.BorderColor.HasValue)
                sb.BorderColor(style.BorderColor.Value);
            if (style.BorderWidth.HasValue)
                sb.Border(style.BorderWidth.Value);
            if (style.CellPadding.HasValue)
                sb.Padding(style.CellPadding.Value);
            if (style.FontFamily != null)
                sb.FontFamily(style.FontFamily);
            if (style.FontSize.HasValue)
                sb.FontSize(style.FontSize.Value);
        });
    }

    private static void ApplySectionStyle(SectionBuilder section, TableStyle style)
    {
        if (style.Background == null && style.TextColor == null && style.FontStyle == null)
            return;

        section.Style(sb =>
        {
            if (style.Background.HasValue)
                sb.Background(style.Background.Value);
            if (style.TextColor.HasValue)
                sb.TextColor(style.TextColor.Value);
            if (style.FontStyle == FontStyle.Bold)
                sb.Bold();
            if (style.CellPadding.HasValue)
                sb.Padding(style.CellPadding.Value);
        });
    }

    private static void ApplyRowStyle(RowBuilder row, TableStyle style)
    {
        if (style.Background == null && style.TextColor == null)
            return;

        row.Style(sb =>
        {
            if (style.Background.HasValue)
                sb.Background(style.Background.Value);
            if (style.TextColor.HasValue)
                sb.TextColor(style.TextColor.Value);
        });
    }
}
