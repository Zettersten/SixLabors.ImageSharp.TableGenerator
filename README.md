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