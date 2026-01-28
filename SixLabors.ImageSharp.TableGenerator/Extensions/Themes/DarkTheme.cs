using BlazorFast.ImageSharp.TableGenerator.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace BlazorFast.ImageSharp.TableGenerator.Extensions.Themes;

/// <summary>
/// Dark theme with dark background and light text.
/// </summary>
public class DarkTheme : ITableTheme
{
    public TableStyle TableStyle { get; } =
        new()
        {
            Background = Color.ParseHex("#1E1E1E"),
            TextColor = Color.ParseHex("#E0E0E0"),
            BorderColor = Color.ParseHex("#3E3E3E"),
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
            Background = Color.ParseHex("#2D2D2D"),
            TextColor = Color.ParseHex("#FFFFFF"),
            FontStyle = FontStyle.Bold,
            CellPadding = Padding.All(10f),
        };

    public TableStyle RowStyle { get; } = new() { };

    public TableStyle AlternatingRowStyle { get; } =
        new() { Background = Color.ParseHex("#252525") };
}
