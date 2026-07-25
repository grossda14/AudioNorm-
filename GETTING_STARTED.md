# AudioNorm+ - Developer Getting Started Guide

## Quick Start (5 Minutes)

### 1. Prerequisites
```bash
# Verify .NET 8.0 SDK is installed
dotnet --version
# Should output: 8.0.x
```

If not installed: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

### 2. Clone Repository
```bash
git clone https://github.com/grossda14/AudioNorm-.git
cd AudioNorm-
```

### 3. Build & Run
```bash
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Run application
dotnet run
```

The GUI launches! 🎉

## Project Architecture

### Directory Structure
```
AudioNorm-/
├── Models/
│   ├── AudioFile.cs              # Audio file data model
│   └── AnalysisMode.cs           # Track vs Album enum
├── Services/
│   ├── AudioAnalyzer.cs          # Loudness measurement
│   ├── ReplayGainCalculator.cs   # Gain computation (EBU R128)
│   └── GainApplier.cs            # ID3v2/iTunes tag writing
├── UI/
│   ├── MainForm.cs               # Main Windows Forms window
│   └── MainForm.Designer.cs      # Designer auto-generated
├── Program.cs                    # Application entry point
├── AudioNorm+.csproj             # Project configuration
├── build.bat / build.ps1 / build.sh  # Build automation
├── README.md                     # User overview
├── USAGE.md                      # User manual
├── QUICKSTART.md                 # Quick start guide
├── BUILD.md                      # Build instructions
├── DEPLOYMENT.md                 # Distribution guide
├── GETTING_STARTED.md            # This file
├── CONTRIBUTING.md               # Contribution guidelines
├── CHANGELOG.md                  # Version history
└── LICENSE.txt                   # MIT License
```

### Core Components

#### Models (Data Layer)
```csharp
// AudioFile.cs - Represents an audio file with metadata
public class AudioFile
{
    public string FilePath { get; set; }
    public double? LoudnessIntegrated { get; set; }  // LUFS
    public double? CalculatedGain { get; set; }      // dB
    public double AppliedGain { get; set; }          // dB
    public ProcessingStatus Status { get; set; }
}

// AnalysisMode.cs - Track vs Album analysis
public enum AnalysisMode { Track, Album }
```

#### Services (Business Logic)

**AudioAnalyzer.cs** - Measures audio loudness
```csharp
public class AudioAnalyzer
{
    public async Task AnalyzeFileAsync(AudioFile file)
    {
        // Reads audio data
        // Calculates loudness in LUFS
        // Updates file.LoudnessIntegrated
    }
}
```

**ReplayGainCalculator.cs** - Computes gain values
```csharp
public class ReplayGainCalculator
{
    // Formula: Gain (dB) = Target Loudness (-14 LUFS) - Measured Loudness
    public double CalculateTrackGain(AudioFile file)
    public double CalculateAlbumGain(IEnumerable<AudioFile> files)
    public double NormalizeGainIncrement(double gainDb)  // Round to 0.5dB
}
```

**GainApplier.cs** - Writes metadata tags
```csharp
public class GainApplier
{
    // Writes ReplayGain tags to:
    // - MP3: ID3v2 TXXX frames
    // - AAC: iTunes atoms
    public async Task ApplyGainAsync(AudioFile file, double gainDb)
}
```

#### UI (Presentation Layer)

**MainForm.cs** - Windows Forms GUI
```csharp
public partial class MainForm : Form
{
    private List<AudioFile> audioFiles;
    private AnalysisMode currentMode;
    private AudioAnalyzer analyzer;
    private ReplayGainCalculator calculator;
    private GainApplier applier;
    
    // User interactions:
    // - AddFiles_Click()
    // - Analyze_Click()
    // - ApplyGain_Click()
    // - Clear_Click()
}
```

## Development Workflow

### Making a Change

#### Example: Add volume peak detection

1. **Identify affected components**
   ```
   Models/ - Add PeakLevel property to AudioFile
   Services/ - Add peak detection to AudioAnalyzer
   UI/ - Display peak level in grid
   ```

