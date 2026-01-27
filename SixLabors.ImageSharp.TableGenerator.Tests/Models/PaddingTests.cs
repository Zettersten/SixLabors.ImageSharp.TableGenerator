using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Tests.Models;

public class PaddingTests
{
    [Fact]
    public void Padding_Constructor_SetsAllValues()
    {
        // Arrange & Act
        var padding = new Padding(10f, 20f, 30f, 40f);

        // Assert
        padding.Left.Should().Be(10f);
        padding.Top.Should().Be(20f);
        padding.Right.Should().Be(30f);
        padding.Bottom.Should().Be(40f);
    }

    [Fact]
    public void Padding_All_UniformValue_CreatesUniformPadding()
    {
        // Arrange & Act
        var padding = Padding.All(15f);

        // Assert
        padding.Left.Should().Be(15f);
        padding.Top.Should().Be(15f);
        padding.Right.Should().Be(15f);
        padding.Bottom.Should().Be(15f);
    }

    [Fact]
    public void Padding_All_HorizontalVertical_CreatesCorrectPadding()
    {
        // Arrange & Act
        var padding = Padding.All(10f, 20f);

        // Assert
        padding.Left.Should().Be(10f);
        padding.Right.Should().Be(10f);
        padding.Top.Should().Be(20f);
        padding.Bottom.Should().Be(20f);
    }

    [Fact]
    public void Padding_None_HasZeroValues()
    {
        // Arrange & Act
        var padding = Padding.None;

        // Assert
        padding.Left.Should().Be(0f);
        padding.Top.Should().Be(0f);
        padding.Right.Should().Be(0f);
        padding.Bottom.Should().Be(0f);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(5.5f)]
    [InlineData(100f)]
    public void Padding_All_SingleValue_WorksWithVariousInputs(float value)
    {
        // Arrange & Act
        var padding = Padding.All(value);

        // Assert
        padding.Left.Should().Be(value);
        padding.Top.Should().Be(value);
        padding.Right.Should().Be(value);
        padding.Bottom.Should().Be(value);
    }

    [Fact]
    public void Padding_RecordEquality_WorksCorrectly()
    {
        // Arrange
        var padding1 = new Padding(10f, 20f, 30f, 40f);
        var padding2 = new Padding(10f, 20f, 30f, 40f);
        var padding3 = new Padding(10f, 20f, 30f, 41f);

        // Act & Assert
        padding1.Should().Be(padding2);
        padding1.Should().NotBe(padding3);
        (padding1 == padding2).Should().BeTrue();
        (padding1 == padding3).Should().BeFalse();
    }

    [Fact]
    public void Padding_ToString_ReturnsExpectedFormat()
    {
        // Arrange
        var padding = new Padding(1f, 2f, 3f, 4f);

        // Act
        var result = padding.ToString();

        // Assert
        result.Should().Contain("1");
        result.Should().Contain("2");
        result.Should().Contain("3");
        result.Should().Contain("4");
    }
}
