# AudioNorm+ Build Script (PowerShell)
# Builds the application for Windows x64

Write-Host "========================================"
Write-Host "AudioNorm+ Build Script"
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Check if .NET SDK is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host "✓ .NET SDK found: $dotnetVersion" -ForegroundColor Green
}
catch {
    Write-Host "✗ ERROR: .NET SDK not found" -ForegroundColor Red
    Write-Host "Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/en-us/download" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "[1/5] Cleaning previous builds..." -ForegroundColor Cyan
dotnet clean --configuration Release --nologo --verbosity quiet 2>$null

Write-Host "[2/5] Restoring NuGet packages..." -ForegroundColor Cyan
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ ERROR: Failed to restore packages" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "[3/5] Building project (Release)..." -ForegroundColor Cyan
dotnet build --configuration Release --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ ERROR: Build failed" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "[4/5] Publishing executable (requires .NET runtime)..." -ForegroundColor Cyan
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish/win-x64
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ ERROR: Publish failed" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "[5/5] Creating standalone executable..." -ForegroundColor Cyan
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish/standalone
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ ERROR: Standalone publish failed" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "========================================"
Write-Host "Build Complete!" -ForegroundColor Green
Write-Host "========================================"
Write-Host ""
Write-Host "Executables created:" -ForegroundColor Yellow
Write-Host "  1. Runtime-Dependent (smaller): publish\win-x64\AudioNorm+.exe" -ForegroundColor White
Write-Host "     - Requires .NET Runtime to be installed" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Standalone (larger): publish\standalone\AudioNorm+.exe" -ForegroundColor White
Write-Host "     - Works without .NET Runtime installed" -ForegroundColor Gray
Write-Host ""
Write-Host "Ready to distribute!" -ForegroundColor Green
Read-Host "Press Enter to exit"
