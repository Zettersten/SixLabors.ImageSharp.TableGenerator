using BlazorFast.ImageSharp.TableGenerator.Models;

namespace BlazorFast.ImageSharp.TableGenerator.Builders;

/// <summary>
/// Fluent builder for creating table rows.
/// </summary>
public class RowBuilder
{
    private readonly List<TableCell> _cells = new();
    private TableStyle? _style;

    /// <summary>
    /// Applies a style configuration to this row.
    /// </summary>
    /// <param name="configure">The style configuration action</param>
    public RowBuilder Style(Action<StyleBuilder> configure)
    {
        var styleBuilder = new StyleBuilder();
        configure(styleBuilder);

        _style = (_style ?? new TableStyle()).Merge(styleBuilder.Build());
        return this;
    }

    /// <summary>
    /// Adds a cell with the specified text to this row and returns this RowBuilder for chaining.
    /// </summary>
    /// <param name="text">The cell text</param>
    /// <param name="configure">Optional cell configuration action</param>
    public RowBuilder Cell(string text, Action<CellBuilder>? configure = null)
    {
        var cellBuilder = new CellBuilder(text);
        configure?.Invoke(cellBuilder);
        _cells.Add(cellBuilder.Build());
        return this;
    }

    /// <summary>
    /// Adds a pre-configured cell to this row.
    /// </summary>
    /// <param name="cell">The cell to add</param>
    public RowBuilder Cell(TableCell cell)
    {
        _cells.Add(cell);
        return this;
    }

    /// <summary>
    /// Adds multiple cells with the specified text values to this row.
    /// </summary>
    /// <param name="cellTexts">The text for each cell</param>
    public RowBuilder Cells(params string[] cellTexts)
    {
        foreach (var text in cellTexts)
        {
            _cells.Add(new TableCell(text));
        }
        return this;
    }

    /// <summary>
    /// Builds the table row.
    /// </summary>
    /// <returns>The configured table row</returns>
    public TableRow Build()
    {
        return new TableRow(_cells) { Style = _style };
    }

    /// <summary>
    /// Implicit conversion to TableRow.
    /// </summary>
    public static implicit operator TableRow(RowBuilder builder)
    {
        return builder.Build();
    }
}
