using SixLabors.Fonts;
using SixLabors.ImageSharp.TableGenerator.Models;
using SixLabors.ImageSharp.TableGenerator.Rendering;

namespace SixLabors.ImageSharp.TableGenerator.Layout;

/// <summary>
/// Engine for calculating table layout and measurements.
/// </summary>
internal static class LayoutEngine
{
    /// <summary>
    /// Measures the table and returns layout information.
    /// </summary>
    /// <param name="tableModel">The table model to measure</param>
    /// <returns>Measured grid with layout data</returns>
    public static MeasuredGrid MeasureTable(TableModel tableModel)
    {
        // 1. Flatten all sections into a list of rows
        var allRows = tableModel.GetAllRows().ToList();
        if (allRows.Count == 0)
        {
            return new MeasuredGrid(new float[0], new float[0], new GridCell[0]);
        }

        // 2. Resolve grid positions accounting for spans
        var gridCells = ResolveGridPositions(allRows);

        if (gridCells.Count == 0)
        {
            return new MeasuredGrid(new float[0], new float[0], new GridCell[0]);
        }

        // 3. Determine the number of columns and rows
        var maxColumn = gridCells.Max(gc => gc.LastColIndex);
        var maxRow = gridCells.Max(gc => gc.LastRowIndex);
        var columnCount = Math.Max(maxColumn + 1, tableModel.Columns.Count);
        var rowCount = maxRow + 1;

        // 4. Measure column widths
        var columnWidths = MeasureColumnWidths(gridCells, tableModel, columnCount);

        // 5. Apply max width constraint if specified
        if (tableModel.MaxWidth.HasValue && columnWidths.Sum() > tableModel.MaxWidth.Value)
        {
            columnWidths = ApplyMaxWidthConstraint(
                columnWidths,
                tableModel.MaxWidth.Value,
                tableModel.Columns
            );
        }

        // 6. Measure row heights based on column widths
        var rowHeights = MeasureRowHeights(gridCells, columnWidths, tableModel, rowCount);

        return new MeasuredGrid(columnWidths, rowHeights, gridCells.ToArray());
    }

    private static List<GridCell> ResolveGridPositions(List<TableRow> rows)
    {
        var gridCells = new List<GridCell>();
        var occupiedCells = new Dictionary<(int row, int col), bool>();

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            var row = rows[rowIndex];
            var colIndex = 0;

            foreach (var cell in row.Cells)
            {
                // Find the next available column position
                while (occupiedCells.ContainsKey((rowIndex, colIndex)))
                {
                    colIndex++;
                }

                // Create grid cell
                var gridCell = new GridCell(rowIndex, colIndex, cell.RowSpan, cell.ColSpan, cell);
                gridCells.Add(gridCell);

                // Mark all positions occupied by this cell
                for (var r = rowIndex; r <= gridCell.LastRowIndex; r++)
                {
                    for (var c = colIndex; c <= gridCell.LastColIndex; c++)
                    {
                        occupiedCells[(r, c)] = true;
                    }
                }

                colIndex += cell.ColSpan;
            }
        }

