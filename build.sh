#!/bin/bash
# AudioNorm+ Build Script (Linux/macOS)
# Builds the application for Windows x64

echo "========================================"
echo "AudioNorm+ Build Script"
echo "========================================"
echo ""

# Check if .NET SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found"
    echo "Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/en-us/download"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo "✓ .NET SDK found: $DOTNET_VERSION"
echo ""

echo "[1/5] Cleaning previous builds..."
dotnet clean --configuration Release --nologo --verbosity quiet 2>/dev/null

echo "[2/5] Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore packages"
    exit 1
fi

echo "[3/5] Building project (Release)..."
dotnet build --configuration Release --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed"
    exit 1
fi

echo "[4/5] Publishing executable (requires .NET runtime)..."
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish/win-x64
if [ $? -ne 0 ]; then
    echo "ERROR: Publish failed"
    exit 1
fi

echo "[5/5] Creating standalone executable..."
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish/standalone
if [ $? -ne 0 ]; then
    echo "ERROR: Standalone publish failed"
    exit 1
fi

echo ""
echo "========================================"
echo "Build Complete!"
echo "========================================"
echo ""
echo "Executables created:"
echo "  1. Runtime-Dependent: publish/win-x64/AudioNorm+.exe"
echo "     - Requires .NET Runtime to be installed"
echo ""
echo "  2. Standalone: publish/standalone/AudioNorm+.exe"
echo "     - Works without .NET Runtime installed"
echo ""
echo "Ready to distribute!"
