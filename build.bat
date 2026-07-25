@echo off
REM AudioNorm+ Build Script
REM Builds the application for Windows x64

echo ========================================
echo AudioNorm+ Build Script
echo ========================================
echo.

REM Check if .NET SDK is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK not found. Please install .NET 8.0 SDK
    echo Download from: https://dotnet.microsoft.com/en-us/download
    pause
    exit /b 1
)

echo [1/5] Cleaning previous builds...
dotnet clean --configuration Release >nul 2>&1

echo [2/5] Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo [3/5] Building project...
dotnet build --configuration Release --no-restore
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo [4/5] Publishing executable...
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish/win-x64
if errorlevel 1 (
    echo ERROR: Publish failed
    pause
    exit /b 1
)

echo [5/5] Creating standalone version...
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish/standalone
if errorlevel 1 (
    echo ERROR: Standalone publish failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo Build Complete!
echo ========================================
echo.
echo Executables created:
echo   - Requires .NET Runtime: publish\win-x64\AudioNorm+.exe
echo   - Standalone: publish\standalone\AudioNorm+.exe
echo.
echo Ready to distribute!
pause
