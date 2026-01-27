# NuGet Publishing Guide

This document explains how to publish `SixLabors.ImageSharp.TableGenerator` to NuGet.org.

## Prerequisites

1. **NuGet.org Account**: Create an account at [nuget.org](https://www.nuget.org/)
2. **API Key**: Generate an API key from your NuGet.org account settings
3. **GitHub Repository**: Push the code to a GitHub repository
4. **GitHub Secrets**: Add `NUGET_API_KEY` to repository secrets

## Local Testing

Before publishing, test the package locally:

```bash
# Build the package
dotnet pack --configuration Release --output ./artifacts

# Inspect package contents
unzip -l ./artifacts/SixLabors.ImageSharp.TableGenerator.1.0.0.nupkg

# Test locally by adding the artifacts folder as a NuGet source
dotnet nuget add source ./artifacts --name local-test

# Create a test project and install
dotnet new console -n TestPackage
cd TestPackage
dotnet add package SixLabors.ImageSharp.TableGenerator --version 1.0.0 --source local-test
```

## Publishing Steps

### Option 1: Manual Publishing

```bash
# Build the package
dotnet pack --configuration Release --output ./artifacts

# Push to NuGet.org (replace YOUR_API_KEY)
dotnet nuget push ./artifacts/SixLabors.ImageSharp.TableGenerator.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

### Option 2: Automated GitHub Actions Release

The repository includes automated release workflows. To publish:

1. **Update version** in `SixLabors.ImageSharp.TableGenerator.csproj`:
   ```xml
   <Version>1.0.0</Version>
   ```

2. **Update CHANGELOG.md** with release notes for the version

3. **Commit and push changes**:
   ```bash
   git add .
   git commit -m "Release v1.0.0"
   git push origin main
   ```

4. **Create and push a tag**:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

5. **GitHub Actions will automatically**:
   - Build the project
   - Run all tests
   - Create NuGet packages (.nupkg and .snupkg)
   - Publish to NuGet.org
   - Create a GitHub Release with artifacts

## GitHub Secrets Configuration

Add the following secret to your GitHub repository:

1. Go to repository **Settings → Secrets and variables → Actions**
2. Click **New repository secret**
3. Name: `NUGET_API_KEY`
4. Value: Your NuGet.org API key
5. Click **Add secret**

## Post-Publishing

After publishing:

1. **Verify on NuGet.org**: Check that the package appears at `https://www.nuget.org/packages/SixLabors.ImageSharp.TableGenerator`
2. **Test installation**: Try installing in a fresh project:
   ```bash
   dotnet add package SixLabors.ImageSharp.TableGenerator
   ```
3. **Update repository URLs**: Replace `YOUR_USERNAME` placeholders in:
   - `SixLabors.ImageSharp.TableGenerator.csproj`
   - `CHANGELOG.md`
   - `README.md`

## Version Management

This project follows [Semantic Versioning](https://semver.org/):

- **MAJOR** (1.x.x): Breaking API changes
- **MINOR** (x.1.x): New features, backward compatible
- **PATCH** (x.x.1): Bug fixes, backward compatible

Update version in:
1. `SixLabors.ImageSharp.TableGenerator.csproj` → `<Version>` property
2. `CHANGELOG.md` → Add new version section
3. Git tag → `vX.Y.Z`

## Package Contents Checklist

Before publishing, verify the package includes:

- ✅ DLL: `SixLabors.ImageSharp.TableGenerator.dll`
- ✅ XML docs: `SixLabors.ImageSharp.TableGenerator.xml`
- ✅ README: `README.md`
- ✅ Icon: `icon.png`
- ✅ Symbol package: `.snupkg` for debugging
- ✅ License: Apache-2.0 identifier
- ✅ SourceLink: Enabled for source debugging

## Troubleshooting

### Package validation errors
- Run `dotnet pack` locally first to catch errors
- Check that `icon.png` and `README.md` exist
- Verify all PackageReference versions are available

### SourceLink warnings
- Normal if repository has no remote configured locally
- These warnings won't appear in GitHub Actions CI

### Symbol package issues
- Ensure `<IncludeSymbols>true</IncludeSymbols>` is set
- Verify `<SymbolPackageFormat>snupkg</SymbolPackageFormat>` is configured

## Additional Resources

- [NuGet Package Documentation](https://docs.microsoft.com/en-us/nuget/)
- [Creating NuGet Packages](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package)
- [SourceLink Documentation](https://github.com/dotnet/sourcelink)
- [Keep a Changelog](https://keepachangelog.com/)
