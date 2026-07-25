# AudioNorm+ - Quick Start Guide

## For End Users (Running the Application)

### System Requirements
- **Windows 7 or later** (64-bit)
- **One of the following:**
  - Standalone version: No additional software needed
  - Runtime version: .NET 8.0 Runtime installed

### Installation (5 seconds)

1. **Download** `AudioNorm+.exe` from GitHub Releases
2. **Run** the executable - no installation needed
3. **Done!** Application launches immediately

### First Use

#### Adding Audio Files
1. Click **"Add Files"** button
2. Select one or more MP3 or AAC files
3. Files appear in the table below

#### Analyzing Audio
1. Choose **analysis mode:**
   - **Track**: Each file analyzed separately
   - **Album**: All files analyzed together
2. Click **"Analyze"** button
3. Wait for analysis to complete
4. View calculated gain in "Calc. Gain" column

#### Applying Gain
1. Use **gain slider** to adjust (-24dB to +24dB, 0.5dB increments)
2. Preview gain value shown in real-time
3. Click **"Apply Gain"** button
4. Gain is written to file metadata (non-destructive)
5. Check "Status" column for confirmation

#### Viewing Results
- **Loudness (LUFS)** - Measured loudness of file
- **Calc. Gain (dB)** - Recommended gain adjustment
- **Applied Gain (dB)** - Gain that was applied
- **Status** - Processing status (Pending, Analyzed, Applied, Error)

### Tips

✅ **Best Practices:**
- Backup important files before first use
- Test with a single file first
- Use Album mode for complete albums
- Use Track mode for mixed sources

❌ **What Not To Do:**
- Don't delete `.bak` files (those are backups)
- Don't apply extreme gains (> ±12dB)
- Don't mix multiple albums in one analysis session

### Playback

After applying gain, listen in a compatible player:
- **Foobar2000** (with ReplayGain plugin)
- **VLC Media Player**
- **Clementine**
- Many other modern media players

Most players default to automatic ReplayGain if tags are present.

### Troubleshooting

**"Cannot read file"**
- Verify file is a valid MP3 or AAC
- Try copying file to different location
- Check file permissions

**"Gain not heard in player"**
- Check player supports ReplayGain
- Enable ReplayGain in player settings
- Try different player application

**"Application won't start"**
- Ensure Windows 7 or later (64-bit)
- If runtime-dependent: Install .NET Runtime
- Try standalone version instead

### Getting Help
- Visit: https://github.com/grossda14/AudioNorm-
- Open an issue if you find a bug
- Check documentation on GitHub

---

## For Developers (Building from Source)

### Prerequisites
1. **Install .NET 8.0 SDK**
   - Download: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
   - Verify: `dotnet --version`

2. **Clone Repository**
   ```bash
   git clone https://github.com/grossda14/AudioNorm-.git
   cd AudioNorm-
   ```

### Building

**Option 1: Quick Build (Windows)**
```bash
.\build.ps1
```

**Option 2: Manual Build**
```bash
dotnet restore
dotnet build --configuration Release
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish/standalone
```

### Outputs

**Standalone Executable** (Recommended)
```
publish/standalone/AudioNorm+.exe
```
- No dependencies required
- Works on any Windows machine
- Ready to distribute

**Runtime-Dependent Executable** (Smaller)
```
publish/win-x64/AudioNorm+.exe
```
- Requires .NET 8.0 Runtime installed
- Smaller file size
- Faster startup

### Running Locally

```bash
dotnet run
```

This launches the GUI without creating an executable.

### Project Structure

```
AudioNorm-/
├── Models/           # Data classes
├── Services/         # Business logic
├── UI/              # Windows Forms GUI
├── build.bat        # Windows batch build script
├── build.ps1        # Windows PowerShell script
├── build.sh         # Linux/macOS script
└── *.md             # Documentation
```

### Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | Application entry point |
| `UI/MainForm.cs` | Main GUI window |
| `Services/AudioAnalyzer.cs` | Loudness calculation |
| `Services/ReplayGainCalculator.cs` | Gain math |
| `Services/GainApplier.cs` | Tag writing |
| `Models/AudioFile.cs` | Audio file model |

### Making Changes

1. Edit source file
2. Rebuild: `dotnet build`
3. Test: `dotnet run`
4. Commit: `git add . && git commit -m "message"`
5. Push: `git push origin main`

### Common Commands

```bash
# Clean build
dotnet clean && dotnet build

# Run application
dotnet run

# Build release
dotnet build --configuration Release

# Create standalone exe
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish

# Format code
dotnet format

# List dependencies
dotnet list package
```

### Dependencies

- **NAudio 2.2.1** - Audio file I/O
- **TagLibSharp 2.2.0** - Metadata manipulation

Both installed automatically via NuGet.

---

## Distribution Steps

### Step 1: Build Executable
```bash
.\build.ps1
# Creates: publish/standalone/AudioNorm+.exe
```

### Step 2: Test Executable
- Download the `.exe` file
- Run on clean Windows machine
- Test with various audio files
- Verify no errors or crashes

### Step 3: Create GitHub Release
1. Go to: https://github.com/grossda14/AudioNorm-/releases
2. Click "New Release"
3. Tag: `v1.0.0`
4. Title: `AudioNorm+ v1.0.0`
5. Upload: `publish/standalone/AudioNorm+.exe`
6. Publish

### Step 4: Share Download Link
Users can download from:
```
https://github.com/grossda14/AudioNorm-/releases/download/v1.0.0/AudioNorm+.exe
```

---

## Version Info

- **Current Version**: 1.0.0
- **.NET Target**: 8.0
- **Platform**: Windows x64
- **Architecture**: 64-bit only
- **GUI Framework**: Windows Forms

---

## Next Steps

### For Users
1. ✅ Download executable
2. ✅ Run application
3. ✅ Add audio files
4. ✅ Analyze and apply gain
5. ✅ Enjoy normalized audio!

### For Developers
1. ✅ Clone repository
2. ✅ Install .NET SDK
3. ✅ Build project
4. ✅ Run locally
5. ✅ Make improvements
6. ✅ Submit pull requests

---

## Resources

- **User Guide**: [USAGE.md](USAGE.md)
- **Build Instructions**: [BUILD.md](BUILD.md)
- **Deployment Guide**: [DEPLOYMENT.md](DEPLOYMENT.md)
- **Developer Guide**: [GETTING_STARTED.md](GETTING_STARTED.md)
- **Contributing**: [CONTRIBUTING.md](CONTRIBUTING.md)
- **GitHub**: https://github.com/grossda14/AudioNorm-

---

**Need Help?** Open an issue on GitHub or check the documentation!
