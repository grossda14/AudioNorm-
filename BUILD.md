# Build Instructions

## Quick Start

### Windows (Recommended)

#### Using Batch Script
```bash
build.bat
```

#### Using PowerShell
```powershell
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
.\build.ps1
```

#### Manual Build
```bash
dotnet restore
dotnet build --configuration Release
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish/win-x64
```

### macOS / Linux

```bash
chmod +x build.sh
./build.sh
```

## Requirements

- **.NET 8.0 SDK** or later
  - Download: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
  - Verify: `dotnet --version`

## Build Outputs

### Option 1: Runtime-Dependent (Recommended for Distribution)
**File**: `publish/win-x64/AudioNorm+.exe`
- **Size**: ~20 MB
- **Requirements**: .NET 8.0 Runtime (or later)
- **Installation**: Include .NET Runtime installer with setup
- **Use Case**: Smaller download, required runtime typically already installed

### Option 2: Standalone
**File**: `publish/standalone/AudioNorm+.exe`
- **Size**: ~180 MB
- **Requirements**: None (self-contained)
- **Installation**: Single file, no dependencies
- **Use Case**: Guaranteed to work on any Windows machine, no setup required

## Distribution Steps

### Option A: Simple Standalone Distribution
1. Build using scripts above
2. Copy `publish/standalone/AudioNorm+.exe` to distribution folder
3. Users can download and run directly - no installation needed

### Option B: Professional Setup Installer (Advanced)
Install WiX Toolset:
```bash
dotnet tool install --global wix
```

Create installer (future enhancement):
```bash
wix build AudioNorm+.wxs -o AudioNorm+-Setup.msi
```

### Option C: Compressed Archive
1. Copy `publish/standalone/AudioNorm+.exe`
2. Compress with 7-Zip or WinRAR
3. Distribute compressed file
4. Users extract and run

## Troubleshooting

### Error: "dotnet: command not found"
- Install .NET SDK: https://dotnet.microsoft.com/en-us/download

### Error: "The project file could not be loaded"
- Ensure you're in the correct directory
- Check that `AudioNorm+.csproj` exists

### Build succeeds but no exe produced
- Check `publish/` directory
- Ensure sufficient disk space
- Check file permissions

### .NET Runtime not found error
- Use the standalone version (Option 2)
- Or install .NET Runtime: https://dotnet.microsoft.com/en-us/download/dotnet-runtime

## Development Build

For development/debugging:
```bash
dotnet run
```

This runs the application directly without creating an executable.

## Release Checklist

- [ ] Update version in `AudioNorm+.csproj`
- [ ] Test with various MP3/AAC files
- [ ] Run `build.bat` or `build.ps1`
- [ ] Test generated executables
- [ ] Create GitHub Release with binaries
- [ ] Test download and execution
- [ ] Update CHANGELOG.md

## CI/CD Integration

GitHub Actions workflows now live in `.github/workflows/`:

- `ci.yml` - Pull request and push validation on Windows, plus Linux build validation
- `codeql.yml` - C# CodeQL security scanning with uploads to the GitHub Security tab
- `release.yml` - Manual or tag-triggered Windows x64 release publishing

### Running Workflows Locally with `act`

`act` is useful for quick Linux validation, but it cannot fully emulate `windows-latest`. In practice:

- Use `act` for the Linux build-validation job in `ci.yml`
- Use GitHub-hosted Windows runners for the Windows test and release jobs

Example:

```bash
act pull_request -W .github/workflows/ci.yml -j build-linux -P ubuntu-latest=catthehacker/ubuntu:act-latest
```

### Recommended Branch Protection

For `main` (and optionally `develop`), enable branch protection with:

- Require a pull request before merging
- Require status checks to pass before merging
- Require conversation resolution before merging
- Block direct pushes to protected branches

Recommended required checks:

- `CI / build-and-test-windows`
- `CI / build-linux`
- `CodeQL / analyze`

## Performance Tips

- Rebuild cache: `dotnet build -c Release --no-incremental`
- Clean rebuild: `dotnet clean && dotnet restore && dotnet build -c Release`
- Parallel build: `dotnet build -c Release -m`
