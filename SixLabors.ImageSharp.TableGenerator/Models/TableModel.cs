namespace SixLabors.ImageSharp.TableGenerator.Models;

/// <summary>
/// Represents the complete table model with all sections and configuration.
/// </summary>
public record TableModel
{
    public TableSection? Header { get; init; }
    public TableSection Body { get; init; } = new();
    public TableSection? Footer { get; init; }
    public TableStyle Style { get; init; } = TableStyle.Default;
    public float? MaxWidth { get; init; }
    public List<ColumnSpec> Columns { get; init; } = new();

    /// <summary>
    /// Creates an empty table model.
    /// </summary>
    public TableModel() { }

    /// <summary>
    /// Creates a copy of this table model with the specified modifications.
    /// </summary>
    public TableModel With(
        TableSection? header = null,
        TableSection? body = null,
        TableSection? footer = null,
        TableStyle? style = null,
        float? maxWidth = null,
        List<ColumnSpec>? columns = null
    )
    {
        return this with
        {
            Header = header ?? Header,
            Body = body ?? Body,
            Footer = footer ?? Footer,
            Style = style ?? Style,
            MaxWidth = maxWidth ?? MaxWidth,
            Columns = columns ?? Columns,
        };
    }

    /// <summary>
    /// Gets all sections in order (header, body, footer).
    /// </summary>
    public IEnumerable<TableSection> GetAllSections()
    {
        if (Header != null)
            yield return Header;
        yield return Body;
        if (Footer != null)
            yield return Footer;
    }

    /// <summary>
    /// Gets all rows from all sections in order.
    /// </summary>
    public IEnumerable<TableRow> GetAllRows()
    {
        return GetAllSections().SelectMany(section => section.Rows);
    }
}
