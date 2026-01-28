namespace BlazorFast.ImageSharp.TableGenerator.Models;

/// <summary>
/// Abstract base record for column width specifications.
/// </summary>
public abstract record ColumnSpec
{
    /// <summary>
    /// Creates an auto-sizing column that fits to content.
    /// </summary>
    public static ColumnSpec Auto() => new AutoColumn();

    /// <summary>
    /// Creates a fixed-width column.
    /// </summary>
    /// <param name="width">The fixed width in pixels</param>
    public static ColumnSpec Fixed(float width) => new FixedColumn(width);
}

/// <summary>
/// Auto-sizing column that fits to content.
/// </summary>
public record AutoColumn : ColumnSpec;

/// <summary>
/// Fixed-width column.
/// </summary>
/// <param name="Width">The width in pixels</param>
public record FixedColumn(float Width) : ColumnSpec;
