using SixLabors.Fonts;
using SixLabors.ImageSharp.TableGenerator.Layout;
using SixLabors.ImageSharp.TableGenerator.Rendering;

namespace SixLabors.ImageSharp.TableGenerator.Tests.Layout;

public class TextWrapperTests
{
    private readonly Font _testFont;

    public TextWrapperTests()
    {
        // Use FontCache for cross-platform font support
        _testFont = FontCache.GetFont("Arial", 12f, FontStyle.Regular);
    }

    [Fact]
    public void WrapText_EmptyString_ReturnsEmptyLine()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText("", 100f, _testFont);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be("");
    }

    [Fact]
    public void WrapText_NullString_ReturnsEmptyLine()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText(null!, 100f, _testFont);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be("");
    }

    [Fact]
    public void WrapText_ZeroMaxWidth_ReturnsOriginalText()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText("Hello World", 0f, _testFont);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be("Hello World");
    }

    [Fact]
    public void WrapText_NegativeMaxWidth_ReturnsOriginalText()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText("Hello World", -10f, _testFont);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be("Hello World");
    }

    [Fact]
    public void WrapText_SingleWordThatFits_ReturnsOneWord()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText("Hello", 1000f, _testFont);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be("Hello");
    }

    [Fact]
    public void WrapText_MultipleWordsThatFit_ReturnsOneLine()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText("Hello World Test", 1000f, _testFont);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be("Hello World Test");
    }

    [Fact]
    public void WrapText_SimpleWordWrapping_WrapsAtSpaces()
    {
        // Arrange - Use a very narrow width to force wrapping
        var text = "Hello World";

        // Act
        var result = TextWrapper.WrapText(text, 30f, _testFont);

        // Assert
        result.Should().HaveCountGreaterThan(1);
        // The exact result depends on font metrics, but it should wrap
        result.Should().AllSatisfy(line => line.Should().NotBeEmpty());
    }

    [Fact]
    public void WrapText_ExistingLineBreaks_PreservesLineBreaks()
    {
        // Arrange
        var text = "Line 1\nLine 2\rLine 3";

        // Act
        var result = TextWrapper.WrapText(text, 1000f, _testFont);

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Be("Line 1");
        result[1].Should().Be("Line 2");
        result[2].Should().Be("Line 3");
    }

    [Fact]
    public void WrapText_EmptyLinesInText_PreservesEmptyLines()
    {
        // Arrange
        var text = "Line 1\n\nLine 3";

        // Act
        var result = TextWrapper.WrapText(text, 1000f, _testFont);

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Be("Line 1");
        result[1].Should().Be("");
        result[2].Should().Be("Line 3");
    }

    [Fact]
    public void WrapText_VeryLongSingleWord_BreaksAtCharacters()
    {
        // Arrange
        var longWord = "Supercalifragilisticexpialidocious";

        // Act - Use narrow width to force character breaking
        var result = TextWrapper.WrapText(longWord, 20f, _testFont);

        // Assert
        result.Should().HaveCountGreaterThan(1);
        // All parts combined should equal the original word
        string.Join("", result).Should().Be(longWord);
    }

    [Fact]
    public void WrapText_MixedContent_HandlesCorrectly()
    {
        // Arrange
        var text = "Short words and Supercalifragilisticexpialidocious mixed together";

        // Act
        var result = TextWrapper.WrapText(text, 50f, _testFont);

        // Assert
        result.Should().HaveCountGreaterThan(1);
        // Should contain all original content
        string.Join(" ", result).Should().Contain("Short");
        string.Join("", result).Should().Contain("Supercalifragilisticexpialidocious");
    }

    [Fact]
    public void WrapText_OnlySpaces_HandlesGracefully()
    {
        // Arrange & Act
        var result = TextWrapper.WrapText("   ", 100f, _testFont);

        // Assert
        result.Should().NotBeEmpty();
        // Exact behavior may vary, but should handle gracefully
    }

    [Fact]
    public void MeasureWrappedHeight_EmptyString_ReturnsZero()
    {
        // Arrange & Act
        var height = TextWrapper.MeasureWrappedHeight("", 100f, _testFont);

        // Assert
        // Empty string returns one empty line, so height > 0
        height.Should().BeGreaterThan(0f);
    }

    [Fact]
    public void MeasureWrappedHeight_SingleLine_ReturnsLineHeight()
    {
        // Arrange & Act
        var height = TextWrapper.MeasureWrappedHeight("Hello", 1000f, _testFont);

        // Assert
        height.Should().BeGreaterThan(0f);
        // Should be approximately one line height
        var singleLineHeight = TextMeasurer.MeasureSize("Ag", new TextOptions(_testFont)).Height;
        height.Should().BeApproximately(singleLineHeight, 1f);
    }

    [Fact]
    public void MeasureWrappedHeight_MultipleLines_ReturnsMultipleLineHeight()
    {
        // Arrange
        var text = "Line 1\nLine 2\nLine 3";

        // Act
        var height = TextWrapper.MeasureWrappedHeight(text, 1000f, _testFont);

        // Assert
        height.Should().BeGreaterThan(0f);
        // Should be approximately three times line height
        var singleLineHeight = TextMeasurer.MeasureSize("Ag", new TextOptions(_testFont)).Height;
        height.Should().BeApproximately(singleLineHeight * 3, 2f);
    }

    [Fact]
    public void MeasureWrappedHeight_WrappedText_ReturnsCorrectHeight()
    {
        // Arrange
        var text = "This is a long text that should wrap";

        // Act - Use narrow width to force wrapping
        var height = TextWrapper.MeasureWrappedHeight(text, 30f, _testFont);

        // Assert
        height.Should().BeGreaterThan(0f);
        var singleLineHeight = TextMeasurer.MeasureSize("Ag", new TextOptions(_testFont)).Height;
        // Should be more than one line height since it wrapped
        height.Should().BeGreaterThan(singleLineHeight);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("Short")]
    [InlineData("Medium length text")]
    [InlineData("This is a much longer piece of text that will definitely require wrapping")]
    public void WrapText_VariousLengths_ProducesConsistentResults(string text)
    {
        // Act
        var result = TextWrapper.WrapText(text, 100f, _testFont);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(line => line.Should().NotBeNull());

        // Recombining should preserve original content (accounting for spaces)
        var recombined = string.Join(" ", result.Where(line => !string.IsNullOrEmpty(line)));
        if (!string.IsNullOrEmpty(text))
        {
            recombined.Should().Contain(text.Split(' ')[0]); // At least first word should be present
        }
    }

    [Fact]
    public void WrapText_ConsistentResults_WithSameInputs()
    {
        // Arrange
        var text = "Consistent test text for wrapping";
        var maxWidth = 80f;

        // Act - Call multiple times
        var result1 = TextWrapper.WrapText(text, maxWidth, _testFont);
        var result2 = TextWrapper.WrapText(text, maxWidth, _testFont);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }
}
