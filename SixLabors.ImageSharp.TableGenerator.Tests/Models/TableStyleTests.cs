using BlazorFast.ImageSharp.TableGenerator.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace BlazorFast.ImageSharp.TableGenerator.Tests.Models;

public class TableStyleTests
{
    [Fact]
    public void TableStyle_Empty_HasAllNullValues()
    {
        // Arrange & Act
        var style = TableStyle.Empty;

        // Assert
        style.Background.Should().BeNull();
        style.TextColor.Should().BeNull();
        style.BorderColor.Should().BeNull();
        style.BorderWidth.Should().BeNull();
        style.BorderTop.Should().BeNull();
        style.BorderRight.Should().BeNull();
        style.BorderBottom.Should().BeNull();
        style.BorderLeft.Should().BeNull();
        style.CellPadding.Should().BeNull();
        style.HAlign.Should().BeNull();
        style.VAlign.Should().BeNull();
        style.FontFamily.Should().BeNull();
        style.FontSize.Should().BeNull();
        style.FontStyle.Should().BeNull();
    }

    [Fact]
    public void TableStyle_Default_HasExpectedValues()
    {
        // Arrange & Act
        var style = TableStyle.Default;

        // Assert
        style.Background.Should().Be(Color.White);
        style.TextColor.Should().Be(Color.Black);
        style.BorderColor.Should().Be(Color.Black);
        style.BorderWidth.Should().Be(1f);
        style.CellPadding.Should().Be(Padding.All(8f));
        style.HAlign.Should().Be(HAlign.Left);
        style.VAlign.Should().Be(VAlign.Top);
        style.FontFamily.Should().Be("Arial");
        style.FontSize.Should().Be(12f);
        style.FontStyle.Should().Be(SixLabors.Fonts.FontStyle.Regular);
    }

    [Fact]
    public void Merge_WithNullOther_ReturnsOriginalStyle()
    {
        // Arrange
        var original = new TableStyle { Background = Color.Red, TextColor = Color.Blue };

        // Act
        var result = original.Merge(null);

        // Assert
        result.Should().Be(original);
        result.Background.Should().Be(Color.Red);
        result.TextColor.Should().Be(Color.Blue);
    }

    [Fact]
    public void Merge_WithOtherStyle_OverridesNonNullValues()
    {
        // Arrange
        var original = new TableStyle
        {
            Background = Color.Red,
            TextColor = Color.Blue,
            BorderWidth = 2f,
            FontSize = 14f,
        };

        var other = new TableStyle
        {
            Background = Color.Green, // Should override
            BorderWidth = 3f, // Should override
            BorderColor = Color.Yellow, // Should be added (was null)
            // TextColor is null, should keep original
            // FontSize is null, should keep original
        };

        // Act
        var result = original.Merge(other);

        // Assert
        result.Background.Should().Be(Color.Green); // Overridden
        result.TextColor.Should().Be(Color.Blue); // Kept original
        result.BorderWidth.Should().Be(3f); // Overridden
        result.BorderColor.Should().Be(Color.Yellow); // Added
        result.FontSize.Should().Be(14f); // Kept original
    }

    [Fact]
    public void Merge_WithCompleteOverride_ReplacesAllValues()
    {
        // Arrange
        var original = new TableStyle { Background = Color.Red, TextColor = Color.Blue };

        var other = new TableStyle
        {
            Background = Color.Green,
            TextColor = Color.Yellow,
            BorderColor = Color.Black,
            BorderWidth = 5f,
        };

        // Act
        var result = original.Merge(other);

        // Assert
        result.Background.Should().Be(Color.Green);
        result.TextColor.Should().Be(Color.Yellow);
        result.BorderColor.Should().Be(Color.Black);
        result.BorderWidth.Should().Be(5f);
    }

    [Theory]
    [InlineData(HAlign.Left)]
    [InlineData(HAlign.Center)]
    [InlineData(HAlign.Right)]
    public void Merge_HAlign_OverridesCorrectly(HAlign alignment)
    {
        // Arrange
        var original = new TableStyle { HAlign = HAlign.Left };
        var other = new TableStyle { HAlign = alignment };

        // Act
        var result = original.Merge(other);

        // Assert
        result.HAlign.Should().Be(alignment);
    }

    [Theory]
    [InlineData(VAlign.Top)]
    [InlineData(VAlign.Middle)]
    [InlineData(VAlign.Bottom)]
    public void Merge_VAlign_OverridesCorrectly(VAlign alignment)
    {
        // Arrange
        var original = new TableStyle { VAlign = VAlign.Top };
        var other = new TableStyle { VAlign = alignment };

        // Act
        var result = original.Merge(other);

        // Assert
        result.VAlign.Should().Be(alignment);
    }

    [Fact]
    public void Merge_CascadingStyles_AppliesInCorrectOrder()
    {
        // Test the cascade: Table → Section → Row → Cell

        // Arrange
        var tableStyle = new TableStyle
        {
            Background = Color.White,
            TextColor = Color.Black,
            BorderWidth = 1f,
            FontSize = 12f,
        };

        var sectionStyle = new TableStyle
        {
            Background = Color.LightGray, // Override table background
            BorderWidth = 2f, // Override table border
            // Keep table's TextColor and FontSize
        };

        var rowStyle = new TableStyle
        {
            TextColor = Color.Blue, // Override section/table text color
            FontSize = 14f, // Override table font size
            // Keep section's Background and BorderWidth
        };

        var cellStyle = new TableStyle
        {
            Background = Color.Yellow, // Override everything above
            // Keep row's TextColor and FontSize
            // Keep section's BorderWidth
        };

        // Act - Apply cascade
        var result = tableStyle.Merge(sectionStyle).Merge(rowStyle).Merge(cellStyle);

        // Assert - Final style should have:
        result.Background.Should().Be(Color.Yellow); // From cell
        result.TextColor.Should().Be(Color.Blue); // From row
        result.BorderWidth.Should().Be(2f); // From section
        result.FontSize.Should().Be(14f); // From row
    }
}
