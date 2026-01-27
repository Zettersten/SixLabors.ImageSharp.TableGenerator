namespace SixLabors.ImageSharp.TableGenerator.Models;

/// <summary>
/// Represents a row of cells in a table.
/// </summary>
public record TableRow
{
    public List<TableCell> Cells { get; init; } = new();
    public TableStyle? Style { get; init; }

    /// <summary>
    /// Creates an empty row.
    /// </summary>
    public TableRow() { }

    /// <summary>
    /// Creates a row with the specified cells.
    /// </summary>
    /// <param name="cells">The cells for this row</param>
    public TableRow(IEnumerable<TableCell> cells)
    {
        Cells = cells.ToList();
    }

    /// <summary>
    /// Creates a row with cells containing the specified text values.
    /// </summary>
    /// <param name="cellTexts">The text for each cell</param>
    public TableRow(params string[] cellTexts)
    {
        Cells = cellTexts.Select(text => new TableCell(text)).ToList();
    }

    /// <summary>
    /// Creates a copy of this row with the specified modifications.
    /// </summary>
    public TableRow With(List<TableCell>? cells = null, TableStyle? style = null)
    {
        return this with { Cells = cells ?? Cells, Style = style ?? Style };
    }
}