2. **Edit AudioFile.cs**
   ```csharp
   public class AudioFile
   {
       public double? PeakLevel { get; set; }  // NEW
   }
   ```

3. **Edit AudioAnalyzer.cs**
   ```csharp
   private async Task<double> CalculatePeakAsync(string filePath)
   {
       // Scan audio for peak level
       return peakDb;
   }
   
   public async Task AnalyzeFileAsync(AudioFile file)
   {
       // ... existing code ...
       file.PeakLevel = await CalculatePeakAsync(file.FilePath);
   }
   ```

4. **Edit MainForm.cs**
   ```csharp
   // Add column to DataGridView
   fileGrid.Columns.Add(new DataGridViewTextBoxColumn
   {
       Name = "PeakLevel",
       HeaderText = "Peak Level (dB)",
       Width = 120
   });
   
   // Update UpdateFileGrid()
   fileGrid.Rows.Add(
       // ... existing values ...
       file.PeakLevel?.ToString("F2") ?? "—"
   );
   ```

5. **Test**
   ```bash
   dotnet run
   # Test with audio files
   # Verify peak levels display
   ```

6. **Commit**
   ```bash
   git add .
   git commit -m "Add peak level detection"
   git push origin feature/peak-detection
   ```

### Testing Guidelines

**Manual Testing Checklist**
- [ ] Load MP3 files
- [ ] Load AAC files
- [ ] Track mode analysis
- [ ] Album mode analysis
- [ ] Apply gain to files
- [ ] Verify backup files created
- [ ] Check metadata written correctly
- [ ] No crashes or unhandled exceptions
- [ ] Error messages are helpful

**Unit Testing (Future)**
```csharp
[TestClass]
public class ReplayGainCalculatorTests
{
    [TestMethod]
    public void CalculateTrackGain_ReturnsCorrectValue()
    {
        // Arrange
        var file = new AudioFile("test.mp3")
        {
            LoudnessIntegrated = -18.0  // LUFS
        };
        
        // Act
        var calculator = new ReplayGainCalculator();
        var gain = calculator.CalculateTrackGain(file);
        
        // Assert
        Assert.AreEqual(4.0, gain);  // -14 - (-18) = 4.0 dB
    }
}
```

## Code Style Guidelines

### Naming Conventions
```csharp
// Classes and public methods: PascalCase
public class ReplayGainCalculator { }
public void AnalyzeFileAsync() { }

// Properties: PascalCase
public double LoudnessIntegrated { get; set; }

// Local variables and parameters: camelCase
double measuredLoudness = -18.0;
void CalculateGain(double targetLoudness) { }

// Constants: UPPER_CASE
private const double TargetLoudness = -14.0;
```

### Documentation
```csharp
/// <summary>
/// Calculates replay gain for a single audio file.
/// </summary>
/// <param name="file">Audio file with loudness measurement</param>
/// <returns>Gain value in dB, normalized to 0.5dB increments</returns>
public double CalculateTrackGain(AudioFile file)
{
    if (file.LoudnessIntegrated == null)
        throw new InvalidOperationException("File has not been analyzed");
    
    return CalculateGain(file.LoudnessIntegrated.Value);
}
```

### Error Handling
```csharp
try
{
    await analyzer.AnalyzeFileAsync(file);
}
catch (FileNotFoundException ex)
{
    file.ErrorMessage = $"File not found: {ex.Message}";
    file.Status = ProcessingStatus.Error;
}
catch (Exception ex)
{
    file.ErrorMessage = $"Unexpected error: {ex.Message}";
    file.Status = ProcessingStatus.Error;
    // Consider logging or telemetry here
}
```

## Building for Distribution

### Release Build
```bash
# Clean rebuild
dotnet clean
dotnet restore

# Build Release configuration
dotnet build --configuration Release

# Publish standalone executable
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish/standalone

# Output: publish/standalone/AudioNorm+.exe (~180 MB)
```

