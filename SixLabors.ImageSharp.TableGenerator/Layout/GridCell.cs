using BlazorFast.ImageSharp.TableGenerator.Models;

namespace BlazorFast.ImageSharp.TableGenerator.Layout;

/// <summary>
/// Internal struct for tracking cell positions and spans in the table grid.
/// </summary>
internal struct GridCell
{
    public int RowIndex { get; }
    public int ColIndex { get; }
    public int RowSpan { get; }
    public int ColSpan { get; }
    public TableCell Cell { get; }

    public GridCell(int rowIndex, int colIndex, int rowSpan, int colSpan, TableCell cell)
    {
        RowIndex = rowIndex;
        ColIndex = colIndex;
        RowSpan = rowSpan;
        ColSpan = colSpan;
        Cell = cell;
    }

    /// <summary>
    /// Gets the last row index this cell occupies (inclusive).
    /// </summary>
    public int LastRowIndex => RowIndex + RowSpan - 1;

    /// <summary>
    /// Gets the last column index this cell occupies (inclusive).
    /// </summary>
    public int LastColIndex => ColIndex + ColSpan - 1;

    /// <summary>
    /// Checks if this cell occupies the specified grid position.
    /// </summary>
    /// <param name="row">The row index</param>
    /// <param name="col">The column index</param>
    /// <returns>True if the cell occupies this position</returns>
    public bool OccupiesPosition(int row, int col)
    {
        return row >= RowIndex && row <= LastRowIndex && col >= ColIndex && col <= LastColIndex;
    }
}
