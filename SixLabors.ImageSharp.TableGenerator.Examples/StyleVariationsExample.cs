using SixLabors.ImageSharp.TableGenerator.Builders;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Examples;

public static class StyleVariationsExample
{
    public static void Run(string outputDir)
    {
        Console.WriteLine("Running Style Variations Example...");

        GenerateCondensedTable(outputDir);
        GenerateCozyTable(outputDir);
        GenerateRoomyTable(outputDir);
        GenerateMinimalistTable(outputDir);
        GenerateDarkModeTable(outputDir);

        Console.WriteLine("  ✓ Style variations examples complete!");
    }

    private static void GenerateCondensedTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 10)
            .CellPadding(2)
            .Border(1)
            .Style(s => s.Background("#ffffff").BorderColor("#cccccc"))
            .Header(header =>
                header.Style(s => s.Background("#f0f0f0").Bold()).Row("ID", "Name", "Status", "Count")
            )
            .Body(body =>
                body.Row("1", "Project Alpha", "Active", "42")
                    .Row("2", "Project Beta", "Pending", "7")
                    .Row("3", "Project Gamma", "Complete", "128")
                    .Row("4", "Project Delta", "Active", "63")
                    .Row("5", "Project Epsilon", "On Hold", "15")
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-condensed-style.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Condensed style: {outputPath}");
    }

    private static void GenerateCozyTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 13)
            .CellPadding(8)
            .Border(1)
            .Style(s => s.Background("#fafafa").BorderColor("#d0d0d0"))
            .Header(header =>
                header
                    .Style(s => s.Background("#3498db").TextColor("#ffffff").Bold())
                    .Row("Product", "Category", "Stock", "Price")
            )
            .Body(body =>
                body.Row(row =>
                        row.Cell("MacBook Pro")
                            .Cell("Computers")
                            .Cell("24", cell => cell.Align(HAlign.Right))
                            .Cell("$2,499", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("Ergonomic Chair")
                            .Cell("Furniture")
                            .Cell("15", cell => cell.Align(HAlign.Right))
                            .Cell("$349", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("4K Monitor")
                            .Cell("Peripherals")
                            .Cell("8", cell => cell.Align(HAlign.Right))
                            .Cell("$599", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("Wireless Mouse")
                            .Cell("Peripherals")
                            .Cell("42", cell => cell.Align(HAlign.Right))
                            .Cell("$79", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("Desk Lamp")
                            .Cell("Accessories")
                            .Cell("31", cell => cell.Align(HAlign.Right))
                            .Cell("$129", cell => cell.Align(HAlign.Right))
                    )
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-cozy-style.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Cozy style: {outputPath}");
    }

    private static void GenerateRoomyTable(string outputDir)
    {
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
                    .Row("Quarter", "Revenue", "Growth")
            )
            .Body(body =>
                body.Row(row =>
                        row.Cell("Q1 2026")
                            .Cell("$24.5M", cell => cell.Align(HAlign.Right))
                            .Cell("+12.3%", cell =>
                                cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Cell("Q2 2026")
                            .Cell("$28.7M", cell => cell.Align(HAlign.Right))
                            .Cell("+17.1%", cell =>
                                cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Cell("Q3 2026")
                            .Cell("$31.2M", cell => cell.Align(HAlign.Right))
                            .Cell("+8.7%", cell =>
                                cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-roomy-style.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Roomy style: {outputPath}");
    }

    private static void GenerateMinimalistTable(string outputDir)
    {
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

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-minimalist-style.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Minimalist style: {outputPath}");
    }

    private static void GenerateDarkModeTable(string outputDir)
    {
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
                    .Row("Test", "Run all tests", "Ctrl+R, A")
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-dark-mode-style.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Dark mode style: {outputPath}");
    }
}