        return gridCells;
    }

    private static float[] MeasureColumnWidths(
        List<GridCell> gridCells,
        TableModel tableModel,
        int columnCount
    )
    {
        var columnWidths = new float[columnCount];

        // Initialize with fixed widths from column specs
        for (var i = 0; i < Math.Min(tableModel.Columns.Count, columnCount); i++)
        {
            if (tableModel.Columns[i] is FixedColumn fixedCol)
            {
                columnWidths[i] = fixedCol.Width;
            }
        }

        // Measure content widths for auto columns
        foreach (var gridCell in gridCells)
        {
            if (gridCell.ColSpan == 1) // Only consider non-spanning cells for auto width
            {
                var colIndex = gridCell.ColIndex;
                if (
                    colIndex < tableModel.Columns.Count
                    && tableModel.Columns[colIndex] is AutoColumn
                )
                {
                    var contentWidth = MeasureCellContentWidth(gridCell, tableModel);
                    columnWidths[colIndex] = Math.Max(columnWidths[colIndex], contentWidth);
                }
                else if (colIndex >= tableModel.Columns.Count) // No column spec, assume auto
                {
                    var contentWidth = MeasureCellContentWidth(gridCell, tableModel);
                    columnWidths[colIndex] = Math.Max(columnWidths[colIndex], contentWidth);
                }
            }
        }

        return columnWidths;
    }

    private static float MeasureCellContentWidth(GridCell gridCell, TableModel tableModel)
    {
        var cell = gridCell.Cell;

        if (string.IsNullOrEmpty(cell.Text))
            return 0f;

        // Get effective style
        var effectiveStyle = GetEffectiveStyle(gridCell, tableModel);

        // Get font
        var font = GetFont(effectiveStyle);

        // Measure text width
        var textWidth = TextMeasurer.MeasureSize(cell.Text, new TextOptions(font)).Width;

        // Add padding
        var padding = effectiveStyle.CellPadding ?? Padding.None;
        return textWidth + padding.Left + padding.Right;
    }

    private static float[] ApplyMaxWidthConstraint(
        float[] columnWidths,
        float maxWidth,
        List<ColumnSpec> columnSpecs
    )
    {
        var totalWidth = columnWidths.Sum();
        if (totalWidth <= maxWidth)
            return columnWidths;

        var result = new float[columnWidths.Length];
        var fixedTotalWidth = 0f;
        var autoIndices = new List<int>();

        // Calculate total width of fixed columns
        for (var i = 0; i < Math.Min(columnSpecs.Count, columnWidths.Length); i++)
        {
            if (columnSpecs[i] is FixedColumn)
            {
                result[i] = columnWidths[i];
                fixedTotalWidth += columnWidths[i];
            }
            else
            {
                autoIndices.Add(i);
            }
        }

        // Add remaining columns as auto
        for (var i = columnSpecs.Count; i < columnWidths.Length; i++)
        {
            autoIndices.Add(i);
        }

        // Distribute remaining width among auto columns proportionally
        var remainingWidth = maxWidth - fixedTotalWidth;
        var autoTotalWidth = autoIndices.Sum(i => columnWidths[i]);

        if (autoTotalWidth > 0 && remainingWidth > 0)
        {
            foreach (var index in autoIndices)
            {
                var proportion = columnWidths[index] / autoTotalWidth;
                result[index] = remainingWidth * proportion;
            }
        }

        return result;
    }

    private static float[] MeasureRowHeights(
        List<GridCell> gridCells,
        float[] columnWidths,
        TableModel tableModel,
        int rowCount
    )
    {
        var rowHeights = new float[rowCount];

        foreach (var gridCell in gridCells)
        {
            if (gridCell.RowSpan == 1) // Only consider non-spanning cells initially
            {
                var cellHeight = MeasureCellHeight(gridCell, columnWidths, tableModel);
                rowHeights[gridCell.RowIndex] = Math.Max(rowHeights[gridCell.RowIndex], cellHeight);
            }
        }

        // Handle row-spanning cells (distribute height among spanned rows)
        foreach (var gridCell in gridCells.Where(gc => gc.RowSpan > 1))
        {
            var requiredHeight = MeasureCellHeight(gridCell, columnWidths, tableModel);
            var currentSpanHeight = 0f;

            for (var r = gridCell.RowIndex; r <= gridCell.LastRowIndex; r++)
            {
                currentSpanHeight += rowHeights[r];
            }

            if (requiredHeight > currentSpanHeight)
            {
                // Distribute additional height evenly among spanned rows
                var additionalHeight = requiredHeight - currentSpanHeight;
                var heightPerRow = additionalHeight / gridCell.RowSpan;

                for (var r = gridCell.RowIndex; r <= gridCell.LastRowIndex; r++)
                {
                    rowHeights[r] += heightPerRow;
                }
            }
        }

        return rowHeights;
    }

    private static float MeasureCellHeight(
        GridCell gridCell,
        float[] columnWidths,
        TableModel tableModel
    )
    {
        var cell = gridCell.Cell;

        if (string.IsNullOrEmpty(cell.Text))
            return 0f;

        // Get effective style
        var effectiveStyle = GetEffectiveStyle(gridCell, tableModel);

        // Get font
        var font = GetFont(effectiveStyle);

        // Calculate available width for text
        var availableWidth = 0f;
        for (var c = gridCell.ColIndex; c <= gridCell.LastColIndex; c++)
        {
            availableWidth += columnWidths[c];
        }

        // Subtract padding
        var padding = effectiveStyle.CellPadding ?? Padding.None;
        availableWidth -= (padding.Left + padding.Right);

        // Measure wrapped text height
        var textHeight = TextWrapper.MeasureWrappedHeight(cell.Text, availableWidth, font);

        // Add padding
        return textHeight + padding.Top + padding.Bottom;
    }

    private static TableStyle GetEffectiveStyle(GridCell gridCell, TableModel tableModel)
    {
        // Start with table style
        var effectiveStyle = tableModel.Style;

        // Find the section this cell belongs to
        var currentRow = 0;
        foreach (var section in tableModel.GetAllSections())
        {
            if (
                gridCell.RowIndex >= currentRow
                && gridCell.RowIndex < currentRow + section.Rows.Count
            )
            {
                // Merge section style
                effectiveStyle = effectiveStyle.Merge(section.Style);

                // Find the row and merge row style
                var rowIndex = gridCell.RowIndex - currentRow;
                var row = section.Rows[rowIndex];
                effectiveStyle = effectiveStyle.Merge(row.Style);
                break;
            }
            currentRow += section.Rows.Count;
        }

        // Merge cell style
        effectiveStyle = effectiveStyle.Merge(gridCell.Cell.Style);

        return effectiveStyle;
    }

    private static Font GetFont(TableStyle style)
    {
        var fontFamily = style.FontFamily ?? "Arial";
        var fontSize = style.FontSize ?? 12f;
        var fontStyle = style.FontStyle ?? SixLabors.Fonts.FontStyle.Regular;

        return FontCache.GetFont(fontFamily, fontSize, fontStyle);
    }
}
