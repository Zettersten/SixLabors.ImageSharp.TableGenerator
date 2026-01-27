# SixLabors.ImageSharp.TableGenerator

A comprehensive table rendering library for SixLabors.ImageSharp that generates table images with an HTML/CSS-inspired fluent API.

## Features

- **Fluent API**: HTML/CSS-inspired builder pattern for easy table construction
- **Comprehensive Styling**: Support for backgrounds, borders, fonts, padding, and text alignment
- **Advanced Layout**: Row/column spans, fixed and auto-sizing columns, text wrapping
- **Section Support**: Header, body, and footer sections with independent styling
- **Alternating Rows**: Built-in support for zebra-striped tables
- **Text Alignment**: Horizontal (left, center, right) and vertical (top, middle, bottom) alignment
- **Responsive Design**: Automatic column width calculation with max-width constraints

## Installation

Add the NuGet package to your project (when published):

```bash
dotnet add package SixLabors.ImageSharp.TableGenerator
```

For now, you can reference the project directly or build it locally.

## Dependencies

- **SixLabors.ImageSharp** (3.1.7+): Core image processing
- **SixLabors.ImageSharp.Drawing** (2.1.5+): Drawing and text rendering
- **SixLabors.Fonts** (2.0.8+): Font loading and text measurement

## Quick Start

### Basic Table

```csharp
using SixLabors.ImageSharp.TableGenerator;
using SixLabors.ImageSharp.TableGenerator.Builders;

var table = TableBuilder.Create()
    .Body(body => body
        .Row("Name", "Age", "City")
        .Row("John Doe", "30", "New York")
        .Row("Jane Smith", "25", "Los Angeles")
        .Row("Bob Johnson", "35", "Chicago"))
    .Build();

var image = table.Render();
image.SaveAsPng("basic_table.png");
```

### Styled Table with Header

```csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 14)
    .CellPadding(12)
    .Border(2)
    .Style(s => s
        .Background("#f8f9fa")
        .BorderColor("#dee2e6"))
    .Header(header => header
        .Style(s => s.Background("#007bff").TextColor("#ffffff").Bold())
        .Row("Product", "Price", "Stock"))
    .Body(body => body
        .Row("Laptop", "$999", "15")
        .Row("Mouse", "$25", "50")
        .Row("Keyboard", "$75", "30"))
    .Build();

var image = table.Render();
image.SaveAsPng("styled_table.png");
```

### Alternating Row Colors

```csharp
var table = TableBuilder.Create()
    .Header(header => header
        .Style(s => s.Background("#343a40").TextColor("#ffffff"))
        .Row("ID", "Name", "Department", "Salary"))
    .Body(body => body
        .Row("001", "Alice Johnson", "Engineering", "$75,000")
        .Row("002", "Bob Smith", "Marketing", "$65,000")
        .Row("003", "Carol Davis", "Engineering", "$80,000"))
    .AlternateRows(
        even => even.Background("#ffffff"),
        odd => odd.Background("#f8f9fa"))
    .Build();

var image = table.Render();
image.SaveAsPng("alternating_table.png");
```

### Table with Spans

```csharp
var table = TableBuilder.Create()
    .Body(body => body
        .Row(row => row
            .Cell("Q1 Sales Report", cell => cell.ColSpan(3).Style(s => s.Background("#e9ecef").Bold())))
        .Row("Month", "Revenue", "Growth")
        .Row("January", "$10,000", "+5%")
        .Row("February", "$12,000", "+20%"))
    .Build();
```

### Column Configuration

```csharp
var table = TableBuilder.Create()
    .Columns(cols => cols
        .Fixed(100)    // Fixed width column
        .Auto()        // Auto-sizing column
        .Fixed(150))   // Another fixed width column
    .Width(800)        // Maximum table width
    .Body(body => body
        .Row("Fixed 100px", "Auto-sized", "Fixed 150px")
        .Row("Short", "This is a longer text that will wrap", "Medium"))
    .Build();
```

## API Reference

### TableBuilder

Main entry point for creating tables:

- `Create()`: Creates a new table builder
- `DefaultFont(family, size)`: Sets default font
- `CellPadding(padding)`: Sets default cell padding
- `Border(width)`: Sets default border width
- `Width(maxWidth)`: Sets maximum table width
- `Columns(configure)`: Configures column specifications
- `Header/Body/Footer(configure)`: Configures table sections
- `AlternateRows(evenStyle, oddStyle)`: Applies alternating row styles
- `Style(configure)`: Applies table-level styling
- `Build()`: Creates the Table instance

### StyleBuilder

Configures styling for table elements:

- `Background(color)`: Sets background color (hex or Color)
- `TextColor(color)`: Sets text color
- `BorderColor(color)`: Sets border color
- `Border(width)`: Sets border width for all sides
- `BorderTop/Right/Bottom/Left(width)`: Sets individual border widths
- `FontFamily(name)`: Sets font family
- `FontSize(size)`: Sets font size
- `Bold()`: Makes text bold
- `Italic()`: Makes text italic
- `HAlign(alignment)`: Sets horizontal alignment
- `VAlign(alignment)`: Sets vertical alignment
- `Padding(padding)`: Sets cell padding

### CellBuilder

Configures individual cells:

- `ColSpan(count)`: Sets column span
- `RowSpan(count)`: Sets row span
- `Width(width)`: Sets cell width
- `Align(hAlign, vAlign)`: Sets text alignment
- `Bold()`: Makes cell text bold
- `Style(configure)`: Applies cell-specific styling

