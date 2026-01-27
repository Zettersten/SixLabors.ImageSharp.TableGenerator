using SixLabors.Fonts;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Extensions.Themes;

/// <summary>
/// Compact theme with reduced padding and smaller font.
/// </summary>
public class CompactTheme : ITableTheme
{
    public TableStyle TableStyle { get; } =
        new()
        {
            Background = Color.White,
            TextColor = Color.Black,
            BorderColor = Color.ParseHex("#CCCCCC"),
            BorderWidth = 1f,
            CellPadding = Padding.All(4f),
            HAlign = HAlign.Left,
            VAlign = VAlign.Top,
            FontFamily = "Arial",
            FontSize = 10f,
            FontStyle = FontStyle.Regular,
        };

    public TableStyle HeaderStyle { get; } =
        new()
        {
            Background = Color.ParseHex("#F0F0F0"),
            TextColor = Color.Black,
            FontStyle = FontStyle.Bold,
            CellPadding = Padding.All(6f, 4f),
        };

    public TableStyle RowStyle { get; } = new() { };

    public TableStyle AlternatingRowStyle { get; } =
        new() { Background = Color.ParseHex("#F8F8F8") };
}
