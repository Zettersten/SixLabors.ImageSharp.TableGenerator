namespace BlazorFast.ImageSharp.TableGenerator.Layout;

/// <summary>
/// Contains the computed layout data for a table after measurement.
/// </summary>
internal class MeasuredGrid
{
    public float[] ColumnWidths { get; }
    public float[] RowHeights { get; }
    public GridCell[] GridCells { get; }
    public int ColumnCount { get; }
    public int RowCount { get; }

    public MeasuredGrid(float[] columnWidths, float[] rowHeights, GridCell[] gridCells)
    {
        ColumnWidths = columnWidths;
        RowHeights = rowHeights;
        GridCells = gridCells;
        ColumnCount = columnWidths.Length;
        RowCount = rowHeights.Length;
    }

    /// <summary>
    /// Gets the total width of the table.
    /// </summary>
    public float TotalWidth => ColumnWidths.Sum();

    /// <summary>
    /// Gets the total height of the table.
    /// </summary>
    public float TotalHeight => RowHeights.Sum();

    /// <summary>
    /// Gets the X coordinate for the specified column.
    /// </summary>
    /// <param name="columnIndex">The column index</param>
    /// <returns>The X coordinate</returns>
    public float GetColumnX(int columnIndex)
    {
        return ColumnWidths.Take(columnIndex).Sum();
    }

    /// <summary>
    /// Gets the Y coordinate for the specified row.
    /// </summary>
    /// <param name="rowIndex">The row index</param>
    /// <returns>The Y coordinate</returns>
    public float GetRowY(int rowIndex)
    {
        return RowHeights.Take(rowIndex).Sum();
    }

    /// <summary>
    /// Gets the width of the specified column span.
    /// </summary>
    /// <param name="startColumn">The starting column index</param>
    /// <param name="columnSpan">The number of columns</param>
    /// <returns>The total width</returns>
    public float GetSpanWidth(int startColumn, int columnSpan)
    {
        return ColumnWidths.Skip(startColumn).Take(columnSpan).Sum();
    }

    /// <summary>
    /// Gets the height of the specified row span.
    /// </summary>
    /// <param name="startRow">The starting row index</param>
    /// <param name="rowSpan">The number of rows</param>
    /// <returns>The total height</returns>
    public float GetSpanHeight(int startRow, int rowSpan)
    {
        return RowHeights.Skip(startRow).Take(rowSpan).Sum();
    }
}
