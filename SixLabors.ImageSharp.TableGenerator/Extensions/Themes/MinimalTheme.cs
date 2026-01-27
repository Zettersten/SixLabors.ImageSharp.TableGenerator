using SixLabors.Fonts;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Extensions.Themes;

/// <summary>
/// Minimal theme with no borders and clean spacing.
/// </summary>
public class MinimalTheme : ITableTheme
{
    public TableStyle TableStyle { get; } =
        new()
        {
            Background = Color.White,
            TextColor = Color.Black,
            BorderWidth = 0f,
            CellPadding = Padding.All(12f, 8f),
            HAlign = HAlign.Left,
            VAlign = VAlign.Top,
            FontFamily = "Arial",
            FontSize = 12f,
            FontStyle = FontStyle.Regular,
        };

    public TableStyle HeaderStyle { get; } =
        new()
        {
            TextColor = Color.ParseHex("#666666"),
            FontStyle = FontStyle.Bold,
            CellPadding = Padding.All(12f, 12f),
            BorderBottom = 2f,
            BorderColor = Color.ParseHex("#E0E0E0"),
        };

    public TableStyle RowStyle { get; } = new() { };

    public TableStyle AlternatingRowStyle { get; } = new() { };
}
