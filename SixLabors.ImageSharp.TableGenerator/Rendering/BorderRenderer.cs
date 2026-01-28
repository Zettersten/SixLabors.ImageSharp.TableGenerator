using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BlazorFast.ImageSharp.TableGenerator.Rendering;

/// <summary>
/// Helper for rendering table borders.
/// </summary>
internal static class BorderRenderer
{
    /// <summary>
    /// Draws a border around the specified rectangle.
    /// </summary>
    /// <param name="image">The image to draw on</param>
    /// <param name="bounds">The rectangle bounds</param>
    /// <param name="color">The border color</param>
    /// <param name="topWidth">Top border width</param>
    /// <param name="rightWidth">Right border width</param>
    /// <param name="bottomWidth">Bottom border width</param>
    /// <param name="leftWidth">Left border width</param>
    public static void DrawBorder<TPixel>(
        Image<TPixel> image,
        RectangleF bounds,
        Color color,
        float topWidth,
        float rightWidth,
        float bottomWidth,
        float leftWidth
    )
        where TPixel : unmanaged, IPixel<TPixel>
    {
        image.Mutate(ctx =>
        {
            // Draw top border
            if (topWidth > 0)
            {
                var topRect = new RectangleF(bounds.X, bounds.Y, bounds.Width, topWidth);
                ctx.Fill(color, topRect);
            }

            // Draw right border
            if (rightWidth > 0)
            {
                var rightRect = new RectangleF(
                    bounds.Right - rightWidth,
                    bounds.Y,
                    rightWidth,
                    bounds.Height
                );
                ctx.Fill(color, rightRect);
            }

            // Draw bottom border
            if (bottomWidth > 0)
            {
                var bottomRect = new RectangleF(
                    bounds.X,
                    bounds.Bottom - bottomWidth,
                    bounds.Width,
                    bottomWidth
                );
                ctx.Fill(color, bottomRect);
            }

            // Draw left border
            if (leftWidth > 0)
            {
                var leftRect = new RectangleF(bounds.X, bounds.Y, leftWidth, bounds.Height);
                ctx.Fill(color, leftRect);
            }
        });
    }

    /// <summary>
    /// Draws a uniform border around the specified rectangle.
    /// </summary>
    /// <param name="image">The image to draw on</param>
    /// <param name="bounds">The rectangle bounds</param>
    /// <param name="color">The border color</param>
    /// <param name="width">The border width</param>
    public static void DrawBorder<TPixel>(
        Image<TPixel> image,
        RectangleF bounds,
        Color color,
        float width
    )
        where TPixel : unmanaged, IPixel<TPixel>
    {
        DrawBorder(image, bounds, color, width, width, width, width);
    }
}
