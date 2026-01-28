using BlazorFast.ImageSharp.TableGenerator.Layout;
using BlazorFast.ImageSharp.TableGenerator.Models;
using BlazorFast.ImageSharp.TableGenerator.Rendering;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BlazorFast.ImageSharp.TableGenerator;

/// <summary>
/// Represents a table that can be rendered to an image.
/// </summary>
public class Table
{
    private readonly TableModel _model;

    /// <summary>
    /// Creates a new table instance.
    /// </summary>
    /// <param name="model">The table model</param>
    internal Table(TableModel model)
    {
        _model = model;
    }

    /// <summary>
    /// Renders the table to an image.
    /// </summary>
    /// <param name="options">Rendering options</param>
    /// <returns>The rendered table image</returns>
    public Image<Rgba32> Render(RenderOptions? options = null)
    {
        options ??= RenderOptions.Default;

        // 1. Measure the table layout
        var measuredGrid = LayoutEngine.MeasureTable(_model);

        if (measuredGrid.GridCells.Length == 0)
        {
            // Empty table - return a minimal image
            return new Image<Rgba32>(1, 1, options.Background);
        }

        // 2. Calculate total image dimensions including margins
        var tableWidth = measuredGrid.TotalWidth;
        var tableHeight = measuredGrid.TotalHeight;
        var imageWidth = (int)Math.Ceiling(tableWidth + options.Margin.Left + options.Margin.Right);
        var imageHeight = (int)
            Math.Ceiling(tableHeight + options.Margin.Top + options.Margin.Bottom);

        // 3. Create the image
        var image = new Image<Rgba32>(imageWidth, imageHeight, options.Background);

        // 4. Render each cell
        foreach (var gridCell in measuredGrid.GridCells)
        {
            RenderCell(image, gridCell, measuredGrid, options);
        }

        return image;
    }

    private void RenderCell(
        Image<Rgba32> image,
        GridCell gridCell,
        MeasuredGrid measuredGrid,
        RenderOptions options
    )
    {
        // Get effective style for this cell
        var effectiveStyle = GetEffectiveStyle(gridCell);

        // Calculate cell bounds
        var x = options.Margin.Left + measuredGrid.GetColumnX(gridCell.ColIndex);
        var y = options.Margin.Top + measuredGrid.GetRowY(gridCell.RowIndex);
        var width = measuredGrid.GetSpanWidth(gridCell.ColIndex, gridCell.ColSpan);
        var height = measuredGrid.GetSpanHeight(gridCell.RowIndex, gridCell.RowSpan);

        var cellBounds = new RectangleF(x, y, width, height);

        // 1. Draw cell background
        if (effectiveStyle.Background.HasValue)
        {
            image.Mutate(ctx => ctx.Fill(effectiveStyle.Background.Value, cellBounds));
        }

        // 2. Draw borders
        DrawCellBorders(image, cellBounds, effectiveStyle);

        // 3. Draw text content
        if (!string.IsNullOrEmpty(gridCell.Cell.Text))
        {
            DrawCellText(image, gridCell, cellBounds, effectiveStyle, measuredGrid);
        }
    }

    private void DrawCellBorders(Image<Rgba32> image, RectangleF bounds, TableStyle style)
    {
        var borderColor = style.BorderColor ?? Color.Black;

        var topWidth = style.BorderTop ?? style.BorderWidth ?? 0f;
        var rightWidth = style.BorderRight ?? style.BorderWidth ?? 0f;
        var bottomWidth = style.BorderBottom ?? style.BorderWidth ?? 0f;
        var leftWidth = style.BorderLeft ?? style.BorderWidth ?? 0f;

        if (topWidth > 0 || rightWidth > 0 || bottomWidth > 0 || leftWidth > 0)
        {
            BorderRenderer.DrawBorder(
                image,
                bounds,
                borderColor,
                topWidth,
                rightWidth,
                bottomWidth,
                leftWidth
            );
        }
    }

    private void DrawCellText(
        Image<Rgba32> image,
        GridCell gridCell,
        RectangleF cellBounds,
        TableStyle style,
        MeasuredGrid measuredGrid
    )
    {
        var textColor = style.TextColor ?? Color.Black;
        var padding = style.CellPadding ?? Padding.None;

        // Get font
        var fontFamily = style.FontFamily ?? "Arial";
        var fontSize = style.FontSize ?? 12f;
        var fontStyle = style.FontStyle ?? SixLabors.Fonts.FontStyle.Regular;
        var font = FontCache.GetFont(fontFamily, fontSize, fontStyle);

        // Calculate text bounds (subtract padding)
        var textBounds = new RectangleF(
            cellBounds.X + padding.Left,
            cellBounds.Y + padding.Top,
            cellBounds.Width - padding.Left - padding.Right,
            cellBounds.Height - padding.Top - padding.Bottom
        );

        if (textBounds.Width <= 0 || textBounds.Height <= 0)
            return;

        // Wrap text
        var lines = TextWrapper.WrapText(gridCell.Cell.Text, textBounds.Width, font);

        if (lines.Count == 0)
            return;

        // Calculate line height
        var lineHeight = TextMeasurer.MeasureSize("Ag", new TextOptions(font)).Height;
        var totalTextHeight = lineHeight * lines.Count;

        // Calculate vertical alignment offset
        var vAlign = style.VAlign ?? Models.VAlign.Top;
        var verticalOffset = vAlign switch
        {
            Models.VAlign.Top => 0f,
            Models.VAlign.Middle => (textBounds.Height - totalTextHeight) / 2f,
            Models.VAlign.Bottom => textBounds.Height - totalTextHeight,
            _ => 0f,
        };

        // Draw each line
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line))
                continue;

            var lineY = textBounds.Y + verticalOffset + (i * lineHeight);

            // Calculate horizontal alignment
            var hAlign = style.HAlign ?? Models.HAlign.Left;
            var lineX = hAlign switch
            {
                Models.HAlign.Left => textBounds.X,
                Models.HAlign.Center => textBounds.X
                    + (
                        textBounds.Width
                        - TextMeasurer.MeasureSize(line, new TextOptions(font)).Width
                    ) / 2f,
                Models.HAlign.Right => textBounds.X
                    + textBounds.Width
                    - TextMeasurer.MeasureSize(line, new TextOptions(font)).Width,
                _ => textBounds.X,
            };

            // Draw the text line
            image.Mutate(ctx => ctx.DrawText(line, font, textColor, new PointF(lineX, lineY)));
        }
    }

    private TableStyle GetEffectiveStyle(GridCell gridCell)
    {
        // Start with table style
        var effectiveStyle = _model.Style;

        // Find the section this cell belongs to and merge styles
        var currentRow = 0;
        foreach (var section in _model.GetAllSections())
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
}