### Rendering Options

```csharp
var options = new RenderOptions
{
    Background = Color.White,
    Margin = Padding.All(10)
};

var image = table.Render(options);
```

## Style Variations

The library supports various table styles for different use cases:

### Condensed Style
```csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 10)
    .CellPadding(2)
    .Border(1)
    .Style(s => s.Background("#ffffff").BorderColor("#cccccc"))
    .Header(header => header
        .Style(s => s.Background("#f0f0f0").Bold())
        .Row("ID", "Status", "Count"))
    .Body(body => body
        .Row("1", "Active", "42")
        .Row("2", "Pending", "7"))
    .Build();
```

### Cozy Style
```csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 13)
    .CellPadding(8)
    .Border(1)
    .Style(s => s.Background("#fafafa").BorderColor("#d0d0d0"))
    .Header(header => header
        .Style(s => s.Background("#4a90e2").TextColor("#ffffff").Bold())
        .Row("Product", "Category", "Price"))
    .Body(body => body
        .Row("Laptop", "Electronics", "$1,299")
        .Row("Monitor", "Electronics", "$499"))
    .Build();
```

### Roomy Style
```csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 16)
    .CellPadding(20)
    .Border(2)
    .Style(s => s.Background("#ffffff").BorderColor("#999999"))
    .Header(header => header
        .Style(s => s.Background("#2c3e50").TextColor("#ecf0f1").Bold().FontSize(18).Padding(24))
        .Row("Team", "Score", "Rank"))
    .Body(body => body
        .Row("Warriors", "98", "1st")
        .Row("Lakers", "87", "2nd"))
    .Build();
```

### Dark Mode Style
```csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 14)
    .CellPadding(12)
    .Border(1)
    .Style(s => s.Background("#1e1e1e").TextColor("#e0e0e0").BorderColor("#404040"))
    .Header(header => header
        .Style(s => s.Background("#2d2d30").TextColor("#ffffff").Bold().BorderBottom(2).BorderColor("#007acc"))
        .Row("Command", "Description", "Shortcut"))
    .Body(body => body
        .Row("Build", "Compile project", "Ctrl+Shift+B")
        .Row("Run", "Execute program", "F5"))
    .Build();
```

## Large Data Tables

The library efficiently handles tables with many rows and numerical data:

### Financial Data Example
```csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 11)
    .CellPadding(6)
    .Border(1)
    .Header(header => header
        .Style(s => s.Background("#2c3e50").TextColor("#ffffff").Bold().HAlign(HAlign.Center))
        .Row("Ticker", "Company", "Price", "Change", "% Change", "Volume", "Mkt Cap", "P/E"))
    .Body(body => body
        .Row(row => row
            .Cell("AAPL")
            .Cell("Apple Inc.")
            .Cell("187.45", cell => cell.Align(HAlign.Right))
            .Cell("+2.34", cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right)))
            .Cell("+1.27%", cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right)))
            .Cell("42.5M", cell => cell.Align(HAlign.Right))
            .Cell("2.87T", cell => cell.Align(HAlign.Right))
            .Cell("28.5", cell => cell.Align(HAlign.Right)))
        // ... more rows
        .Row(row => row
            .Style(s => s.Background("#f8f9fa"))
            .Cell("MSFT")
            .Cell("Microsoft Corp.")
            .Cell("402.12", cell => cell.Align(HAlign.Right))
            .Cell("+5.67", cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right)))
            .Cell("+1.43%", cell => cell.Style(s => s.TextColor("#27ae60").HAlign(HAlign.Right)))
            .Cell("28.3M", cell => cell.Align(HAlign.Right))
            .Cell("2.99T", cell => cell.Align(HAlign.Right))
            .Cell("35.2", cell => cell.Align(HAlign.Right))))
    .Build();
```

## Architecture

The library follows a clean architecture with distinct layers:

1. **Builder API Layer**: Fluent API for constructing tables
2. **Model Layer**: Immutable data structures representing table state
3. **Layout Engine**: Algorithms for grid placement, column sizing, and text wrapping
4. **Rendering Engine**: Image generation using SixLabors.ImageSharp.Drawing

### Key Design Decisions

- **Style Cascade**: Table → Section → Row → Cell (follows HTML/CSS model)
- **Column Width Algorithm**: Fixed columns are honored, auto columns fit content
- **Border Model**: Separate borders (each cell draws its own borders)
- **Text Wrapping**: Greedy word wrapping with character-level fallback
- **Span Resolution**: 2D grid tracking for proper span placement

## Performance Considerations

- **Font Caching**: Fonts are cached to avoid repeated file I/O
- **Lazy Measurement**: Layout is calculated only when rendering
- **Efficient Text Wrapping**: Optimized algorithms for text measurement
- **Memory Management**: Proper disposal of ImageSharp resources

## Examples

The `Examples` project contains comprehensive demonstrations of all features:

1. **Basic Table**: Simple data table
2. **Styled Table**: Professional styling with headers
3. **Alternating Rows**: Zebra-striped tables
4. **Table with Spans**: Column and row spanning
5. **Text Alignment**: Various alignment options

Run the examples:

```bash
cd SixLabors.ImageSharp.TableGenerator.Examples
dotnet run
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the [Apache License 2.0](LICENSE).

## Acknowledgments

Built on the excellent [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) ecosystem.

---

For more examples and detailed documentation, see the [Examples](SixLabors.ImageSharp.TableGenerator.Examples/) project.