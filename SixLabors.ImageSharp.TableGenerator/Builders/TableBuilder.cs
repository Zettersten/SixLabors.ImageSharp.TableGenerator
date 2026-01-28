using BlazorFast.ImageSharp.TableGenerator.Models;

namespace BlazorFast.ImageSharp.TableGenerator.Builders;

/// <summary>
/// Main fluent builder for creating tables.
/// </summary>
public class TableBuilder
{
    private TableStyle _style = TableStyle.Default;
    private float? _maxWidth;
    private List<ColumnSpec> _columns = new();
    private TableSection? _header;
    private TableSection _body = new();
    private TableSection? _footer;

    /// <summary>
    /// Creates a new table builder.
    /// </summary>
    /// <returns>A new TableBuilder instance</returns>
    public static TableBuilder Create() => new();

    /// <summary>
    /// Sets the default font for the table.
    /// </summary>
    /// <param name="family">The font family</param>
    /// <param name="size">The font size</param>
    public TableBuilder DefaultFont(string family, float size)
    {
        _style = _style with { FontFamily = family, FontSize = size };
        return this;
    }

    /// <summary>
    /// Sets the default cell padding for the table.
    /// </summary>
    /// <param name="horizontal">Horizontal padding</param>
    /// <param name="vertical">Vertical padding</param>
    public TableBuilder CellPadding(float horizontal, float vertical)
    {
        _style = _style with { CellPadding = Padding.All(horizontal, vertical) };
        return this;
    }

    /// <summary>
    /// Sets the default cell padding for the table.
    /// </summary>
    /// <param name="padding">Uniform padding for all sides</param>
    public TableBuilder CellPadding(float padding)
    {
        _style = _style with { CellPadding = Padding.All(padding) };
        return this;
    }

    /// <summary>
    /// Sets the default border width for the table.
    /// </summary>
    /// <param name="width">The border width</param>
    public TableBuilder Border(float width)
    {
        _style = _style with { BorderWidth = width };
        return this;
    }

    /// <summary>
    /// Sets the maximum width for the table.
    /// </summary>
    /// <param name="maxWidth">The maximum table width</param>
    public TableBuilder Width(float maxWidth)
    {
        _maxWidth = maxWidth;
        return this;
    }

    /// <summary>
    /// Configures the column specifications for the table.
    /// </summary>
    /// <param name="configure">The column configuration action</param>
    public TableBuilder Columns(Action<ColumnsBuilder> configure)
    {
        var columnsBuilder = new ColumnsBuilder();
        configure(columnsBuilder);
        _columns = columnsBuilder.Build();
        return this;
    }

    /// <summary>
    /// Configures the table header section.
    /// </summary>
    /// <param name="configure">The header configuration action</param>
    public TableBuilder Header(Action<SectionBuilder> configure)
    {
        var sectionBuilder = new SectionBuilder();
        configure(sectionBuilder);
        _header = sectionBuilder.Build();
        return this;
    }

    /// <summary>
    /// Configures the table body section.
    /// </summary>
    /// <param name="configure">The body configuration action</param>
    public TableBuilder Body(Action<SectionBuilder> configure)
    {
        var sectionBuilder = new SectionBuilder();
        configure(sectionBuilder);
        _body = sectionBuilder.Build();
        return this;
    }

    /// <summary>
    /// Configures the table footer section.
    /// </summary>
    /// <param name="configure">The footer configuration action</param>
    public TableBuilder Footer(Action<SectionBuilder> configure)
    {
        var sectionBuilder = new SectionBuilder();
        configure(sectionBuilder);
        _footer = sectionBuilder.Build();
        return this;
    }

    /// <summary>
    /// Applies alternating row styles to the body section.
    /// </summary>
    /// <param name="evenRowStyle">Style for even-numbered rows (0-based)</param>
    /// <param name="oddRowStyle">Style for odd-numbered rows (0-based)</param>
    public TableBuilder AlternateRows(TableStyle evenRowStyle, TableStyle oddRowStyle)
    {
        var rows = _body
            .Rows.Select(
                (row, index) =>
                {
                    var alternatingStyle = index % 2 == 0 ? evenRowStyle : oddRowStyle;
                    return row with
                    {
                        Style = (row.Style ?? new TableStyle()).Merge(alternatingStyle),
                    };
                }
            )
            .ToList();

        _body = _body with { Rows = rows };
        return this;
    }

    /// <summary>
    /// Applies alternating row styles to the body section using style builders.
    /// </summary>
    /// <param name="configureEven">Configuration for even-numbered rows (0-based)</param>
    /// <param name="configureOdd">Configuration for odd-numbered rows (0-based)</param>
    public TableBuilder AlternateRows(
        Action<StyleBuilder> configureEven,
        Action<StyleBuilder> configureOdd
    )
    {
        var evenBuilder = new StyleBuilder();
        configureEven(evenBuilder);
        var evenStyle = evenBuilder.Build();

        var oddBuilder = new StyleBuilder();
        configureOdd(oddBuilder);
        var oddStyle = oddBuilder.Build();

        return AlternateRows(evenStyle, oddStyle);
    }

    /// <summary>
    /// Applies a style configuration to the entire table.
    /// </summary>
    /// <param name="configure">The style configuration action</param>
    public TableBuilder Style(Action<StyleBuilder> configure)
    {
        var styleBuilder = new StyleBuilder();
        configure(styleBuilder);
        _style = _style.Merge(styleBuilder.Build());
        return this;
    }

    /// <summary>
    /// Builds the table model and creates a Table instance.
    /// </summary>
    /// <returns>A Table instance ready for rendering</returns>
    public Table Build()
    {
        var model = new TableModel
        {
            Header = _header,
            Body = _body,
            Footer = _footer,
            Style = _style,
            MaxWidth = _maxWidth,
            Columns = _columns,
        };

        return new Table(model);
    }

    /// <summary>
    /// Implicit conversion to Table.
    /// </summary>
    public static implicit operator Table(TableBuilder builder)
    {
        return builder.Build();
    }
}
