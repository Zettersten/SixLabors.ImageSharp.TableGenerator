# SixLabors.ImageSharp.TableGenerator

[![NuGet](https://img.shields.io/nuget/v/SixLabors.ImageSharp.TableGenerator.svg)](https://www.nuget.org/packages/SixLabors.ImageSharp.TableGenerator/)
[![License: Apache 2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)

A powerful table rendering library for **SixLabors.ImageSharp** that generates beautiful table images with an HTML/CSS-inspired fluent API.

## ✨ Features

- 🎨 **Rich Styling** - Full control over colors, borders, fonts, padding, and alignment
- 📊 **Advanced Layout** - Row/column spans, auto-sizing columns, text wrapping
- 🎯 **CSS-Like API** - Familiar fluent API with style cascading
- 📑 **Section Support** - Header, body, and footer sections
- 🦓 **Zebra Striping** - Built-in alternating row styles
- ⚡ **High Performance** - Font caching and optimized rendering

## 📦 Installation

\`\`\`bash
dotnet add package SixLabors.ImageSharp.TableGenerator
\`\`\`

## 🚀 Quick Start

\`\`\`csharp
using SixLabors.ImageSharp.TableGenerator;
using SixLabors.ImageSharp.TableGenerator.Builders;

var table = TableBuilder.Create()
    .Body(body => body
        .Row("Name", "Age", "City")
        .Row("John Doe", "30", "New York")
        .Row("Jane Smith", "25", "Los Angeles"))
    .Build();

var image = table.Render();
image.SaveAsPng("table.png");
\`\`\`

![Basic Table](SixLabors.ImageSharp.TableGenerator.Examples/output/basic_table.png)

## 📸 Style Showcase

### Condensed | Cozy | Roomy
![Condensed](SixLabors.ImageSharp.TableGenerator.Examples/output/example-condensed-style.png)
![Cozy](SixLabors.ImageSharp.TableGenerator.Examples/output/example-cozy-style.png)
![Roomy](SixLabors.ImageSharp.TableGenerator.Examples/output/example-roomy-style.png)

### Minimalist | Dark Mode
![Minimalist](SixLabors.ImageSharp.TableGenerator.Examples/output/example-minimalist-style.png)
![Dark Mode](SixLabors.ImageSharp.TableGenerator.Examples/output/example-dark-mode-style.png)

## 💼 Real-World Examples

### Financial Data with Color-Coded Values
![Stock Market](SixLabors.ImageSharp.TableGenerator.Examples/output/example-stock-market-large.png)

### Analytics Dashboard with Footer Totals
![Analytics](SixLabors.ImageSharp.TableGenerator.Examples/output/example-analytics-dashboard.png)

## 📚 Examples

### Styled Table
\`\`\`csharp
var table = TableBuilder.Create()
    .DefaultFont("Arial", 14)
    .CellPadding(12)
    .Border(2)
    .Style(s => s.Background("#f8f9fa").BorderColor("#dee2e6"))
    .Header(header => header
        .Style(s => s.Background("#007bff").TextColor("#ffffff").Bold())
        .Row("Product", "Price", "Stock"))
    .Body(body => body
        .Row("Laptop", "$999", "15")
        .Row("Mouse", "$25", "50"))
    .Build();
\`\`\`

![Styled](SixLabors.ImageSharp.TableGenerator.Examples/output/styled_table.png)

### Alternating Rows
\`\`\`csharp
var table = TableBuilder.Create()
    .Body(body => body
        .Row("001", "Alice", "Engineering", "$75,000")
        .Row("002", "Bob", "Marketing", "$65,000"))
    .AlternateRows(
        even => even.Background("#ffffff"),
        odd => odd.Background("#f8f9fa"))
    .Build();
\`\`\`

![Alternating](SixLabors.ImageSharp.TableGenerator.Examples/output/alternating_rows_table.png)

### Column Spans
\`\`\`csharp
var table = TableBuilder.Create()
    .Body(body => body
        .Row(row => row
            .Cell("Q1 Report", cell => cell.ColSpan(3).Style(s => s.Bold())))
        .Row("Month", "Revenue", "Growth"))
    .Build();
\`\`\`

![Spans](SixLabors.ImageSharp.TableGenerator.Examples/output/spans_table.png)

## 🎯 API Reference

### TableBuilder
- \`Create()\` - Start building a table
- \`DefaultFont(family, size)\` - Set default font
- \`CellPadding(padding)\` - Set cell padding
- \`Border(width)\` - Set border width
- \`Width(maxWidth)\` - Set maximum width
- \`Header/Body/Footer(configure)\` - Configure sections
- \`AlternateRows(even, odd)\` - Zebra striping
- \`Style(configure)\` - Apply styles

### StyleBuilder
- \`Background(color)\` / \`TextColor(color)\` / \`BorderColor(color)\`
- \`Border(width)\` / \`BorderTop/Right/Bottom/Left(width)\`
- \`FontFamily(name)\` / \`FontSize(size)\`
- \`Bold()\` / \`Italic()\`
- \`HAlign(alignment)\` / \`VAlign(alignment)\`
- \`Padding(padding)\`

### CellBuilder
- \`ColSpan(count)\` / \`RowSpan(count)\`
- \`Align(hAlign, vAlign)\`
- \`Style(configure)\`

## 🏗️ Architecture

Clean layered design:
1. **Builder API** - Fluent interface
2. **Model Layer** - Immutable records
3. **Layout Engine** - Grid positioning & text wrapping
4. **Rendering** - ImageSharp.Drawing integration

## ⚡ Performance

- Font caching
- Lazy measurement
- Optimized text wrapping
- Modern C# patterns (spans, records)

## 📄 License

Apache License 2.0

## 🤝 Contributing

Contributions welcome! Please submit Pull Requests.

## 🙏 Credits

Built on [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)

---

Made with ❤️ for .NET
