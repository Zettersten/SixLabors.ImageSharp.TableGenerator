# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2026-01-27

### Added
- Initial release of BlazorFast.ImageSharp.TableGenerator
- Fluent API for building tables with TableBuilder, StyleBuilder, CellBuilder, RowBuilder
- Comprehensive styling system with CSS-like cascading (Table → Section → Row → Cell)
- Support for header, body, and footer sections
- Row and column span support
- Fixed and auto-sizing columns with max-width constraints
- Text wrapping with word and character-level fallback
- Horizontal alignment (Left, Center, Right)
- Vertical alignment (Top, Middle, Bottom)
- Border styling with per-side control
- Background colors, text colors, and border colors (hex and Color support)
- Font customization (family, size, bold, italic)
- Padding control at table, section, row, and cell levels
- Alternating row styles (zebra striping)
- Font caching for performance
- Comprehensive test suite (153 tests)
- Style variation examples (condensed, cozy, roomy, minimalist, dark mode)
- Large data table examples (financial data, analytics dashboard)
- Complete documentation and README
- Husky.Net pre-commit hooks for code quality
- CSharpier code formatting

### Performance
- Font caching to avoid repeated I/O
- Lazy layout measurement
- Optimized text wrapping algorithms
- Efficient grid position tracking for spans
- `Padding` and `FontKey` implemented as readonly structs for reduced heap allocations
- AggressiveInlining attributes on hot paths (TableStyle.Merge, FontCache.GetFont, Padding factory methods)

### Documentation
- NuGet-ready README with showcase images
- API reference documentation
- Architecture overview
- 12 example images demonstrating various styles
- Code quality documentation

[Unreleased]: https://github.com/YOUR_USERNAME/BlazorFast.ImageSharp.TableGenerator/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/YOUR_USERNAME/BlazorFast.ImageSharp.TableGenerator/releases/tag/v1.0.0