### Using Build Scripts
```bash
# Windows: PowerShell
.\build.ps1

# Windows: Batch
.\build.bat

# Linux/macOS: Bash
chmod +x build.sh
./build.sh
```

## Dependencies

### NuGet Packages

**NAudio 2.2.1**
- Purpose: Read audio files, calculate loudness
- GitHub: https://github.com/naudio/NAudio
- Docs: https://naudio.github.io/

**TagLibSharp 2.2.0**
- Purpose: Read/write metadata tags
- GitHub: https://github.com/mono/taglib-sharp
- Docs: https://wiki.hydrogenaud.io/index.php?title=Taglib_Sharp

### Version Management
- Update in `AudioNorm+.csproj`
- Check for security updates regularly
- Use `dotnet list package --vulnerable`

## Advanced Topics

### Async/Await Pattern
```csharp
// Don't block UI thread during long operations
private async void Analyze_Click(object? sender, EventArgs e)
{
    foreach (var file in audioFiles)
    {
        // Doesn't block - UI stays responsive
        await analyzer.AnalyzeFileAsync(file);
        UpdateFileGrid();  // Refresh after each file
    }
}
```

### ReplayGain Algorithm
```
Integrated Loudness = Average loudness over entire file (LUFS)
Target Loudness = -14 LUFS (industry standard)
Replay Gain = Target - Integrated Loudness (in dB)

Example:
- File: -18 LUFS
- Target: -14 LUFS
- Gain: -14 - (-18) = +4 dB
- Normalized: +4.0 dB (already at 0.5dB increment)
```

### Metadata Tag Writing

**MP3 (ID3v2)**
```
Frame type: TXXX (User-defined text)
Description: REPLAYGAIN_TRACK_GAIN
Value: "+4.00 dB"
```

**AAC (iTunes)**
```
Atom name: mean
Meaning: com.apple.metadata
Item name: replaygain_track_gain
Value: "+4.00 dB"
```

## Performance Optimization

### Current Bottlenecks
1. Audio file I/O (reading large files)
2. Loudness calculation (complex DSP)
3. Metadata tag writing

### Improvement Opportunities
```csharp
// Parallel analysis
var tasks = audioFiles.Select(f => analyzer.AnalyzeFileAsync(f));
await Task.WhenAll(tasks);

// Progress reporting
private IProgress<int> analysisProgress;
await analyzer.AnalyzeFileAsync(file, analysisProgress);

// Caching
private Dictionary<string, double> loudnessCache;
```

## Debugging Tips

### Enable Verbose Logging
```csharp
// In Program.cs
System.Diagnostics.Debug.Write("AudioNorm+ started");

// In services
Debug.WriteLine($"Analyzing file: {file.FilePath}");
Debug.WriteLine($"Measured loudness: {file.LoudnessIntegrated} LUFS");
```

### Break Points
```csharp
// In Visual Studio: Click left margin to add breakpoint
if (file.LoudnessIntegrated < -30)
{
    System.Diagnostics.Debugger.Break();  // Stop here
}
```

### Common Issues

| Issue | Solution |
|-------|----------|
| Build fails | `dotnet clean && dotnet restore` |
| GUI doesn't load | Check UI/MainForm.cs and SetupUI() |
| Tags not written | Verify file permissions and format support |
| Loudness is 0 | Check audio file validity |
| Application slow | Profile code, check for blocking operations |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for:
- Code style requirements
- Pull request process
- Testing guidelines
- Commit message format

## Next Steps

1. ✅ Clone repository
2. ✅ Build project (`dotnet build`)
3. ✅ Run application (`dotnet run`)
4. ✅ Make a small change
5. ✅ Test the change
6. ✅ Commit and push
7. ✅ Submit pull request

## Resources

- .NET Documentation: https://docs.microsoft.com/dotnet
- Windows Forms: https://docs.microsoft.com/dotnet/desktop/winforms
- ReplayGain Spec: http://www.hydrogenaudio.org/rg_specification.html
- EBU R128: https://tech.ebu.ch/loudness

Happy coding! 🎵
