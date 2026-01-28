using BlazorFast.ImageSharp.TableGenerator;
using BlazorFast.ImageSharp.TableGenerator.Builders;
using BlazorFast.ImageSharp.TableGenerator.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace BlazorFast.ImageSharp.TableGenerator.Tests.Integration;

public class BasicTableRenderingTests
{
    [Fact]
    public void Render_SimpleTable_ProducesValidImage()
    {
        // Arrange
        var table = TableBuilder
            .Create()
            .Body(body => body.Row("Name", "Age", "City").Row("John", "30", "NYC"))
            .Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);
        image.Should().BeOfType<Image<Rgba32>>();

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_EmptyBody_ProducesMinimalImage()
    {
        // Arrange
        var table = TableBuilder.Create().Header(header => header.Row("Column")).Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithHeaderOnly_ProducesValidImage()
    {
        // Arrange
        var table = TableBuilder
            .Create()
            .Header(header =>
                header
                    .Style(s => s.Background("#007bff").TextColor("#ffffff").Bold())
                    .Row("Name", "Age", "Occupation")
            )
            .Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithAllSections_ProducesValidImage()
    {
        // Arrange
        var table = TableBuilder
            .Create()
            .Header(header =>
                header
                    .Style(s => s.Background("#343a40").TextColor("#ffffff"))
                    .Row("Column 1", "Column 2", "Column 3")
            )
            .Body(body => body.Row("Data 1", "Data 2", "Data 3").Row("Data 4", "Data 5", "Data 6"))
            .Footer(footer =>
                footer
                    .Style(s => s.Background("#f8f9fa").Italic())
                    .Row("Footer 1", "Footer 2", "Footer 3")
            )
            .Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithCustomDimensions_RespectsMaxWidth()
    {
        // Arrange
        var table = TableBuilder
            .Create()
            .Width(400f)
            .Body(body =>
                body.Row("Very long text that should wrap", "More text", "Even more text")
            )
            .Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeLessOrEqualTo(410); // Max width + small margin
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithBorders_ProducesValidImage()
    {
        // Arrange
        var table = TableBuilder
            .Create()
            .Border(2f)
            .Style(s => s.BorderColor("#000000"))
            .Body(body => body.Row("A", "B", "C").Row("1", "2", "3"))
            .Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithPadding_ProducesValidImage()
    {
        // Arrange
        var table = TableBuilder
            .Create()
            .CellPadding(20f)
            .Body(body => body.Row("Padded", "Content").Row("More", "Data"))
            .Build();

        // Act
        var image = table.Render();

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithRenderOptions_AppliesOptions()
    {
        // Arrange
        var table = TableBuilder.Create().Body(body => body.Row("Test")).Build();

        var options = new RenderOptions { Background = Color.LightGray, Margin = Padding.All(10f) };

        // Act
        var image = table.Render(options);

        // Assert
        image.Should().NotBeNull();
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);

        // Cleanup
        image.Dispose();
    }

    [Fact]
    public void Render_TableWithMultipleRows_SizeIncreasesWithRowCount()
    {
        // Arrange
        var tableWith2Rows = TableBuilder
            .Create()
            .Body(body => body.Row("Data 1").Row("Data 2"))
            .Build();

        var tableWith5Rows = TableBuilder
            .Create()
            .Body(body =>
                body.Row("Data 1").Row("Data 2").Row("Data 3").Row("Data 4").Row("Data 5")
            )
            .Build();

        // Act
        var image2Rows = tableWith2Rows.Render();
        var image5Rows = tableWith5Rows.Render();

        // Assert
        image2Rows.Height.Should().BeLessThan(image5Rows.Height);

        // Cleanup
        image2Rows.Dispose();
        image5Rows.Dispose();
    }

    [Fact]
    public void Render_TableWithWideContent_WidthIncreasesWithContent()
    {
        // Arrange
        var narrowTable = TableBuilder.Create().Body(body => body.Row("A", "B", "C")).Build();

        var wideTable = TableBuilder
            .Create()
            .Body(body =>
                body.Row("Very Long Column 1", "Very Long Column 2", "Very Long Column 3")
            )
            .Build();

        // Act
        var narrowImage = narrowTable.Render();
        var wideImage = wideTable.Render();

        // Assert
        narrowImage.Width.Should().BeLessThan(wideImage.Width);

        // Cleanup
        narrowImage.Dispose();
        wideImage.Dispose();
    }

    [Fact]
    public void Render_MultipleTables_EachProducesIndependentImages()
    {
        // Arrange
        var table1 = TableBuilder.Create().Body(body => body.Row("Table 1")).Build();

        var table2 = TableBuilder.Create().Body(body => body.Row("Table 2")).Build();

        // Act
        var image1 = table1.Render();
        var image2 = table2.Render();

        // Assert
        image1.Should().NotBeSameAs(image2);
        image1.Should().NotBeNull();
        image2.Should().NotBeNull();

        // Cleanup
        image1.Dispose();
        image2.Dispose();
    }
}
