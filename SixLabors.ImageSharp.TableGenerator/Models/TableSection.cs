namespace SixLabors.ImageSharp.TableGenerator.Models;

/// <summary>
/// Represents a section of a table (header, body, or footer).
/// </summary>
public record TableSection
{
    public List<TableRow> Rows { get; init; } = new();
    public TableStyle? Style { get; init; }

    /// <summary>
    /// Creates an empty section.
    /// </summary>
    public TableSection() { }

    /// <summary>
    /// Creates a section with the specified rows.
    /// </summary>
    /// <param name="rows">The rows for this section</param>
    public TableSection(IEnumerable<TableRow> rows)
    {
        Rows = rows.ToList();
    }

    /// <summary>
    /// Creates a copy of this section with the specified modifications.
    /// </summary>
    public TableSection With(List<TableRow>? rows = null, TableStyle? style = null)
    {
        return this with { Rows = rows ?? Rows, Style = style ?? Style };
    }
}
