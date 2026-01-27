using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Builders;

/// <summary>
/// Fluent builder for configuring table columns.
/// </summary>
public class ColumnsBuilder
{
    private readonly List<ColumnSpec> _columns = new();

    /// <summary>
    /// Adds an auto-sizing column that fits to content.
    /// </summary>
    public ColumnsBuilder Auto()
    {
        _columns.Add(ColumnSpec.Auto());
        return this;
    }

    /// <summary>
    /// Adds a fixed-width column.
    /// </summary>
    /// <param name="width">The column width in pixels</param>
    public ColumnsBuilder Fixed(float width)
    {
        _columns.Add(ColumnSpec.Fixed(width));
        return this;
    }

    /// <summary>
    /// Builds the column specifications list.
    /// </summary>
    /// <returns>The list of column specifications</returns>
    public List<ColumnSpec> Build()
    {
        return new List<ColumnSpec>(_columns);
    }

    /// <summary>
    /// Implicit conversion to List of ColumnSpec.
    /// </summary>
    public static implicit operator List<ColumnSpec>(ColumnsBuilder builder)
    {
        return builder.Build();
    }
}
