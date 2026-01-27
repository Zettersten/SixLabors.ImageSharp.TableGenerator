using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Extensions.Themes;

/// <summary>
/// Defines a visual theme for table rendering.
/// </summary>
public interface ITableTheme
{
    /// <summary>
    /// Gets the base table style.
    /// </summary>
    TableStyle TableStyle { get; }

    /// <summary>
    /// Gets the header row style.
    /// </summary>
    TableStyle HeaderStyle { get; }

    /// <summary>
    /// Gets the body row style.
    /// </summary>
    TableStyle RowStyle { get; }

    /// <summary>
    /// Gets the alternating row style.
    /// </summary>
    TableStyle AlternatingRowStyle { get; }
}
