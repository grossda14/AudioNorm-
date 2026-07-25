# Contributing to AudioNorm+

Thank you for your interest in contributing to AudioNorm+! We welcome contributions from the community.

## How to Contribute

### Reporting Bugs
1. Check existing issues to avoid duplicates
2. Create a new issue with:
   - Clear title describing the bug
   - Steps to reproduce
   - Expected vs. actual behavior
   - OS and .NET version
   - Error message or stack trace if available

### Suggesting Features
1. Check existing issues and discussions
2. Create an issue with tag `enhancement` containing:
   - Clear feature description
   - Use case and motivation
   - Suggested implementation approach
   - Any alternatives considered

### Code Contributions

#### Setup Development Environment
```bash
# Prerequisites
- Visual Studio 2022 or later (or VS Code with C# Dev Kit)
- .NET 6.0 SDK or later
- Git

# Clone the repository
git clone https://github.com/grossda14/AudioNorm-.git
cd AudioNorm-

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

#### Development Guidelines

1. **Code Style**
   - Follow C# coding conventions
   - Use meaningful variable names
   - Add XML documentation comments for public methods
   - Keep methods focused and concise

2. **Architecture**
   - Maintain separation of concerns (UI, Services, Models)
   - Use dependency injection where appropriate
   - Write testable code

3. **Testing**
   - Test with various MP3 and AAC files
   - Test both Track and Album modes
   - Test gain application with different values
   - Verify metadata is correctly written

4. **Commits**
   - Write clear, descriptive commit messages
   - One logical change per commit
   - Reference issues in commit messages (e.g., "Fixes #42")

#### Pull Request Process

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/description`
3. Make your changes
4. Test thoroughly
5. Commit with clear messages
6. Push to your fork
7. Create a Pull Request with:
   - Clear title and description
   - Reference to related issues
   - Explanation of changes
   - Any breaking changes noted

### Areas for Contribution

- **Audio Processing**: Improve loudness calculation accuracy
- **UI/UX**: Enhance user interface and experience
- **File Format Support**: Add support for more formats (FLAC, OGG, etc.)
- **Performance**: Optimize analysis speed
- **Testing**: Write unit and integration tests
- **Documentation**: Improve README, guides, and comments
- **Localization**: Add language support
- **Bug Fixes**: Help identify and fix issues

## Code Review Process

- Maintainers will review submissions
- Changes may be requested for:
  - Code quality
  - Consistency with existing code
  - Compatibility concerns
  - Performance implications
- Be responsive to feedback
- Discussions are collaborative and constructive

## Building and Testing

```bash
# Build debug version
dotnet build

# Build release version
dotnet build -c Release

# Run the application
dotnet run

# Create portable executable
dotnet publish -c Release -r win-x64 --self-contained false
```

## Project Structure

```
AudioNorm+/
├── Models/              # Data models
│   ├── AudioFile.cs
│   └── AnalysisMode.cs
├── Services/            # Business logic
│   ├── AudioAnalyzer.cs
│   ├── ReplayGainCalculator.cs
│   └── GainApplier.cs
├── UI/                  # User interface
│   ├── MainForm.cs
│   └── MainForm.Designer.cs
├── Program.cs           # Entry point
├── AudioNorm+.csproj    # Project file
├── README.md
├── USAGE.md
└── LICENSE.txt
```

## Coding Standards

### Naming Conventions
- `PascalCase` for classes, methods, properties
- `camelCase` for local variables, parameters
- `UPPER_CASE` for constants

### Documentation
```csharp
/// <summary>
/// Brief description of what the method does
/// </summary>
/// <param name="parameter">Description of parameter</param>
/// <returns>Description of return value</returns>
public void ExampleMethod(string parameter)
{
    // Implementation
}
```

## Performance Considerations

- Analyze audio in parallel when possible
- Use async/await for I/O operations
- Minimize file I/O operations
- Cache computed values when appropriate
- Profile code before and after optimizations

## Security

- Validate all user input
- Handle file paths safely
- Don't execute arbitrary code
- Protect against directory traversal
- Validate file contents before processing

## Questions?

- Open a discussion in the GitHub repository
- Comment on related issues
- Reach out to maintainers politely

## Recognition

All contributors will be recognized in:
- CONTRIBUTORS.md file
- Release notes
- GitHub contributors page

Thank you for making AudioNorm+ better!
