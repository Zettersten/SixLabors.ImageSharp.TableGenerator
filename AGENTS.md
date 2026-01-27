# AGENTS.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

**SixLabors.ImageSharp.TableGenerator** is a .NET library that generates table images using an HTML/CSS-inspired fluent API built on top of SixLabors.ImageSharp and ImageSharp.Drawing.

### Architecture

The library follows a **two-phase rendering pipeline**:

1. **Measure Phase**: Calculate column widths and row heights using `TextMeasurer` from SixLabors.Fonts, resolve grid positions for cells with spans, compute text wrapping
2. **Render Phase**: Draw backgrounds, borders, and text to an `Image<Rgba32>` canvas using ImageSharp.Drawing APIs

**Core component layers**:
- **Builder API** (`Builders/`): Fluent API for constructing tables (`TableBuilder`, `SectionBuilder`, `RowBuilder`, `CellBuilder`, `StyleBuilder`)
- **Model** (`Models/`): Immutable data structures (`TableModel`, `TableSection`, `TableRow`, `TableCell`, `TableStyle`, `ColumnSpec`)
- **Layout Engine** (`Layout/`): Grid positioning with spans, column width resolution (auto vs fixed), text wrapping
- **Rendering Engine** (`Table.cs`, `Rendering/`): Draws final image by iterating cells and applying cascaded styles

### Dependencies

- **SixLabors.ImageSharp**: Core image processing (`Image<TPixel>`, `Color`, pixel manipulation)
- **SixLabors.ImageSharp.Drawing**: Drawing primitives (`Fill`, `Draw`, `DrawText`)
- **SixLabors.Fonts**: Font loading, text measurement via `TextMeasurer`, text layout with wrapping

## Common Commands

### Build
```bash
dotnet build
```

### Run Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test SixLabors.ImageSharp.TableGenerator.Tests

# Run with detailed output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~TableStyleTests"
```

### Run Examples
```bash
cd SixLabors.ImageSharp.TableGenerator.Examples
dotnet run
```

### Clean Build Artifacts
```bash
dotnet clean
rm -rf bin/ obj/ */bin/ */obj/
```

### Package Creation (if needed)
```bash
dotnet pack --configuration Release
```

## Code Architecture Details

### Style Cascade System

Styles follow CSS-like cascade precedence: **Table → Section → Row → Cell**

The `TableStyle.Merge(TableStyle)` method implements cascading by taking more specific (non-default) values from the child style. This is **not reflection-based** for performance—each property is explicitly checked and merged.

When rendering, resolve effective style via:
```
effectiveStyle = tableStyle.Merge(sectionStyle).Merge(rowStyle).Merge(cellStyle)
```

### Grid Layout with Spans

Grid positioning tracks a 2D occupancy map during layout to handle `ColSpan` and `RowSpan`:
- Cells are placed left-to-right, top-to-bottom
- When a cell has `ColSpan > 1`, mark subsequent columns in that row as occupied
- When a cell has `RowSpan > 1`, mark cells in subsequent rows as occupied
- Skip occupied positions when placing next cell

This resolves the logical grid `(rowIndex, colIndex)` for each cell regardless of spans.

### Column Width Resolution Algorithm

From `Layout/LayoutEngine.cs`:

1. **Fixed columns**: Use specified width exactly
2. **Auto columns**: Measure max intrinsic width of non-spanned cells using `TextMeasurer.MeasureSize()`
3. **If `MaxWidth` constraint set and total width exceeds**:
   - Calculate overflow amount
   - Proportionally shrink only auto columns (fixed columns unchanged)
   - Each auto column shrinks by: `(columnWidth / totalAutoWidth) * overflow`
   - Apply minimum width threshold to prevent collapse

### Text Wrapping Strategy

From `Layout/TextWrapper.cs`:

Greedy word wrapping algorithm:
1. Split text by spaces into words
2. Accumulate words until `TextMeasurer.MeasureSize()` exceeds `maxWidth`
3. Emit line and continue with remaining words
4. **Single-word overflow**: If a word alone exceeds max width, break at character level
5. Return array of wrapped lines

### Font Management

`Rendering/FontCache.cs` caches fonts by `(family, size, style)` tuple to avoid repeated I/O:
- First attempt: `SystemFonts.TryGet(family, out fontFamily)`
- Fallback: Use system default (Arial or first available)
- Cache `Font` instances, not `FontFamily`, since size is commonly reused

### Border Rendering Model

**Version 1 uses border-collapse: separate**:
- Each cell draws its own borders on all four sides
- No conflict resolution between adjacent cells
- Simpler implementation, potential for double-width borders at cell boundaries

To add border-collapse support later:
- Track which borders have been drawn
- Apply CSS-like precedence rules for conflict resolution
- Consider implementing as opt-in via `RenderOptions.BorderCollapse`

## Testing Conventions

### Unit Test Structure
- Use xUnit with FluentAssertions for readable assertions
- One test class per source class (e.g., `TableStyleTests.cs` for `TableStyle.cs`)
- Test method naming: `MethodName_Scenario_ExpectedBehavior`

### Integration Test Images
- Save rendered images to `bin/Debug/net10.0/test-output/` during tests
- Use `Image.Load()` and pixel comparison for visual regression testing
- Include tolerance for font rendering differences across platforms (macOS vs Linux vs Windows)

### Testing Spans
When testing span behavior, verify:
1. Grid position is correct (no overlap)
2. Cell rectangle spans correct number of columns/rows
3. Text is positioned within spanned area (not just first cell)

## Development Notes

### Adding New Style Properties
When adding a new style property (e.g., `TextShadow`):
1. Add to `Models/TableStyle.cs` as nullable or with default
2. Update `TableStyle.Merge()` to handle new property
3. Add fluent method to `Builders/StyleBuilder.cs`
4. Update rendering logic in `Table.Render()` to apply style
5. Add unit test to `Tests/Models/TableStyleTests.cs`
6. Add integration test demonstrating visual effect

### Performance Considerations
- Text measurement is expensive: cache `TextMeasurer.MeasureSize()` results when same text/font/width measured multiple times
- Font loading is I/O bound: use `FontCache` for all font access
- Image allocation is memory intensive: dispose images properly, avoid keeping large images in memory during tests

### Platform-Specific Behavior
- **Font availability varies**: Always provide fallback font logic
- **Font rendering differs**: Anti-aliasing and hinting vary across platforms, use pixel tolerance in tests
- **System fonts**: macOS has different defaults than Linux; test with explicit font files when consistency required

## Troubleshooting

### "Font not found" errors
- Ensure font family name matches system font exactly (case-sensitive on some platforms)
- Use `SystemFonts.Families` to enumerate available fonts
- For tests, consider bundling font files and using `FontCollection.Add(path)`

### Text appears clipped or cut off
- Check padding calculation includes all border widths
- Verify `TextMeasurer.MeasureSize()` uses same `TextOptions` as `DrawText()`
- Ensure row height accounts for line spacing in multi-line cells

### Borders not appearing
- Verify border width > 0 (default is 0 for per-side borders)
- Check border color is not same as background
- Confirm `BorderRenderer` receives merged style with border properties

### Spans render incorrectly
- Debug grid position resolution: print occupancy map after each cell placement
- Verify `ColSpan` and `RowSpan` are clamped to table dimensions
- Check that spanned cell's width/height calculation sums correct columns/rows
