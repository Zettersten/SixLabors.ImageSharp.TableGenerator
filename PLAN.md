# Problem Statement
Implement a comprehensive table rendering library for SixLabors.ImageSharp that generates table images with an HTML/CSS-inspired API. The library needs to support rows/columns, headers/footers, styling (backgrounds, borders, fonts), padding/alignment, empty cells, row/column spans, column width handling with text wrapping, and alternating row styles.
# Current State
The project is a fresh .NET 10.0 class library with:
* Empty `Class1.cs` placeholder
* No NuGet dependencies configured
* No existing implementation
Required dependencies:
* **SixLabors.ImageSharp** (version 3.x or 2.x): Core image processing library providing `Image<TPixel>`, `Color`, pixel manipulation, and save operations
* **SixLabors.ImageSharp.Drawing** (version 2.x): Extension library providing drawing primitives (`Fill`, `Draw`, shape rendering) and text rendering via `DrawText`
* **SixLabors.Fonts** (version 2.x): Font loading, text measurement via `TextMeasurer.MeasureSize()`, and text layout capabilities including wrapping and shaping
# Architecture Overview
The implementation follows a two-phase rendering pipeline:
1. **Measure Phase**: Calculate column widths and row heights by measuring text with `TextMeasurer`, resolving spans, and computing wrap behavior
2. **Render Phase**: Draw backgrounds, borders, and text to an `Image<Rgba32>` canvas using ImageSharp.Drawing APIs
Core component layers:
* **Builder API Layer**: Fluent API (`TableBuilder`, `SectionBuilder`, `RowBuilder`, `CellBuilder`, `StyleBuilder`) for constructing table structure
* **Model Layer**: Immutable data structures (`TableModel`, `TableSection`, `TableRow`, `TableCell`, `TableStyle`, `ColumnSpec`) representing table state
* **Layout Engine**: Algorithms for grid placement with spans, column width resolution (auto vs fixed), and text wrapping
* **Rendering Engine**: Draws the final image by iterating through cells and applying styles
# Proposed Changes
## 1. Project Setup
**File**: `SixLabors.ImageSharp.TableGenerator.csproj`
* Add NuGet package references for ImageSharp, ImageSharp.Drawing, and Fonts
* Target framework is already set to net10.0
## 2. Core Data Models
**File**: `Models/TableStyle.cs`
* Define `TableStyle` record with properties: `Background`, `TextColor`, `BorderColor`, `BorderWidth`, per-side borders (`BorderTop/Right/Bottom/Left`), `CellPadding`, `HAlign`, `VAlign`, `FontFamily`, `FontSize`, `FontStyle`
* Implement `Merge(TableStyle)` method for cascading styles (table → section → row → cell)
**File**: `Models/Padding.cs`
* Simple record with `Left`, `Top`, `Right`, `Bottom` properties
* Factory method `All(int x, int y)` for uniform padding
**File**: `Models/TableCell.cs`
* Properties: `Text`, `ColSpan`, `RowSpan`, `Width`, `Style`
**File**: `Models/TableRow.cs`
* Properties: `List<TableCell> Cells`, `Style`
**File**: `Models/TableSection.cs`
* Properties: `List<TableRow> Rows`, `Style`
* Used for Header, Body, and Footer sections
**File**: `Models/TableModel.cs`
* Properties: `Header`, `Body`, `Footer` (all `TableSection`), `Style`, `MaxWidth`, `List<ColumnSpec> Columns`
**File**: `Models/ColumnSpec.cs`
* Abstract record with two variants: `Auto` (fit to content) and `Fixed(float Width)`
**File**: `Models/Enums.cs`
* Define `HAlign` (Left, Center, Right) and `VAlign` (Top, Middle, Bottom) enums
## 3. Builder API
**File**: `Builders/TableBuilder.cs`
* Fluent API entry point with methods:
    * `Create()`: Static factory
    * `DefaultFont(family, size)`: Set default font
    * `CellPadding(x, y)`: Set default padding
    * `Border(width)`: Set border widths
    * `Width(maxWidth)`: Set max table width
    * `Columns(Action<ColumnsBuilder>)`: Configure column specs
    * `Header/Body/Footer(Action<SectionBuilder>)`: Configure sections
    * `AlternateRows(even, odd)`: Apply alternating styles to body rows
    * `Build()`: Return `Table` instance
