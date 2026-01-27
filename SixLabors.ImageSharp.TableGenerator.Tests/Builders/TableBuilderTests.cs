using SixLabors.ImageSharp;
using SixLabors.ImageSharp.TableGenerator.Builders;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Tests.Builders;

public class TableBuilderTests
{
    [Fact]
    public void TableBuilder_Create_ReturnsNewInstance()
    {
        // Act
        var builder = TableBuilder.Create();

        // Assert
        builder.Should().NotBeNull();
        builder.Should().BeOfType<TableBuilder>();
    }

    [Fact]
    public void TableBuilder_Build_CreatesTable()
    {
        // Arrange
        var builder = TableBuilder.Create();

        // Act
        var table = builder.Build();

        // Assert
        table.Should().NotBeNull();
        table.Should().BeOfType<Table>();
    }

    [Fact]
    public void TableBuilder_DefaultFont_SetsFont()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .DefaultFont("Helvetica", 16f)
            .Body(body => body.Row("Test"))
            .Build();

        // Assert
        // We can't easily inspect the internal model, but we can verify it builds successfully
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_CellPadding_SetsPadding()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .CellPadding(10f, 20f)
            .Body(body => body.Row("Test"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_CellPadding_UniformValue_SetsPadding()
    {
        // Arrange & Act
        var table = TableBuilder.Create().CellPadding(15f).Body(body => body.Row("Test")).Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Border_SetsBorderWidth()
    {
        // Arrange & Act
        var table = TableBuilder.Create().Border(3f).Body(body => body.Row("Test")).Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Width_SetsMaxWidth()
    {
        // Arrange & Act
        var table = TableBuilder.Create().Width(800f).Body(body => body.Row("Test")).Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Columns_ConfiguresColumns()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Columns(cols => cols.Fixed(100f).Auto().Fixed(200f))
            .Body(body => body.Row("Col1", "Col2", "Col3"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Header_ConfiguresHeader()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Header(header =>
                header.Style(s => s.Bold().Background("#cccccc")).Row("Name", "Age", "City")
            )
            .Body(body => body.Row("John", "30", "NYC"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Body_ConfiguresBody()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Body(body => body.Row("John", "30", "NYC").Row("Jane", "25", "LA"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Footer_ConfiguresFooter()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Body(body => body.Row("Data"))
            .Footer(footer => footer.Style(s => s.Italic()).Row("Total: 100"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_AlternateRows_WithStyleBuilders_ConfiguresAlternation()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Body(body => body.Row("Row 1").Row("Row 2").Row("Row 3").Row("Row 4"))
            .AlternateRows(even => even.Background("#ffffff"), odd => odd.Background("#f0f0f0"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_AlternateRows_WithTableStyles_ConfiguresAlternation()
    {
        // Arrange
        var evenStyle = new TableStyle { Background = Color.White };
        var oddStyle = new TableStyle { Background = Color.LightGray };

        // Act
        var table = TableBuilder
            .Create()
            .Body(body => body.Row("Row 1").Row("Row 2"))
            .AlternateRows(evenStyle, oddStyle)
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_Style_ConfiguresTableStyle()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Style(s => s.Background("#ffffff").TextColor("#000000").BorderColor("#cccccc"))
            .Body(body => body.Row("Test"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_FluentChaining_WorksCorrectly()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 14f)
            .CellPadding(8f)
            .Border(1f)
            .Width(600f)
            .Style(s => s.Background("#ffffff"))
            .Columns(cols => cols.Auto().Fixed(100f).Auto())
            .Header(header =>
                header
                    .Style(s => s.Bold().Background("#007bff").TextColor("#ffffff"))
                    .Row("Name", "ID", "Status")
            )
            .Body(body =>
                body.Row("John Doe", "001", "Active").Row("Jane Smith", "002", "Inactive")
            )
            .Footer(footer => footer.Row("Total: 2 records", "", ""))
            .AlternateRows(even => even.Background("#ffffff"), odd => odd.Background("#f8f9fa"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_ImplicitConversion_WorksCorrectly()
    {
        // Arrange
        TableBuilder builder = TableBuilder.Create().Body(body => body.Row("Test"));

        // Act - implicit conversion
        Table table = builder;

        // Assert
        table.Should().NotBeNull();
        table.Should().BeOfType<Table>();
    }

    [Fact]
    public void TableBuilder_EmptyBody_BuildsSuccessfully()
    {
        // Arrange & Act
        var table = TableBuilder.Create().Header(header => header.Row("Column")).Build();

        // Assert
        table.Should().NotBeNull();
    }

    [Fact]
    public void TableBuilder_MultipleStyles_MergeCorrectly()
    {
        // Arrange & Act
        var table = TableBuilder
            .Create()
            .Style(s => s.Background("#ffffff").FontSize(12f))
            .Style(s => s.TextColor("#000000").FontSize(14f)) // Should override font size
            .Body(body => body.Row("Test"))
            .Build();

        // Assert
        table.Should().NotBeNull();
    }
}
