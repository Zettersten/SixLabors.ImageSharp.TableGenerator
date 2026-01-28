using BlazorFast.ImageSharp.TableGenerator.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace BlazorFast.ImageSharp.TableGenerator.Tests.Utils;

public class ColorParserTests
{
    private static Rgba32 ToRgba32(Color color) => color.ToPixel<Rgba32>();

    [Theory]
    [InlineData("#F00", 255, 0, 0, 255)] // Red
    [InlineData("#0F0", 0, 255, 0, 255)] // Green
    [InlineData("#00F", 0, 0, 255, 255)] // Blue
    [InlineData("#FFF", 255, 255, 255, 255)] // White
    [InlineData("#000", 0, 0, 0, 255)] // Black
    [InlineData("#ABC", 170, 187, 204, 255)] // Light blue-gray
    public void ParseHex_RGB_Format_ParsesCorrectly(
        string hex,
        byte expectedR,
        byte expectedG,
        byte expectedB,
        byte expectedA
    )
    {
        // Act
        var result = ToRgba32(ColorParser.ParseHex(hex));

        // Assert
        result.R.Should().Be(expectedR);
        result.G.Should().Be(expectedG);
        result.B.Should().Be(expectedB);
        result.A.Should().Be(expectedA);
    }

    [Theory]
    [InlineData("#FF0000", 255, 0, 0, 255)] // Red
    [InlineData("#00FF00", 0, 255, 0, 255)] // Green
    [InlineData("#0000FF", 0, 0, 255, 255)] // Blue
    [InlineData("#FFFFFF", 255, 255, 255, 255)] // White
    [InlineData("#000000", 0, 0, 0, 255)] // Black
    [InlineData("#AABBCC", 170, 187, 204, 255)] // Light blue-gray
    [InlineData("#123456", 18, 52, 86, 255)] // Dark blue
    public void ParseHex_RRGGBB_Format_ParsesCorrectly(
        string hex,
        byte expectedR,
        byte expectedG,
        byte expectedB,
        byte expectedA
    )
    {
        // Act
        var result = ToRgba32(ColorParser.ParseHex(hex));

        // Assert
        result.R.Should().Be(expectedR);
        result.G.Should().Be(expectedG);
        result.B.Should().Be(expectedB);
        result.A.Should().Be(expectedA);
    }

    [Theory]
    [InlineData("#FF000080", 255, 0, 0, 128)] // Semi-transparent red
    [InlineData("#00FF0040", 0, 255, 0, 64)] // Semi-transparent green
    [InlineData("#0000FFFF", 0, 0, 255, 255)] // Fully opaque blue
    [InlineData("#FFFFFF00", 255, 255, 255, 0)] // Fully transparent white
    [InlineData("#AABBCC80", 170, 187, 204, 128)] // Semi-transparent gray
    public void ParseHex_RRGGBBAA_Format_ParsesCorrectly(
        string hex,
        byte expectedR,
        byte expectedG,
        byte expectedB,
        byte expectedA
    )
    {
        // Act
        var result = ToRgba32(ColorParser.ParseHex(hex));

        // Assert
        result.R.Should().Be(expectedR);
        result.G.Should().Be(expectedG);
        result.B.Should().Be(expectedB);
        result.A.Should().Be(expectedA);
    }

    [Theory]
    [InlineData("FF0000")] // Without #
    [InlineData("00FF00")] // Without #
    [InlineData("F00")] // Without #
    public void ParseHex_WithoutHashPrefix_ParsesCorrectly(string hex)
    {
        // Act
        var result = ColorParser.ParseHex(hex);

        // Assert
        result.Should().NotBe(default(Color));
    }

