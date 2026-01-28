namespace BlazorFast.ImageSharp.TableGenerator.Models;

/// <summary>
/// Represents a single cell in a table.
/// </summary>
public record TableCell
{
    public string Text { get; init; } = string.Empty;
    public int ColSpan { get; init; } = 1;
    public int RowSpan { get; init; } = 1;
    public float? Width { get; init; }
    public TableStyle? Style { get; init; }

    /// <summary>
    /// Creates a new table cell with the specified text.
    /// </summary>
    /// <param name="text">The cell text</param>
    public TableCell(string text = "")
    {
        Text = text;
    }

    /// <summary>
    /// Creates a copy of this cell with the specified modifications.
    /// </summary>
    public TableCell With(
        string? text = null,
        int? colSpan = null,
        int? rowSpan = null,
        float? width = null,
        TableStyle? style = null
    )
    {
        return this with
        {
            Text = text ?? Text,
            ColSpan = colSpan ?? ColSpan,
            RowSpan = rowSpan ?? RowSpan,
            Width = width ?? Width,
            Style = style ?? Style,
        };
    }
}
