using SixLabors.ImageSharp;
using SixLabors.ImageSharp.TableGenerator;
using SixLabors.ImageSharp.TableGenerator.Builders;
using SixLabors.ImageSharp.TableGenerator.Models;

namespace SixLabors.ImageSharp.TableGenerator.Examples;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("SixLabors.ImageSharp.TableGenerator Examples");
        Console.WriteLine("============================================");

        // Ensure output directory exists
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDir);

        try
        {
            // Example 1: Basic Table
            Console.WriteLine("1. Creating basic table...");
            CreateBasicTable(outputDir);

            // Example 2: Table with Header and Styling
            Console.WriteLine("2. Creating styled table with header...");
            CreateStyledTable(outputDir);

            // Example 3: Table with Alternating Rows
            Console.WriteLine("3. Creating table with alternating rows...");
            CreateAlternatingRowsTable(outputDir);

            // Example 4: Table with Spans
            Console.WriteLine("4. Creating table with spans...");
            CreateTableWithSpans(outputDir);

            // Example 5: Table with Text Alignment
            Console.WriteLine("5. Creating table with text alignment...");
            CreateAlignmentTable(outputDir);

            Console.WriteLine(
                $"\nAll examples completed! Check the {outputDir} directory for generated images."
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    static void CreateBasicTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .Body(body =>
                body.Row("Name", "Age", "City")
                    .Row("John Doe", "30", "New York")
                    .Row("Jane Smith", "25", "Los Angeles")
                    .Row("Bob Johnson", "35", "Chicago")
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "basic_table.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"   Saved: {outputPath}");
    }

    static void CreateStyledTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 14)
            .CellPadding(12)
            .Border(2)
            .Style(s => s.Background("#f8f9fa").BorderColor("#dee2e6"))
            .Header(header =>
                header
                    .Style(s => s.Background("#007bff").TextColor("#ffffff").Bold())
                    .Row("Product", "Price", "Stock")
            )
            .Body(body =>
                body.Row("Laptop", "$999", "15")
                    .Row("Mouse", "$25", "50")
                    .Row("Keyboard", "$75", "30")
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "styled_table.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"   Saved: {outputPath}");
    }

    static void CreateAlternatingRowsTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 12)
            .CellPadding(10)
            .Border(1)
            .Header(header =>
                header
                    .Style(s => s.Background("#343a40").TextColor("#ffffff"))
                    .Row("ID", "Name", "Department", "Salary")
            )
            .Body(body =>
                body.Row("001", "Alice Johnson", "Engineering", "$75,000")
                    .Row("002", "Bob Smith", "Marketing", "$65,000")
                    .Row("003", "Carol Davis", "Engineering", "$80,000")
                    .Row("004", "David Wilson", "Sales", "$60,000")
                    .Row("005", "Eva Brown", "HR", "$55,000")
            )
            .AlternateRows(even => even.Background("#ffffff"), odd => odd.Background("#f8f9fa"))
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "alternating_rows_table.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"   Saved: {outputPath}");
    }

    static void CreateTableWithSpans(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 12)
            .CellPadding(8)
            .Border(1)
            .Style(s => s.BorderColor("#000000"))
            .Body(body =>
                body.Row(row =>
                        row.Cell(
                            "Q1 Sales Report",
                            cell => cell.ColSpan(3).Style(s => s.Background("#e9ecef").Bold())
                        )
                    )
                    .Row("Month", "Revenue", "Growth")
                    .Row("January", "$10,000", "+5%")
                    .Row("February", "$12,000", "+20%")
                    .Row("March", "$15,000", "+25%")
                    .Row(row =>
                        row.Cell("Total", cell => cell.Style(s => s.Bold()))
                            .Cell("$37,000", cell => cell.Style(s => s.Bold()))
                            .Cell("+17% avg", cell => cell.Style(s => s.Bold()))
                    )
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "spans_table.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"   Saved: {outputPath}");
    }

    static void CreateAlignmentTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 12)
            .CellPadding(15)
            .Border(1)
            .Header(header =>
                header
                    .Style(s => s.Background("#6c757d").TextColor("#ffffff"))
                    .Row("Left Aligned", "Center Aligned", "Right Aligned")
            )
            .Body(body =>
                body.Row(row =>
                        row.Cell("This text is left aligned", cell => cell.Align(HAlign.Left))
                            .Cell("This text is centered", cell => cell.Align(HAlign.Center))
                            .Cell("This text is right aligned", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("Short", cell => cell.Align(HAlign.Left))
                            .Cell("Medium text here", cell => cell.Align(HAlign.Center))
                            .Cell(
                                "This is a longer piece of text",
                                cell => cell.Align(HAlign.Right)
                            )
                    )
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "alignment_table.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"   Saved: {outputPath}");
    }
}
