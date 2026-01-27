using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Builders;

/// <summary>
/// Fluent builder for creating table sections (header, body, footer).
/// </summary>
public class SectionBuilder
{
    private readonly List<TableRow> _rows = new();
    private TableStyle? _style;

    /// <summary>
    /// Applies a style configuration to this section.
    /// </summary>
    /// <param name="configure">The style configuration action</param>
    public SectionBuilder Style(Action<StyleBuilder> configure)
    {
        var styleBuilder = new StyleBuilder();
        configure(styleBuilder);

        _style = (_style ?? new TableStyle()).Merge(styleBuilder.Build());
        return this;
    }

    /// <summary>
    /// Adds a row to this section using a row builder.
    /// </summary>
    /// <param name="configure">The row configuration action</param>
    public SectionBuilder Row(Action<RowBuilder> configure)
    {
        var rowBuilder = new RowBuilder();
        configure(rowBuilder);
        _rows.Add(rowBuilder.Build());
        return this;
    }

    /// <summary>
    /// Adds a pre-configured row to this section.
    /// </summary>
    /// <param name="row">The row to add</param>
    public SectionBuilder Row(TableRow row)
    {
        _rows.Add(row);
        return this;
    }

    /// <summary>
    /// Adds multiple rows to this section.
    /// </summary>
    /// <param name="rows">The rows to add</param>
    public SectionBuilder Rows(params TableRow[] rows)
    {
        _rows.AddRange(rows);
        return this;
    }

    /// <summary>
    /// Adds a simple row with the specified cell texts.
    /// </summary>
    /// <param name="cellTexts">The text for each cell in the row</param>
    public SectionBuilder Row(params string[] cellTexts)
    {
        _rows.Add(new TableRow(cellTexts));
        return this;
    }

    /// <summary>
    /// Builds the table section.
    /// </summary>
    /// <returns>The configured table section</returns>
    public TableSection Build()
    {
        return new TableSection(_rows) { Style = _style };
    }

    /// <summary>
    /// Implicit conversion to TableSection.
    /// </summary>
    public static implicit operator TableSection(SectionBuilder builder)
    {
        return builder.Build();
    }
}
