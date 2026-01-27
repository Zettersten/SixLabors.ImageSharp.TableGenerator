using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Builders;

/// <summary>
/// Fluent builder for creating table cells.
/// </summary>
public class CellBuilder
{
    private TableCell _cell;

    /// <summary>
    /// Creates a new cell builder with the specified text.
    /// </summary>
    /// <param name="text">The cell text</param>
    public CellBuilder(string text)
    {
        _cell = new TableCell(text);
    }

    /// <summary>
    /// Sets the column span for this cell.
    /// </summary>
    /// <param name="span">The number of columns to span (minimum 1)</param>
    public CellBuilder ColSpan(int span)
    {
        if (span < 1)
            throw new ArgumentException("Column span must be at least 1", nameof(span));

        _cell = _cell with { ColSpan = span };
        return this;
    }

    /// <summary>
    /// Sets the row span for this cell.
    /// </summary>
    /// <param name="span">The number of rows to span (minimum 1)</param>
    public CellBuilder RowSpan(int span)
    {
        if (span < 1)
            throw new ArgumentException("Row span must be at least 1", nameof(span));

        _cell = _cell with { RowSpan = span };
        return this;
    }

    /// <summary>
    /// Sets the width for this cell.
    /// </summary>
    /// <param name="width">The cell width</param>
    public CellBuilder Width(float width)
    {
        _cell = _cell with { Width = width };
        return this;
    }

    /// <summary>
    /// Sets the horizontal and vertical alignment for this cell.
    /// </summary>
    /// <param name="hAlign">The horizontal alignment</param>
    /// <param name="vAlign">The vertical alignment</param>
    public CellBuilder Align(HAlign hAlign, VAlign vAlign)
    {
        var style = _cell.Style ?? new TableStyle();
        _cell = _cell with { Style = style with { HAlign = hAlign, VAlign = vAlign } };
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment for this cell.
    /// </summary>
    /// <param name="align">The horizontal alignment</param>
    public CellBuilder Align(HAlign align)
    {
        var style = _cell.Style ?? new TableStyle();
        _cell = _cell with { Style = style with { HAlign = align } };
        return this;
    }

    /// <summary>
    /// Sets the vertical alignment for this cell.
    /// </summary>
    /// <param name="align">The vertical alignment</param>
    public CellBuilder Align(VAlign align)
    {
        var style = _cell.Style ?? new TableStyle();
        _cell = _cell with { Style = style with { VAlign = align } };
        return this;
    }

    /// <summary>
    /// Makes the cell text bold.
    /// </summary>
    public CellBuilder Bold()
    {
        var style = _cell.Style ?? new TableStyle();
        _cell = _cell with { Style = style with { FontStyle = SixLabors.Fonts.FontStyle.Bold } };
        return this;
    }

    /// <summary>
    /// Makes the cell text italic.
    /// </summary>
    public CellBuilder Italic()
    {
        var style = _cell.Style ?? new TableStyle();
        _cell = _cell with { Style = style with { FontStyle = SixLabors.Fonts.FontStyle.Italic } };
        return this;
    }

    /// <summary>
    /// Applies a style configuration to this cell.
    /// </summary>
    /// <param name="configure">The style configuration action</param>
    public CellBuilder Style(Action<StyleBuilder> configure)
    {
        var styleBuilder = new StyleBuilder();
        configure(styleBuilder);

        var currentStyle = _cell.Style ?? new TableStyle();
        _cell = _cell with { Style = currentStyle.Merge(styleBuilder.Build()) };
        return this;
    }

    /// <summary>
    /// Builds the table cell.
    /// </summary>
    /// <returns>The configured table cell</returns>
    public TableCell Build()
    {
        return _cell;
    }

    /// <summary>
    /// Implicit conversion to TableCell.
    /// </summary>
    public static implicit operator TableCell(CellBuilder builder)
    {
        return builder.Build();
    }
}
