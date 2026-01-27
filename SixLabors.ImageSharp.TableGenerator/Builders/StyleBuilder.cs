using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.TableGenerator.Models;
using SixLabors.ImageSharp.TableGenerator.Utils;

namespace SixLabors.ImageSharp.TableGenerator.Builders;

/// <summary>
/// Fluent builder for creating table styles.
/// </summary>
public class StyleBuilder
{
    private TableStyle _style = new();

    /// <summary>
    /// Sets the background color.
    /// </summary>
    /// <param name="color">The background color (hex format)</param>
    public StyleBuilder Background(string color)
    {
        _style = _style with { Background = ColorParser.ParseHex(color) };
        return this;
    }

    /// <summary>
    /// Sets the background color.
    /// </summary>
    /// <param name="color">The background color</param>
    public StyleBuilder Background(Color color)
    {
        _style = _style with { Background = color };
        return this;
    }

    /// <summary>
    /// Sets the text color.
    /// </summary>
    /// <param name="color">The text color (hex format)</param>
    public StyleBuilder TextColor(string color)
    {
        _style = _style with { TextColor = ColorParser.ParseHex(color) };
        return this;
    }

    /// <summary>
    /// Sets the text color.
    /// </summary>
    /// <param name="color">The text color</param>
    public StyleBuilder TextColor(Color color)
    {
        _style = _style with { TextColor = color };
        return this;
    }

    /// <summary>
    /// Sets the border width for all sides.
    /// </summary>
    /// <param name="width">The border width</param>
    public StyleBuilder Border(float width)
    {
        _style = _style with { BorderWidth = width };
        return this;
    }

    /// <summary>
    /// Sets the top border width.
    /// </summary>
    /// <param name="width">The border width</param>
    public StyleBuilder BorderTop(float width)
    {
        _style = _style with { BorderTop = width };
        return this;
    }

    /// <summary>
    /// Sets the bottom border width.
    /// </summary>
    /// <param name="width">The border width</param>
    public StyleBuilder BorderBottom(float width)
    {
        _style = _style with { BorderBottom = width };
        return this;
    }

    /// <summary>
    /// Sets the left border width.
    /// </summary>
    /// <param name="width">The border width</param>
    public StyleBuilder BorderLeft(float width)
    {
        _style = _style with { BorderLeft = width };
        return this;
    }

    /// <summary>
    /// Sets the right border width.
    /// </summary>
    /// <param name="width">The border width</param>
    public StyleBuilder BorderRight(float width)
    {
        _style = _style with { BorderRight = width };
        return this;
    }

    /// <summary>
    /// Sets the border color.
    /// </summary>
    /// <param name="color">The border color (hex format)</param>
    public StyleBuilder BorderColor(string color)
    {
        _style = _style with { BorderColor = ColorParser.ParseHex(color) };
        return this;
    }

    /// <summary>
    /// Sets the border color.
    /// </summary>
    /// <param name="color">The border color</param>
    public StyleBuilder BorderColor(Color color)
    {
        _style = _style with { BorderColor = color };
        return this;
    }

    /// <summary>
    /// Sets the cell padding.
    /// </summary>
    /// <param name="padding">The padding value</param>
    public StyleBuilder Padding(Padding padding)
    {
        _style = _style with { CellPadding = padding };
        return this;
    }

    /// <summary>
    /// Sets uniform cell padding.
    /// </summary>
    /// <param name="all">The padding value for all sides</param>
    public StyleBuilder Padding(float all)
    {
        _style = _style with { CellPadding = Models.Padding.All(all) };
        return this;
    }

    /// <summary>
    /// Sets the font family.
    /// </summary>
    /// <param name="family">The font family name</param>
    public StyleBuilder FontFamily(string family)
    {
        _style = _style with { FontFamily = family };
        return this;
    }

    /// <summary>
    /// Sets the font size.
    /// </summary>
    /// <param name="size">The font size</param>
    public StyleBuilder FontSize(float size)
    {
        _style = _style with { FontSize = size };
        return this;
    }

    /// <summary>
    /// Sets the font style to bold.
    /// </summary>
    public StyleBuilder Bold()
    {
        _style = _style with { FontStyle = SixLabors.Fonts.FontStyle.Bold };
        return this;
    }

    /// <summary>
    /// Sets the font style to italic.
    /// </summary>
    public StyleBuilder Italic()
    {
        _style = _style with { FontStyle = SixLabors.Fonts.FontStyle.Italic };
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment.
    /// </summary>
    /// <param name="align">The alignment</param>
    public StyleBuilder HAlign(HAlign align)
    {
        _style = _style with { HAlign = align };
        return this;
    }

    /// <summary>
    /// Sets the vertical alignment.
    /// </summary>
    /// <param name="align">The alignment</param>
    public StyleBuilder VAlign(VAlign align)
    {
        _style = _style with { VAlign = align };
        return this;
    }

    /// <summary>
    /// Builds the table style.
    /// </summary>
    /// <returns>The configured table style</returns>
    public TableStyle Build()
    {
        return _style;
    }

    /// <summary>
    /// Implicit conversion to TableStyle.
    /// </summary>
    public static implicit operator TableStyle(StyleBuilder builder)
    {
        return builder.Build();
    }
}
