using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using SixLabors.Fonts;

namespace BlazorFast.ImageSharp.TableGenerator.Rendering;

/// <summary>
/// Cache for loaded fonts to avoid repeated file I/O operations.
/// </summary>
internal static class FontCache
{
    private static readonly ConcurrentDictionary<FontKey, Font> _cache = new();

    /// <summary>
    /// Gets or creates a font from the cache.
    /// </summary>
    /// <param name="family">Font family name</param>
    /// <param name="size">Font size</param>
    /// <param name="style">Font style</param>
    /// <returns>The cached or newly created font</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Font GetFont(string family, float size, FontStyle style)
    {
        var key = new FontKey(family, size, style);
        return _cache.GetOrAdd(key, k => CreateFont(k.Family, k.Size, k.Style));
    }

    private static Font CreateFont(string family, float size, FontStyle style)
    {
        try
        {
            return SystemFonts.CreateFont(family, size, style);
        }
        catch
        {
            // Try common fallback fonts for different platforms
            string[] fallbackFonts = new[]
            {
                "Arial", // Windows, macOS (with MS Office)
                "DejaVu Sans", // Linux (common)
                "Liberation Sans", // Linux (common)
                "FreeSans", // Linux fallback
                "Helvetica", // macOS
                "Segoe UI", // Windows
            };

            foreach (var fallback in fallbackFonts)
            {
                try
                {
                    return SystemFonts.CreateFont(fallback, size, style);
                }
                catch
                {
                    // Continue to next fallback
                }
            }

            // Final fallback to any available system font
            return SystemFonts.CreateFont(SystemFonts.Families.First().Name, size, style);
        }
    }

    /// <summary>
    /// Clears the font cache.
    /// </summary>
    public static void Clear()
    {
        _cache.Clear();
    }

    private readonly record struct FontKey(string Family, float Size, FontStyle Style);
}
