namespace SixLabors.ImageSharp.TableGenerator.Models;

/// <summary>
/// Represents padding values for all sides of a cell.
/// </summary>
public record Padding(float Left, float Top, float Right, float Bottom)
{
    /// <summary>
    /// Creates uniform padding for all sides.
    /// </summary>
    /// <param name="horizontal">Left and right padding</param>
    /// <param name="vertical">Top and bottom padding</param>
    /// <returns>A new Padding instance</returns>
    public static Padding All(float horizontal, float vertical) =>
        new(horizontal, vertical, horizontal, vertical);

    /// <summary>
    /// Creates uniform padding for all sides.
    /// </summary>
    /// <param name="value">Padding value for all sides</param>
    /// <returns>A new Padding instance</returns>
    public static Padding All(float value) => new(value, value, value, value);

    /// <summary>
    /// No padding.
    /// </summary>
    public static readonly Padding None = new(0, 0, 0, 0);
}
