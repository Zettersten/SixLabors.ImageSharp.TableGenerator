using SixLabors.ImageSharp;

namespace SixLabors.ImageSharp.TableGenerator.Utils;

/// <summary>
/// Utility for parsing hex color strings.
/// </summary>
public static class ColorParser
{
    /// <summary>
    /// Parses a hex color string into a Color object.
    /// </summary>
    /// <param name="hex">The hex color string (supports #RGB, #RRGGBB, #RRGGBBAA formats)</param>
    /// <returns>The parsed color</returns>
    /// <exception cref="ArgumentException">Thrown when the hex string format is invalid</exception>
    public static Color ParseHex(string hex)
    {
        if (string.IsNullOrEmpty(hex))
            throw new ArgumentException("Hex color string cannot be null or empty", nameof(hex));

        // Remove # prefix if present
        if (hex.StartsWith('#'))
            hex = hex[1..];

        // Validate hex characters
        if (!hex.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
            throw new ArgumentException($"Invalid hex color format: #{hex}", nameof(hex));

        return hex.Length switch
        {
            3 => ParseRgb(hex), // #RGB
            6 => ParseRgba(hex + "FF"), // #RRGGBB -> #RRGGBBFF
            8 => ParseRgba(hex), // #RRGGBBAA
            _ => throw new ArgumentException(
                $"Invalid hex color length: #{hex}. Expected 3, 6, or 8 characters.",
                nameof(hex)
            ),
        };
    }

    private static Color ParseRgb(string rgb)
    {
        // Convert #RGB to #RRGGBB by duplicating each character
        var expanded = string.Concat(rgb.SelectMany(c => new[] { c, c }));
        return ParseRgba(expanded + "FF");
    }

    private static Color ParseRgba(string rgba)
    {
        var r = Convert.ToByte(rgba[0..2], 16);
        var g = Convert.ToByte(rgba[2..4], 16);
        var b = Convert.ToByte(rgba[4..6], 16);
        var a = Convert.ToByte(rgba[6..8], 16);

        return Color.FromRgba(r, g, b, a);
    }
}
