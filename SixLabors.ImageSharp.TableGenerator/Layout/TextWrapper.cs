using SixLabors.Fonts;

namespace SixLabors.ImageSharp.TableGenerator.Layout;

/// <summary>
/// Utility for wrapping text to fit within specified width constraints.
/// </summary>
internal static class TextWrapper
{
    /// <summary>
    /// Wraps text to fit within the specified maximum width.
    /// </summary>
    /// <param name="text">The text to wrap</param>
    /// <param name="maxWidth">The maximum width in pixels</param>
    /// <param name="font">The font to use for measurement</param>
    /// <returns>A list of text lines</returns>
    public static List<string> WrapText(string text, float maxWidth, Font font)
    {
        if (string.IsNullOrEmpty(text))
            return new List<string> { string.Empty };

        if (maxWidth <= 0)
            return new List<string> { text };

        var lines = new List<string>();
        var paragraphs = text.Split(new[] { '\n', '\r' }, StringSplitOptions.None);

        foreach (var paragraph in paragraphs)
        {
            if (string.IsNullOrEmpty(paragraph))
            {
                lines.Add(string.Empty);
                continue;
            }

            lines.AddRange(WrapParagraph(paragraph, maxWidth, font));
        }

        return lines;
    }

    private static List<string> WrapParagraph(string paragraph, float maxWidth, Font font)
    {
        var lines = new List<string>();
        var words = paragraph.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (words.Length == 0)
        {
            lines.Add(string.Empty);
            return lines;
        }

        var currentLine = string.Empty;

        foreach (var word in words)
        {
            var testLine = string.IsNullOrEmpty(currentLine) ? word : $"{currentLine} {word}";
            var testWidth = TextMeasurer.MeasureSize(testLine, new TextOptions(font)).Width;

            if (testWidth <= maxWidth)
            {
                currentLine = testLine;
            }
            else
            {
                // If current line is not empty, add it and start new line
                if (!string.IsNullOrEmpty(currentLine))
                {
                    lines.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    // Single word is too long, try to break it
                    var brokenWords = BreakLongWord(word, maxWidth, font);
                    lines.AddRange(brokenWords.Take(brokenWords.Count - 1));
                    currentLine = brokenWords.Last();
                }

                // Check if the new current line still doesn't fit
                var currentWidth = TextMeasurer
                    .MeasureSize(currentLine, new TextOptions(font))
                    .Width;
                if (currentWidth > maxWidth && currentLine.Length > 1)
                {
                    var brokenWords = BreakLongWord(currentLine, maxWidth, font);
                    lines.AddRange(brokenWords.Take(brokenWords.Count - 1));
                    currentLine = brokenWords.Last();
                }
            }
        }

        // Add the last line if not empty
        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return lines;
    }

    private static List<string> BreakLongWord(string word, float maxWidth, Font font)
    {
        var lines = new List<string>();
        var currentLine = string.Empty;

        foreach (var character in word)
        {
            var testLine = currentLine + character;
            var testWidth = TextMeasurer.MeasureSize(testLine, new TextOptions(font)).Width;

            if (testWidth <= maxWidth)
            {
                currentLine = testLine;
            }
            else
            {
                if (!string.IsNullOrEmpty(currentLine))
                {
                    lines.Add(currentLine);
                    currentLine = character.ToString();
                }
                else
                {
                    // Even a single character doesn't fit, add it anyway
                    lines.Add(character.ToString());
                    currentLine = string.Empty;
                }
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }

        return lines.Count == 0 ? new List<string> { word } : lines;
    }

    /// <summary>
    /// Measures the height of wrapped text.
    /// </summary>
    /// <param name="text">The text to measure</param>
    /// <param name="maxWidth">The maximum width for wrapping</param>
    /// <param name="font">The font to use</param>
    /// <returns>The height of the wrapped text</returns>
    public static float MeasureWrappedHeight(string text, float maxWidth, Font font)
    {
        var lines = WrapText(text, maxWidth, font);
        if (lines.Count == 0)
            return 0f;

        // Use the height of a single line and multiply by line count
        var singleLineHeight = TextMeasurer.MeasureSize("Ag", new TextOptions(font)).Height;
        return singleLineHeight * lines.Count;
    }
}
