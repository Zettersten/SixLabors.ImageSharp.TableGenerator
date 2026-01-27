using System.Runtime.CompilerServices;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace SixLabors.ImageSharp.TableGenerator.Models;

/// <summary>
/// Defines the styling properties for table elements.
/// </summary>
public record TableStyle
{
    public Color? Background { get; init; }
    public Color? TextColor { get; init; }
    public Color? BorderColor { get; init; }
    public float? BorderWidth { get; init; }
    public float? BorderTop { get; init; }
    public float? BorderRight { get; init; }
    public float? BorderBottom { get; init; }
    public float? BorderLeft { get; init; }
    public Padding? CellPadding { get; init; }
    public HAlign? HAlign { get; init; }
    public VAlign? VAlign { get; init; }
    public string? FontFamily { get; init; }
    public float? FontSize { get; init; }
    public FontStyle? FontStyle { get; init; }

    /// <summary>
    /// Creates an empty style.
    /// </summary>
    public static readonly TableStyle Empty = new();

    /// <summary>
    /// Merges this style with another style, with the other style taking precedence for non-null values.
    /// </summary>
    /// <param name="other">The style to merge with</param>
    /// <returns>A new merged style</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TableStyle Merge(TableStyle? other)
    {
        if (other == null)
            return this;

        return new TableStyle
        {
            Background = other.Background ?? Background,
            TextColor = other.TextColor ?? TextColor,
            BorderColor = other.BorderColor ?? BorderColor,
            BorderWidth = other.BorderWidth ?? BorderWidth,
            BorderTop = other.BorderTop ?? BorderTop,
            BorderRight = other.BorderRight ?? BorderRight,
            BorderBottom = other.BorderBottom ?? BorderBottom,
            BorderLeft = other.BorderLeft ?? BorderLeft,
            CellPadding = other.CellPadding ?? CellPadding,
            HAlign = other.HAlign ?? HAlign,
            VAlign = other.VAlign ?? VAlign,
            FontFamily = other.FontFamily ?? FontFamily,
            FontSize = other.FontSize ?? FontSize,
            FontStyle = other.FontStyle ?? FontStyle,
        };
    }

    /// <summary>
    /// Creates a default style with common values.
    /// </summary>
    public static TableStyle Default =>
        new()
        {
            Background = Color.White,
            TextColor = Color.Black,
            BorderColor = Color.Black,
            BorderWidth = 1f,
            CellPadding = Padding.All(8f),
            HAlign = Models.HAlign.Left,
            VAlign = Models.VAlign.Top,
            FontFamily = "Arial",
            FontSize = 12f,
            FontStyle = SixLabors.Fonts.FontStyle.Regular,
        };
}