**File**: `Builders/ColumnsBuilder.cs`
* Methods: `Auto()`, `Fixed(width)` to add column specifications
**File**: `Builders/SectionBuilder.cs`
* Methods: `Style(Action<StyleBuilder>)`, `Row(Action<RowBuilder>)`
**File**: `Builders/RowBuilder.cs`
* Methods: `Style(Action<StyleBuilder>)`, `Cell(text)` → returns `CellBuilder`
**File**: `Builders/CellBuilder.cs`
* Methods: `ColSpan(n)`, `RowSpan(n)`, `Width(w)`, `Align(h, v)`, `Bold()`, `Style(Action<StyleBuilder>)`
**File**: `Builders/StyleBuilder.cs`
* Methods: `Background(hex)`, `TextColor(hex)`, `BorderTop/Bottom(width)`, `Border(width)`, `Build()` → returns `TableStyle`
## 4. Layout Engine
**File**: `Layout/GridCell.cs`
* Internal struct tracking: `(RowIndex, ColIndex, RowSpan, ColSpan, TableCell)`
**File**: `Layout/MeasuredGrid.cs`
* Computed layout data: `float[] ColumnWidths`, `float[] RowHeights`, `GridCell[]`
**File**: `Layout/LayoutEngine.cs`
* `MeasureTable(TableModel)` method:
    1. Flatten sections (header, body, footer) into logical row list
    2. Resolve grid positions accounting for spans (track occupied cells)
    3. Measure column widths:
        * For `Fixed` columns: use specified width
        * For `Auto` columns: measure max intrinsic width of non-spanned cells using `TextMeasurer.MeasureSize()`
        * If `MaxWidth` set and total exceeds: proportionally shrink auto columns
    4. Measure row heights:
        * For each cell, compute text height with wrapping based on column width
        * Use `TextMeasurer.MeasureSize()` with constrained width
        * Row height = max cell height in row (accounting for spans)
    5. Return `MeasuredGrid`
**File**: `Layout/TextWrapper.cs`
* `WrapText(string text, float maxWidth, Font font)` method:
    * Split by spaces/words
    * Accumulate until measured width exceeds maxWidth
    * Emit line breaks
    * Handle single-word overflow with character-level breaking
    * Return list of lines
## 5. Rendering Engine
**File**: `Table.cs`
* Public API entry point
* Constructor: `internal Table(TableModel model)`
* `Render(RenderOptions)` method:
    1. Invoke `LayoutEngine.MeasureTable()` to get `MeasuredGrid`
    2. Calculate total image size from measured widths/heights
    3. Allocate `Image<Rgba32>` with calculated dimensions
    4. Fill background color from options
    5. Iterate through grid cells:
        * Resolve effective style via cascade: merge table → section → row → cell styles
        * Calculate cell rectangle from column widths and row heights
        * Draw cell background if specified
        * Draw borders (separate border model: draw each side independently)
        * Wrap text using `TextWrapper`
        * Draw text with alignment:
        * Load font via `SystemFonts` or custom `FontCollection`
        * Create `TextOptions` with font
        * Calculate text position based on HAlign/VAlign and padding
        * Use `image.Mutate(x => x.DrawText(...))`
    6. Return rendered image