    [Theory]
    [InlineData("#ff0000")] // Lowercase
    [InlineData("#00Ff00")] // Mixed case
    [InlineData("#AABBCC")] // Uppercase
    [InlineData("#aabbcc")] // Lowercase
    public void ParseHex_CaseInsensitive_ParsesCorrectly(string hex)
    {
        // Act
        var result = ColorParser.ParseHex(hex);

        // Assert
        result.Should().NotBe(default(Color));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ParseHex_NullOrEmpty_ThrowsArgumentException(string? hex)
    {
        // Act & Assert
        var act = () => ColorParser.ParseHex(hex!);
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Hex color string cannot be null or empty*");
    }

    [Theory]
    [InlineData("#G00")] // Invalid character
    [InlineData("#GGGGGG")] // Invalid characters
    [InlineData("#FF00ZZ")] // Invalid characters
    [InlineData("#12345G")] // Invalid character
    public void ParseHex_InvalidHexCharacters_ThrowsArgumentException(string hex)
    {
        // Act & Assert
        var act = () => ColorParser.ParseHex(hex);
        act.Should()
            .Throw<ArgumentException>()
            .And.Message.Should()
            .Contain("Invalid hex color format");
    }

    [Theory]
    [InlineData("#F")] // Too short
    [InlineData("#FF")] // Too short
    [InlineData("#FFFF")] // Invalid length
    [InlineData("#FFFFF")] // Invalid length
    [InlineData("#FFFFFFF")] // Invalid length
    [InlineData("#FFFFFFFFF")] // Too long
    public void ParseHex_InvalidLength_ThrowsArgumentException(string hex)
    {
        // Act & Assert
        var act = () => ColorParser.ParseHex(hex);
        act.Should()
            .Throw<ArgumentException>()
            .And.Message.Should()
            .Contain("Invalid hex color length");
    }

    [Fact]
    public void ParseHex_KnownColors_ProduceExpectedResults()
    {
        // Arrange & Act
        var red = ColorParser.ParseHex("#FF0000");
        var green = ColorParser.ParseHex("#00FF00");
        var blue = ColorParser.ParseHex("#0000FF");
        var white = ColorParser.ParseHex("#FFFFFF");
        var black = ColorParser.ParseHex("#000000");

        // Assert
        red.Should().Be(Color.Red);
        green.Should().Be(Color.Lime); // Pure green is Lime in ImageSharp
        blue.Should().Be(Color.Blue);
        white.Should().Be(Color.White);
        black.Should().Be(Color.Black);
    }

    [Fact]
    public void ParseHex_TransparencyLevels_WorkCorrectly()
    {
        // Arrange & Act
        var fullyOpaque = ToRgba32(ColorParser.ParseHex("#FF0000FF"));
        var halfTransparent = ToRgba32(ColorParser.ParseHex("#FF000080"));
        var fullyTransparent = ToRgba32(ColorParser.ParseHex("#FF000000"));

        // Assert
        fullyOpaque.A.Should().Be(255);
        halfTransparent.A.Should().Be(128);
        fullyTransparent.A.Should().Be(0);
    }

    [Fact]
    public void ParseHex_ConsistentResults_WithSameInput()
    {
        // Arrange
        const string hex = "#AABBCC";

        // Act
        var result1 = ColorParser.ParseHex(hex);
        var result2 = ColorParser.ParseHex(hex);

        // Assert
        result1.Should().Be(result2);
    }

    [Fact]
    public void ParseHex_RGBExpansion_MatchesExpectedRRGGBB()
    {
        // This test verifies that #RGB correctly expands to #RRGGBB
        // For example: #F0A should expand to #FF00AA

        // Arrange
        const string testRgb = "#F0A";
        const string expectedRrgGbb = "#FF00AA";

        // Act
        var rgbResult = ColorParser.ParseHex(testRgb);
        var rrggbbResult = ColorParser.ParseHex(expectedRrgGbb);

        // Assert
        rgbResult.Should().Be(rrggbbResult);
    }

    [Fact]
    public void ParseHex_EdgeCaseValues_HandleCorrectly()
    {
        // Test edge cases like all 0s, all Fs, etc.

        // Act & Assert - Should not throw
        var allZeros = ToRgba32(ColorParser.ParseHex("#000"));
        var allFs = ToRgba32(ColorParser.ParseHex("#FFF"));
        var mixed = ToRgba32(ColorParser.ParseHex("#0F0F0F0F"));

        allZeros.R.Should().Be(0);
        allZeros.G.Should().Be(0);
        allZeros.B.Should().Be(0);

        allFs.R.Should().Be(255);
        allFs.G.Should().Be(255);
        allFs.B.Should().Be(255);

        mixed.R.Should().Be(15);
        mixed.G.Should().Be(15);
        mixed.B.Should().Be(15);
        mixed.A.Should().Be(15);
    }
}
