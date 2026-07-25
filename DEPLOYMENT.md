# Deployment Guide - AudioNorm+

## Overview

This guide explains how to distribute AudioNorm+ to end users.

## Build the Application

### Step 1: Build Executable

Choose your preferred build method:

**Option A: Windows Batch Script (Easiest)**
```cmd
build.bat
```

**Option B: Windows PowerShell**
```powershell
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
.\build.ps1
```

**Option C: Manual .NET CLI**
```cmd
dotnet restore
dotnet build --configuration Release
dotnet publish -c Release -r win-x64 --self-contained false -o ./publish/win-x64
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish/standalone
```

### Step 2: Build Output

Two versions will be created:

1. **Runtime-Dependent Version**
   - Location: `publish/win-x64/AudioNorm+.exe`
   - Size: ~20 MB
   - Requirements: .NET 8.0 Runtime installed
   - Use: For users who already have .NET Runtime

2. **Standalone Version**
   - Location: `publish/standalone/AudioNorm+.exe`
   - Size: ~180 MB
   - Requirements: None
   - Use: For users without .NET Runtime (recommended for most users)

## Distribution Methods

### Method 1: Direct Download (Recommended)

**Simplest approach for end users:**

1. Host the standalone executable on GitHub Releases
2. Users download `AudioNorm+.exe`
3. Users run directly - no installation needed

**Steps:**
```bash
# Create a release on GitHub
# Upload: publish/standalone/AudioNorm+.exe
# Users download and run
```

**Pros:**
- No installation required
- Single executable file
- Works on any Windows machine

**Cons:**
- Larger file size (~180 MB)
- Longer initial download

### Method 2: Installer (Professional)

**For creating an MSI installer:**

1. **Install WiX Toolset**
   ```bash
   dotnet tool install --global wix
   ```

2. **Create WiX source file** (future enhancement)
   ```xml
   <!-- AudioNorm+.wxs -->
   <Wix>
     <!-- Installer configuration -->
   </Wix>
   ```

3. **Build MSI**
   ```bash
   wix build AudioNorm+.wxs -o AudioNorm+-Setup.msi
   ```

4. **Distribute MSI**
   - Host on GitHub Releases
   - Users download and run installer

**Pros:**
- Professional appearance
- Adds to Program Files
- Creates Start Menu shortcuts
- Can include .NET Runtime in installer

**Cons:**
- More complex to create
- Larger file if including .NET Runtime

### Method 3: Compressed Archive

**For distribution via file sharing:**

1. Copy executable to a folder
   ```bash
   mkdir AudioNorm+-v1.0.0
   copy publish/standalone/AudioNorm+.exe AudioNorm+-v1.0.0/
   copy README.md AudioNorm+-v1.0.0/
   ```

2. Create 7-Zip archive
   ```bash
   7z a AudioNorm+-v1.0.0.7z AudioNorm+-v1.0.0
   ```

3. Distribute archive
   - Users extract files
   - Users run `AudioNorm+.exe`

**Pros:**
- Smaller file size (compression)
- Can include documentation

**Cons:**
- Users must extract files

### Method 4: GitHub Releases (Recommended for Open Source)

**Automated distribution:**

1. **Tag your release**
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. **Create GitHub Release**
   - Go to https://github.com/grossda14/AudioNorm-/releases
   - Click "Create a new release"
   - Fill in version and description
   - Upload `publish/standalone/AudioNorm+.exe`
   - Publish release

3. **Users download from release page**
   - Download executable directly
   - No additional steps needed

## Pre-Deployment Checklist

Before releasing, verify:

- [ ] Application builds without errors
- [ ] All dependencies are included
- [ ] Tested with various MP3 files
- [ ] Tested with various AAC files
- [ ] Track mode analysis works correctly
- [ ] Album mode analysis works correctly
- [ ] Gain application writes tags correctly
- [ ] Backup files are created (.bak)
- [ ] No temporary files left behind
- [ ] Application closes cleanly
- [ ] Error messages are helpful
- [ ] Version number is updated in code

## System Requirements

Provide users with this information:

**For Standalone Version:**
```
Windows 7 or later
64-bit processor
No additional software required
~250 MB free disk space
```

**For Runtime-Dependent Version:**
```
Windows 7 or later
.NET 8.0 Runtime or later
Download: https://dotnet.microsoft.com/en-us/download/dotnet-runtime
64-bit processor
~50 MB free disk space
```

## Installation Instructions for Users

### Standalone Version (Recommended)

1. Download `AudioNorm+.exe`
2. Place file anywhere (Desktop, Documents, Program Files, USB drive, etc.)
3. Double-click to run
4. No additional steps needed

### Runtime-Dependent Version

1. Install .NET 8.0 Runtime from: https://dotnet.microsoft.com/en-us/download
2. Download `AudioNorm+.exe`
3. Place file anywhere
4. Double-click to run

## Troubleshooting for End Users

### "AudioNorm+.exe is not a valid Win32 application"
- Ensure using Windows x64 version
- Not compatible with 32-bit Windows
- Try standalone version if using runtime-dependent

### ".NET Runtime not found"
- Download and install: https://dotnet.microsoft.com/en-us/download/dotnet-runtime
- Or use standalone version instead

### "Windows Defender / Antivirus warning"
- Normal for new applications
- Can be added to antivirus whitelist
- Code is open source at: https://github.com/grossda14/AudioNorm-

### Application crashes on startup
- Check Windows Event Viewer for error details
- Try standalone version
- Report issue on GitHub with error details

### Gain not appearing in media player
- Check player supports ReplayGain
- Verify player has ReplayGain enabled in settings
- Try different player (Foobar2000, VLC, Clementine)

## Version Management

### Versioning Scheme
- Use Semantic Versioning: MAJOR.MINOR.PATCH
- Example: 1.0.0, 1.1.0, 2.0.0

### Update Process
1. Update version in `AudioNorm+.csproj`
2. Update `CHANGELOG.md`
3. Rebuild application
4. Test thoroughly
5. Create GitHub Release with binaries
6. Announce update to users

## Support

Provide users with support options:

- GitHub Issues: https://github.com/grossda14/AudioNorm-/issues
- GitHub Discussions: https://github.com/grossda14/AudioNorm-/discussions
- Include comprehensive README and USAGE guide

## Continuous Deployment (Optional)

For automated builds on GitHub:

Create `.github/workflows/build-release.yml` to automatically build and create releases when pushing tags.

## Analytics (Optional)

Consider tracking:
- Download counts via GitHub Releases
- User issues and bug reports
- Feature requests
- Error reports

## Legal

- Include LICENSE.txt with distribution
- MIT License allows free use
- Link to license file on GitHub

## Post-Release

After publishing:

1. Announce on relevant channels
2. Monitor GitHub Issues for problems
3. Be responsive to bug reports
4. Plan next version based on feedback
5. Maintain documentation
