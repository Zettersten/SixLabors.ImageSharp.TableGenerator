using BlazorFast.ImageSharp.TableGenerator.Builders;
using BlazorFast.ImageSharp.TableGenerator.Models;

namespace BlazorFast.ImageSharp.TableGenerator.Tests.Builders;

public class CellBuilderTests
{
    [Fact]
    public void CellBuilder_Constructor_SetsText()
    {
        // Arrange & Act
        var builder = new CellBuilder("Test Text");
        var cell = builder.Build();

        // Assert
        cell.Text.Should().Be("Test Text");
        cell.ColSpan.Should().Be(1); // Default
        cell.RowSpan.Should().Be(1); // Default
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void CellBuilder_ColSpan_SetsCorrectValue(int span)
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.ColSpan(span).Build();

        // Assert
        cell.ColSpan.Should().Be(span);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void CellBuilder_ColSpan_InvalidValue_ThrowsException(int invalidSpan)
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act & Assert
        var act = () => builder.ColSpan(invalidSpan);
        act.Should().Throw<ArgumentException>().WithMessage("Column span must be at least 1*");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void CellBuilder_RowSpan_SetsCorrectValue(int span)
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.RowSpan(span).Build();

        // Assert
        cell.RowSpan.Should().Be(span);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void CellBuilder_RowSpan_InvalidValue_ThrowsException(int invalidSpan)
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act & Assert
        var act = () => builder.RowSpan(invalidSpan);
        act.Should().Throw<ArgumentException>().WithMessage("Row span must be at least 1*");
    }

    [Fact]
    public void CellBuilder_Width_SetsWidth()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Width(150f).Build();

        // Assert
        cell.Width.Should().Be(150f);
    }

    [Theory]
    [InlineData(HAlign.Left)]
    [InlineData(HAlign.Center)]
    [InlineData(HAlign.Right)]
    public void CellBuilder_Align_HorizontalOnly_SetsHorizontalAlignment(HAlign hAlign)
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Align(hAlign).Build();

        // Assert
        cell.Style!.HAlign.Should().Be(hAlign);
    }

    [Theory]
    [InlineData(VAlign.Top)]
    [InlineData(VAlign.Middle)]
    [InlineData(VAlign.Bottom)]
    public void CellBuilder_Align_VerticalOnly_SetsVerticalAlignment(VAlign vAlign)
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Align(vAlign).Build();

        // Assert
        cell.Style!.VAlign.Should().Be(vAlign);
    }

    [Fact]
    public void CellBuilder_Align_BothAlignments_SetsBothAlignments()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Align(HAlign.Center, VAlign.Middle).Build();

        // Assert
        cell.Style!.HAlign.Should().Be(HAlign.Center);
        cell.Style!.VAlign.Should().Be(VAlign.Middle);
    }

    [Fact]
    public void CellBuilder_Bold_SetsFontStyleToBold()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Bold().Build();

        // Assert
        cell.Style!.FontStyle.Should().Be(SixLabors.Fonts.FontStyle.Bold);
    }

    [Fact]
    public void CellBuilder_Italic_SetsFontStyleToItalic()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Italic().Build();

        // Assert
        cell.Style!.FontStyle.Should().Be(SixLabors.Fonts.FontStyle.Italic);
    }

    [Fact]
    public void CellBuilder_Style_AppliesStyleConfiguration()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder.Style(s => s.Background("#ff0000").TextColor("#ffffff").Bold()).Build();

        // Assert
        cell.Style.Should().NotBeNull();
        cell.Style!.Background.Should().NotBeNull();
        cell.Style!.TextColor.Should().NotBeNull();
        cell.Style!.FontStyle.Should().Be(SixLabors.Fonts.FontStyle.Bold);
    }

    [Fact]
    public void CellBuilder_Style_MergesWithExistingStyle()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder
            .Bold() // Sets FontStyle
            .Style(s => s.Background("#ff0000")) // Adds background but keeps FontStyle
            .Build();

        // Assert
        cell.Style!.FontStyle.Should().Be(SixLabors.Fonts.FontStyle.Bold);
        cell.Style!.Background.Should().NotBeNull();
    }

    [Fact]
    public void CellBuilder_FluentChaining_WorksCorrectly()
    {
        // Arrange & Act
        var cell = new CellBuilder("Test Cell")
            .ColSpan(2)
            .RowSpan(3)
            .Width(200f)
            .Align(HAlign.Center, VAlign.Middle)
            .Bold()
            .Style(s => s.Background("#007bff").TextColor("#ffffff").Padding(10f))
            .Build();

        // Assert
        cell.Text.Should().Be("Test Cell");
        cell.ColSpan.Should().Be(2);
        cell.RowSpan.Should().Be(3);
        cell.Width.Should().Be(200f);
        cell.Style!.HAlign.Should().Be(HAlign.Center);
        cell.Style!.VAlign.Should().Be(VAlign.Middle);
        cell.Style!.FontStyle.Should().Be(SixLabors.Fonts.FontStyle.Bold);
        cell.Style!.Background.Should().NotBeNull();
        cell.Style!.TextColor.Should().NotBeNull();
    }

    [Fact]
    public void CellBuilder_ImplicitConversion_WorksCorrectly()
    {
        // Arrange
        var builder = new CellBuilder("Test").ColSpan(2);

        // Act - implicit conversion
        TableCell cell = builder;

        // Assert
        cell.Should().NotBeNull();
        cell.Text.Should().Be("Test");
        cell.ColSpan.Should().Be(2);
    }

    [Fact]
    public void CellBuilder_EmptyText_HandledCorrectly()
    {
        // Arrange & Act
        var cell1 = new CellBuilder("").Build();
        var cell2 = new CellBuilder(null!).Build(); // Null input

        // Assert
        cell1.Text.Should().Be("");
        // Cell2 text will be null since TableCell doesn't automatically convert null to empty
        (cell2.Text == null || cell2.Text == string.Empty)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void CellBuilder_MultipleAlignmentCalls_LastOneWins()
    {
        // Arrange
        var builder = new CellBuilder("Test");

        // Act
        var cell = builder
            .Align(HAlign.Left, VAlign.Top)
            .Align(HAlign.Right, VAlign.Bottom) // Should override
            .Build();

        // Assert
        cell.Style!.HAlign.Should().Be(HAlign.Right);
        cell.Style!.VAlign.Should().Be(VAlign.Bottom);
    }

    [Fact]
    public void CellBuilder_WithoutStyle_HasNoStyle()
    {
        // Arrange & Act
        var cell = new CellBuilder("Test").Build();

        // Assert
        cell.Style.Should().BeNull();
    }
}
