using FluentAssertions;
using SixLabors.ImageSharp.TableGenerator.Builders;

namespace SixLabors.ImageSharp.TableGenerator.Tests.Integration;

public class StyleVariationsTests
{
    private readonly string _outputDir;

    public StyleVariationsTests()
    {
        _outputDir = Path.Combine(AppContext.BaseDirectory, "test-output", "style-variations");
        Directory.CreateDirectory(_outputDir);
    }

    [Fact]
    public void CondensedStyle_ShouldRenderCompactTable()
    {
        // Arrange - Minimal padding, small font, tight spacing
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 10)
            .CellPadding(2)
            .Border(1)
            .Style(s => s.Background("#ffffff").BorderColor("#cccccc"))
            .Header(header =>
                header.Style(s => s.Background("#f0f0f0").Bold()).Row("ID", "Status", "Count")
            )
            .Body(body =>
                body.Row("1", "Active", "42").Row("2", "Pending", "7").Row("3", "Complete", "128")
            )
            .Build();

        // Act
        var image = table.Render();
        var outputPath = Path.Combine(_outputDir, "condensed-style.png");
        image.SaveAsPng(outputPath);

        // Assert
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);
        image.Height.Should().BeLessThan(150); // Should be compact
    }

    [Fact]
    public void CozyStyle_ShouldRenderMediumPaddingTable()
    {
        // Arrange - Moderate padding, readable font size
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 13)
            .CellPadding(8)
            .Border(1)
            .Style(s => s.Background("#fafafa").BorderColor("#d0d0d0"))
            .Header(header =>
                header
                    .Style(s => s.Background("#4a90e2").TextColor("#ffffff").Bold())
                    .Row("Product", "Category", "Price")
            )
            .Body(body =>
                body.Row("Laptop", "Electronics", "$1,299")
                    .Row("Desk Chair", "Furniture", "$349")
                    .Row("Monitor", "Electronics", "$499")
            )
            .Build();

        // Act
        var image = table.Render();
        var outputPath = Path.Combine(_outputDir, "cozy-style.png");
        image.SaveAsPng(outputPath);

        // Assert
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeInRange(120, 250);
    }

    [Fact]
    public void RoomyStyle_ShouldRenderGenerousPaddingTable()
    {
        // Arrange - Large padding, comfortable spacing
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 16)
            .CellPadding(20)
            .Border(2)
            .Style(s => s.Background("#ffffff").BorderColor("#999999"))
            .Header(header =>
                header
                    .Style(s =>
                        s.Background("#2c3e50").TextColor("#ecf0f1").Bold().FontSize(18).Padding(24)
                    )
                    .Row("Team", "Score", "Rank")
            )
            .Body(body => body.Row("Warriors", "98", "1st").Row("Lakers", "87", "2nd"))
            .Build();

        // Act
        var image = table.Render();
        var outputPath = Path.Combine(_outputDir, "roomy-style.png");
        image.SaveAsPng(outputPath);

        // Assert
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(170); // Should be spacious
    }

    [Fact]
    public void MinimalistStyle_ShouldRenderBorderlessTable()
    {
        // Arrange - No borders, subtle backgrounds
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 14)
            .CellPadding(12)
            .Style(s => s.Background("#ffffff"))
            .Header(header =>
                header
                    .Style(s =>
                        s.Background("#f5f5f5")
                            .TextColor("#333333")
                            .Bold()
                            .BorderBottom(1)
                            .BorderColor("#e0e0e0")
                    )
                    .Row("Name", "Role", "Department", "Location")
            )
            .Body(body =>
                body.Row("Alice Chen", "Engineer", "Engineering", "San Francisco")
                    .Row("Bob Smith", "Designer", "Design", "New York")
                    .Row("Carol Lee", "Manager", "Operations", "Seattle")
            )
            .Build();

        // Act
        var image = table.Render();
        var outputPath = Path.Combine(_outputDir, "minimalist-style.png");
        image.SaveAsPng(outputPath);

        // Assert
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);
    }

    [Fact]
    public void DarkModeStyle_ShouldRenderDarkThemedTable()
    {
        // Arrange - Dark background with light text
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 14)
            .CellPadding(12)
            .Border(1)
            .Style(s => s.Background("#1e1e1e").TextColor("#e0e0e0").BorderColor("#404040"))
            .Header(header =>
                header
                    .Style(s =>
                        s.Background("#2d2d30")
                            .TextColor("#ffffff")
                            .Bold()
                            .BorderBottom(2)
                            .BorderColor("#007acc")
                    )
                    .Row("Command", "Description", "Shortcut")
            )
            .Body(body =>
                body.Row("Build", "Compile project", "Ctrl+Shift+B")
                    .Row("Run", "Execute program", "F5")
                    .Row("Debug", "Start debugger", "F5")
            )
            .Build();

        // Act
        var image = table.Render();
        var outputPath = Path.Combine(_outputDir, "dark-mode-style.png");
        image.SaveAsPng(outputPath);

        // Assert
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);
    }

    [Fact]
    public void AlternatingRowsStyle_ShouldRenderStripedTable()
    {
        // Arrange - Zebra striping for readability
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 13)
            .CellPadding(10)
            .Border(1)
            .Style(s => s.BorderColor("#dddddd"))
            .Header(header =>
                header
                    .Style(s => s.Background("#34495e").TextColor("#ffffff").Bold().Padding(12))
                    .Row("ID", "Date", "Event", "Status")
            )
            .Body(body =>
                body.Row("1001", "2026-01-15", "Deployment", "Success")
                    .Row("1002", "2026-01-16", "Rollback", "Completed")
                    .Row("1003", "2026-01-17", "Update", "In Progress")
                    .Row("1004", "2026-01-18", "Maintenance", "Scheduled")
                    .Row("1005", "2026-01-19", "Backup", "Completed")
            )
            .AlternateRows(even => even.Background("#f9f9f9"), odd => odd.Background("#ffffff"))
            .Build();

        // Act
        var image = table.Render();
        var outputPath = Path.Combine(_outputDir, "alternating-rows-style.png");
        image.SaveAsPng(outputPath);

        // Assert
        image.Width.Should().BeGreaterThan(0);
        image.Height.Should().BeGreaterThan(0);
    }
}
