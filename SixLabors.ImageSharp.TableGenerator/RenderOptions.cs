using SixLabors.ImageSharp;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator;

/// <summary>
/// Options for configuring table rendering.
/// </summary>
public record RenderOptions
{
    /// <summary>
    /// Background color for the image. Default is transparent.
    /// </summary>
    public Color Background { get; init; } = Color.Transparent;

    /// <summary>
    /// Margin around the table. Default is no margin.
    /// </summary>
    public Padding Margin { get; init; } = Padding.None;

    /// <summary>
    /// Default render options.
    /// </summary>
    public static readonly RenderOptions Default = new();
}
