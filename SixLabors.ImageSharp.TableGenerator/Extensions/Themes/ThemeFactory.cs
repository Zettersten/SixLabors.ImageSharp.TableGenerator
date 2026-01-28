using SixLabors.ImageSharp;

namespace BlazorFast.ImageSharp.TableGenerator.Extensions.Themes;

/// <summary>
/// Factory for creating table themes.
/// </summary>
internal static class ThemeFactory
{
    /// <summary>
    /// Creates a theme based on the specified mode.
    /// </summary>
    /// <param name="mode">The theme mode</param>
    /// <returns>A theme instance</returns>
    public static ITableTheme Create(ThemeMode mode)
    {
        return mode switch
        {
            ThemeMode.Light => new LightTheme(),
            ThemeMode.Dark => new DarkTheme(),
            ThemeMode.Minimal => new MinimalTheme(),
            ThemeMode.Compact => new CompactTheme(),
            _ => new LightTheme(),
        };
    }
}
