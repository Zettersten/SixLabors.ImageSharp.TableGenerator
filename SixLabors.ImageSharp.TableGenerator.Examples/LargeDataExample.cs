using BlazorFast.ImageSharp.TableGenerator.Builders;
using BlazorFast.ImageSharp.TableGenerator.Models;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp;


namespace BlazorFast.ImageSharp.TableGenerator.Examples;

public static class LargeDataExample
{
    public static void Run(string outputDir)
    {
        Console.WriteLine("Running Large Data Example...");

        GenerateStockMarketTable(outputDir);
        GenerateAnalyticsDashboard(outputDir);

        Console.WriteLine("  ✓ Large data examples complete!");
    }

    private static void GenerateStockMarketTable(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 11)
            .CellPadding(6)
            .Border(1)
            .Style(s => s.Background("#ffffff").BorderColor("#cccccc"))
            .Header(header =>
                header
                    .Style(s =>
                        s.Background("#2c3e50")
                            .TextColor("#ffffff")
                            .Bold()
                            .Padding(8)
                            .HAlign(HAlign.Center)
                    )
                    .Row(
                        "Ticker",
                        "Company",
                        "Price",
                        "Change",
                        "% Change",
                        "Volume",
                        "Mkt Cap",
                        "P/E"
                    )
            )
            .Body(body =>
                body.Row(row =>
                        row.Cell("AAPL")
                            .Cell("Apple Inc.")
                            .Cell("187.45", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+2.34",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+1.27%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("42.5M", cell => cell.Align(HAlign.Right))
                            .Cell("2.87T", cell => cell.Align(HAlign.Right))
                            .Cell("28.5", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("MSFT")
                            .Cell("Microsoft Corp.")
                            .Cell("402.12", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+5.67",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+1.43%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("28.3M", cell => cell.Align(HAlign.Right))
                            .Cell("2.99T", cell => cell.Align(HAlign.Right))
                            .Cell("35.2", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("GOOGL")
                            .Cell("Alphabet Inc.")
                            .Cell("145.89", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "-1.23",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "-0.84%",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                            .Cell("31.2M", cell => cell.Align(HAlign.Right))
                            .Cell("1.82T", cell => cell.Align(HAlign.Right))
                            .Cell("25.8", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("AMZN")
                            .Cell("Amazon.com Inc.")
                            .Cell("178.34", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+3.21",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+1.83%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("45.7M", cell => cell.Align(HAlign.Right))
                            .Cell("1.85T", cell => cell.Align(HAlign.Right))
                            .Cell("62.4", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("NVDA")
                            .Cell("NVIDIA Corp.")
                            .Cell("725.67", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+15.43",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+2.17%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("68.9M", cell => cell.Align(HAlign.Right))
                            .Cell("1.79T", cell => cell.Align(HAlign.Right))
                            .Cell("71.3", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("META")
                            .Cell("Meta Platforms")
                            .Cell("489.23", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+7.89",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+1.64%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("19.4M", cell => cell.Align(HAlign.Right))
                            .Cell("1.24T", cell => cell.Align(HAlign.Right))
                            .Cell("29.6", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("TSLA")
                            .Cell("Tesla Inc.")
                            .Cell("245.67", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "-4.23",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "-1.69%",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                            .Cell("112.3M", cell => cell.Align(HAlign.Right))
                            .Cell("779B", cell => cell.Align(HAlign.Right))
                            .Cell("68.9", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("JPM")
                            .Cell("JPMorgan Chase")
                            .Cell("189.34", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+0.87",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+0.46%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("8.7M", cell => cell.Align(HAlign.Right))
                            .Cell("549B", cell => cell.Align(HAlign.Right))
                            .Cell("10.8", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("V")
                            .Cell("Visa Inc.")
                            .Cell("278.45", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+2.11",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+0.76%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("6.4M", cell => cell.Align(HAlign.Right))
                            .Cell("565B", cell => cell.Align(HAlign.Right))
                            .Cell("32.1", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("WMT")
                            .Cell("Walmart Inc.")
                            .Cell("67.89", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "-0.34",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "-0.50%",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                            .Cell("12.1M", cell => cell.Align(HAlign.Right))
                            .Cell("534B", cell => cell.Align(HAlign.Right))
                            .Cell("28.7", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Cell("UNH")
                            .Cell("UnitedHealth")
                            .Cell("534.21", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+3.45",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+0.65%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("2.8M", cell => cell.Align(HAlign.Right))
                            .Cell("498B", cell => cell.Align(HAlign.Right))
                            .Cell("24.5", cell => cell.Align(HAlign.Right))
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("DIS")
                            .Cell("Walt Disney Co.")
                            .Cell("98.76", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+1.23",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell(
                                "+1.26%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                            .Cell("15.3M", cell => cell.Align(HAlign.Right))
                            .Cell("180B", cell => cell.Align(HAlign.Right))
                            .Cell("45.2", cell => cell.Align(HAlign.Right))
                    )
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-stock-market-large.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Stock market table: {outputPath}");
    }

    private static void GenerateAnalyticsDashboard(string outputDir)
    {
        var table = TableBuilder
            .Create()
            .DefaultFont("Arial", 12)
            .CellPadding(8)
            .Border(1)
            .Style(s => s.Background("#ffffff").BorderColor("#dee2e6"))
            .Header(header =>
                header
                    .Style(s => s.Background("#495057").TextColor("#ffffff").Bold().Padding(10))
                    .Row("Region", "Revenue", "Users", "Conversion", "Avg Order", "Growth")
            )
            .Body(body =>
                body.Row(row =>
                        row.Cell("North America")
                            .Cell("$24.5M", cell => cell.Align(HAlign.Right))
                            .Cell("1,247,893", cell => cell.Align(HAlign.Right))
                            .Cell("3.42%", cell => cell.Align(HAlign.Right))
                            .Cell("$67.23", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+12.3%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("Europe")
                            .Cell("$18.7M", cell => cell.Align(HAlign.Right))
                            .Cell("892,456", cell => cell.Align(HAlign.Right))
                            .Cell("2.98%", cell => cell.Align(HAlign.Right))
                            .Cell("$54.12", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+8.7%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Cell("Asia Pacific")
                            .Cell("$31.2M", cell => cell.Align(HAlign.Right))
                            .Cell("2,456,789", cell => cell.Align(HAlign.Right))
                            .Cell("4.15%", cell => cell.Align(HAlign.Right))
                            .Cell("$42.89", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+18.2%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("Latin America")
                            .Cell("$7.3M", cell => cell.Align(HAlign.Right))
                            .Cell("456,123", cell => cell.Align(HAlign.Right))
                            .Cell("2.34%", cell => cell.Align(HAlign.Right))
                            .Cell("$38.45", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "-2.1%",
                                cell => cell.Style(s => s.TextColor("#e74c3c").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Cell("Middle East")
                            .Cell("$5.8M", cell => cell.Align(HAlign.Right))
                            .Cell("234,567", cell => cell.Align(HAlign.Right))
                            .Cell("3.67%", cell => cell.Align(HAlign.Right))
                            .Cell("$71.23", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+15.4%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
                    .Row(row =>
                        row.Style(s => s.Background("#f8f9fa"))
                            .Cell("Africa")
                            .Cell("$2.1M", cell => cell.Align(HAlign.Right))
                            .Cell("123,789", cell => cell.Align(HAlign.Right))
                            .Cell("1.89%", cell => cell.Align(HAlign.Right))
                            .Cell("$28.34", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+22.8%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
            )
            .Footer(footer =>
                footer
                    .Style(s => s.Background("#e9ecef").Bold().BorderTop(2).BorderColor("#495057"))
                    .Row(row =>
                        row.Cell("TOTAL")
                            .Cell("$89.6M", cell => cell.Align(HAlign.Right))
                            .Cell("5,411,617", cell => cell.Align(HAlign.Right))
                            .Cell("3.24%", cell => cell.Align(HAlign.Right))
                            .Cell("$52.14", cell => cell.Align(HAlign.Right))
                            .Cell(
                                "+12.1%",
                                cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right))
                            )
                    )
            )
            .Build();

        var image = table.Render();
        var outputPath = Path.Combine(outputDir, "example-analytics-dashboard.png");
        image.SaveAsPng(outputPath);
        Console.WriteLine($"    - Analytics dashboard: {outputPath}");
    }
}