**File**: `RenderOptions.cs`
* Record with: `Background` (default transparent), `Margin` properties
**File**: `Rendering/FontCache.cs`
* Cache loaded fonts by (family, size, style) to avoid repeated file I/O
* Use `FontCollection` and `SystemFonts` APIs
**File**: `Rendering/BorderRenderer.cs`
* Helper methods to draw individual border lines
* Use `image.Mutate(x => x.Draw(...))` with line paths
## 6. Utilities
**File**: `Utils/ColorParser.cs`
* Extension method `Color.ParseHex(string hex)` for hex color parsing
* Handle #RGB, #RRGGBB, #RRGGBBAA formats
## 7. Test Project Setup
**File**: `SixLabors.ImageSharp.TableGenerator.Tests/SixLabors.ImageSharp.TableGenerator.Tests.csproj`
* Create xUnit test project targeting net10.0
* Add project reference to main library
* Add NuGet packages: xUnit, xUnit.runner.visualstudio, Microsoft.NET.Test.Sdk, FluentAssertions
* Add ImageSharp packages for image comparison utilities
## 8. Unit Tests
**File**: `Tests/Models/TableStyleTests.cs`
* Test style merging logic (cascade behavior)
* Test default values
* Test property overrides
**File**: `Tests/Models/PaddingTests.cs`
* Test `All(x, y)` factory method
* Test individual side values
**File**: `Tests/Builders/TableBuilderTests.cs`
* Test fluent API chaining
* Test `AlternateRows()` applies styles correctly to body rows
* Test `Build()` produces valid `Table` instance
* Test builder with all sections (header, body, footer)
**File**: `Tests/Builders/CellBuilderTests.cs`
* Test `ColSpan()` and `RowSpan()` validation (minimum value 1)
* Test `Bold()` sets `FontStyle.Bold`
* Test `Align()` sets alignment properties
**File**: `Tests/Layout/GridPositionTests.cs`
* Test grid position resolution without spans (simple case)
* Test grid position resolution with ColSpan
* Test grid position resolution with RowSpan
* Test complex case: multiple spans in same table
**File**: `Tests/Layout/ColumnWidthTests.cs`
* Test fixed column width resolution
* Test auto column width (fit to content)
* Test mixed fixed + auto columns
* Test proportional shrinking when MaxWidth exceeded
**File**: `Tests/Layout/TextWrapperTests.cs`
* Test no wrapping when text fits
* Test word wrapping at space boundaries
* Test character-level breaking for long words
* Test empty string handling
* Test multi-line input preservation
**File**: `Tests/Utils/ColorParserTests.cs`
* Test parsing #RGB format
* Test parsing #RRGGBB format
* Test parsing #RRGGBBAA format
* Test invalid format error handling
## 9. Integration Tests
**File**: `Tests/Integration/BasicTableRenderingTests.cs`
* Test rendering simple 2x2 table
* Test rendering table with header only
* Test rendering table with header, body, and footer
* Validate output image dimensions
* Validate image is not null and has expected pixel count
**File**: `Tests/Integration/SpanRenderingTests.cs`
* Test rendering table with ColSpan cells
* Test rendering table with RowSpan cells
* Test rendering table with both ColSpan and RowSpan
* Validate cell positions and sizes
**File**: `Tests/Integration/StylingTests.cs`
* Test background colors render correctly
* Test border rendering (all sides)
* Test text color rendering
* Test font family and size changes
* Test alternating row styles
**File**: `Tests/Integration/TextWrappingIntegrationTests.cs`
* Test text wrapping in narrow columns
* Test text wrapping with long words
* Test row height increases with wrapped text
* Test multi-column table with varying text lengths
**File**: `Tests/Integration/AlignmentTests.cs`
* Test horizontal alignment (left, center, right)
* Test vertical alignment (top, middle, bottom)
* Test alignment with padding
* Test alignment in cells with wrapped text
## 10. Example/Demo Project
**File**: `SixLabors.ImageSharp.TableGenerator.Examples/Program.cs`
* Create console app project targeting net10.0
* Add project reference to main library
* Implement the full example from the provided specification
* Generate multiple example tables demonstrating features:
    * Basic table with header and body
    * Table with alternating row colors
    * Table with spans (col and row)
    * Table with various alignments
    * Table with text wrapping
    * Table with custom borders
* Save output images to `output/` directory
* Include comments explaining each feature
**File**: `Examples/README.md`
* Document how to run examples
* Include screenshots/descriptions of each example output
* Link to main library documentation
# Implementation Notes
* Start with **border-collapse: separate** model (each cell draws its own borders) to avoid conflict resolution complexity
* Implement `ColSpan` before `RowSpan` (RowSpan affects height distribution which is more complex)
* Use explicit style merging, not reflection-based, for performance
* Font measurement via `TextMeasurer.MeasureSize(text, TextOptions)` from SixLabors.Fonts
* Text drawing via `image.Mutate(x => x.DrawText(text, font, color, point))` from ImageSharp.Drawing
* All coordinates use float precision to match ImageSharp APIs
* Handle missing fonts gracefully by falling back to system defaults
# Key Design Decisions
1. **Style Cascade**: Table → Section → Row → Cell (matches HTML/CSS mental model)
2. **Column Width Algorithm**: Fixed columns are honored, auto columns fit content with proportional shrinking if MaxWidth exceeded
3. **Border Model**: Separate borders (v1), collapse can be added later
4. **Text Wrapping**: Greedy word wrapping with character-level fallback for long words
5. **Span Resolution**: Track occupied cells in 2D grid during layout to place spanned cells correctly
