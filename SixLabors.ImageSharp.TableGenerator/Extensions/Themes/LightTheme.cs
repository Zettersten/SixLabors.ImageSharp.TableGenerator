using BlazorFast.ImageSharp.TableGenerator.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace BlazorFast.ImageSharp.TableGenerator.Extensions.Themes;

/// <summary>
/// Light theme with white background and dark text.
/// </summary>
public class LightTheme : ITableTheme
{
    public TableStyle TableStyle { get; } =
        new()
        {
            Background = Color.White,
            TextColor = Color.Black,
            BorderColor = Color.ParseHex("#E0E0E0"),
            BorderWidth = 1f,
            CellPadding = Padding.All(8f),
            HAlign = HAlign.Left,
            VAlign = VAlign.Top,
            FontFamily = "Arial",
            FontSize = 12f,
            FontStyle = FontStyle.Regular,
        };

    public TableStyle HeaderStyle { get; } =
        new()
        {
            Background = Color.ParseHex("#F5F5F5"),
            TextColor = Color.Black,
            FontStyle = FontStyle.Bold,
            CellPadding = Padding.All(10f),
        };

    public TableStyle RowStyle { get; } = new() { };

    public TableStyle AlternatingRowStyle { get; } =
        new() { Background = Color.ParseHex("#FAFAFA") };
}
