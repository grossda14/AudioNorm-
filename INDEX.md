# AudioNorm+ - Master Documentation Index

## 🎯 START HERE

Welcome to **AudioNorm+**, a professional replay gain processor for MP3 and AAC files!

This is your **master guide** to all available resources. Choose your path below:

---

## 👥 For Different Users

### 🎵 End Users (I want to use the application)
**Start here:** [QUICKSTART.md](QUICKSTART.md) (5 minutes)

Then read:
1. [INSTALLATION.md](INSTALLATION.md) - System requirements & download
2. [USAGE.md](USAGE.md) - How to use features
3. [README.md](README.md) - What is AudioNorm+?

**Quick Commands:**
```
1. Download AudioNorm+.exe
2. Run the executable
3. Add audio files
4. Analyze and apply gain
5. Enjoy normalized audio!
```

### 💻 Developers (I want to build/modify the code)
**Start here:** [GETTING_STARTED.md](GETTING_STARTED.md) (10 minutes)

Then read:
1. [BUILD.md](BUILD.md) - How to build executables
2. [CONTRIBUTING.md](CONTRIBUTING.md) - How to contribute
3. Project source code in Models/, Services/, UI/

**Quick Commands:**
```bash
git clone https://github.com/grossda14/AudioNorm-.git
cd AudioNorm-
dotnet build
dotnet run
```

### 📦 Distribution Partners (I want to distribute the app)
**Start here:** [DEPLOYMENT.md](DEPLOYMENT.md) (15 minutes)

Then read:
1. [INSTALLATION.md](INSTALLATION.md) - System requirements
2. [BUILD.md](BUILD.md) - Build executables
3. Choose distribution method (GitHub, direct download, etc.)

**Quick Summary:**
```
1. Build standalone exe
2. Upload to GitHub Releases
3. Share download link
4. Users run directly (no installation)
```

---

## 📚 Complete Documentation Map

### Quick References
| Document | Purpose | Read Time | Audience |
|----------|---------|-----------|----------|
| [README.md](README.md) | Project overview | 3 min | Everyone |
| [QUICKSTART.md](QUICKSTART.md) | 5-minute quick start | 5 min | End Users |
| [INSTALLATION.md](INSTALLATION.md) | Build & deploy guide | 10 min | Everyone |
| [CHANGELOG.md](CHANGELOG.md) | Version history | 2 min | Everyone |

### User Documentation
| Document | Purpose | Read Time | Audience |
|----------|---------|-----------|----------|
| [USAGE.md](USAGE.md) | Complete user manual | 15 min | End Users |
| [FAQ.md](FAQ.md) | Frequently asked questions | 5 min | End Users |
| [TROUBLESHOOTING.md](TROUBLESHOOTING.md) | Common issues & fixes | 10 min | End Users |

### Developer Documentation
| Document | Purpose | Read Time | Audience |
|----------|---------|-----------|----------|
| [GETTING_STARTED.md](GETTING_STARTED.md) | Developer quick start | 10 min | Developers |
| [BUILD.md](BUILD.md) | Build instructions | 10 min | Developers |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Contribution guidelines | 15 min | Contributors |
| [ARCHITECTURE.md](ARCHITECTURE.md) | System architecture | 10 min | Developers |

### Deployment Documentation
| Document | Purpose | Read Time | Audience |
|----------|---------|-----------|----------|
| [DEPLOYMENT.md](DEPLOYMENT.md) | Distribution methods | 15 min | Partners |
| [INSTALLATION.md](INSTALLATION.md) | System requirements | 10 min | Everyone |

---

## 🚀 Build & Download Paths

### Path 1: I Want to Download the Executable (Easiest)
```
1. Go to: https://github.com/grossda14/AudioNorm-/releases
2. Download: AudioNorm+.exe (standalone version)
3. Run the executable
4. Done! No installation needed
→ Read: [USAGE.md](USAGE.md)
```

### Path 2: I Want to Build the Executable Myself
```
1. Install .NET 8.0 SDK
2. Clone: git clone https://github.com/grossda14/AudioNorm-.git
3. Run: .\build.ps1 (Windows) or ./build.sh (Linux/macOS)
4. Executable created in: publish/standalone/AudioNorm+.exe
5. Run the executable
→ Read: [BUILD.md](BUILD.md) and [GETTING_STARTED.md](GETTING_STARTED.md)
```

### Path 3: I Want to Modify & Contribute
```
1. Clone repository
2. Open in Visual Studio or VS Code
3. Make changes to source code
4. Build & test: dotnet run
5. Submit pull request
→ Read: [GETTING_STARTED.md](GETTING_STARTED.md) and [CONTRIBUTING.md](CONTRIBUTING.md)
```

### Path 4: I Want to Distribute to Users
```
1. Build standalone executable
2. Upload to GitHub Releases
3. Share download link: https://github.com/grossda14/AudioNorm-/releases
4. Users download and run
5. Support users with documentation
→ Read: [DEPLOYMENT.md](DEPLOYMENT.md)
```

---

## 📁 What's in the Repository

### Core Application Code
```
AudioNorm-/
├── Models/              # Data structures (AudioFile, AnalysisMode)
├── Services/            # Business logic (Analyzer, Calculator, Applier)
├── UI/                  # Windows Forms GUI (MainForm)
├── Program.cs           # Entry point
└── AudioNorm+.csproj    # Project configuration
```

### Build Automation
```
├── build.bat            # Windows Batch script
├── build.ps1            # Windows PowerShell script
└── build.sh             # Linux/macOS Bash script
```

### Documentation (You Are Here!)
```
├── README.md            # Project overview
├── INSTALLATION.md      # This master index
├── QUICKSTART.md        # 5-minute guide
├── USAGE.md             # User manual
├── BUILD.md             # Build instructions
├── DEPLOYMENT.md        # Distribution guide
├── GETTING_STARTED.md   # Developer guide
├── CONTRIBUTING.md      # Contribution guidelines
├── CHANGELOG.md         # Version history
├── LICENSE.txt          # MIT License
└── .gitignore           # Git configuration
```

---

## ⚡ Quick Facts

| Feature | Details |
|---------|---------|
| **Language** | C# 12 (.NET 8.0) |
| **Platform** | Windows x64 only |
| **GUI** | Windows Forms |
| **Supported Formats** | MP3, AAC/M4A |
| **Gain Range** | -24dB to +24dB (0.5dB increments) |
| **Analysis Modes** | Track (individual) & Album (collective) |
| **Loudness Standard** | EBU R128 (-14 LUFS target) |
| **License** | MIT (Open Source) |
| **Version** | 1.0.0 |
| **Release Date** | 2026-07-25 |

---

## 🎯 Common Tasks & Where to Find Help

### "I want to use AudioNorm+"
→ [QUICKSTART.md](QUICKSTART.md) then [USAGE.md](USAGE.md)

### "How do I install it?"
→ [INSTALLATION.md](INSTALLATION.md)

### "The program isn't working"
→ [TROUBLESHOOTING.md](TROUBLESHOOTING.md)

### "How do I build from source?"
→ [BUILD.md](BUILD.md) and [GETTING_STARTED.md](GETTING_STARTED.md)

### "How do I modify the code?"
→ [GETTING_STARTED.md](GETTING_STARTED.md) and [CONTRIBUTING.md](CONTRIBUTING.md)

### "How do I share this with others?"
→ [DEPLOYMENT.md](DEPLOYMENT.md)

### "What changed in this version?"
→ [CHANGELOG.md](CHANGELOG.md)

### "How do I report a bug?"
→ https://github.com/grossda14/AudioNorm-/issues

### "How do I contribute?"
→ [CONTRIBUTING.md](CONTRIBUTING.md)

---

## 📋 Feature Checklist

### Implemented Features ✅
- [x] Windows Forms GUI application
- [x] MP3 file support with ID3v2 tags
- [x] AAC/M4A support with iTunes atoms
- [x] Track analysis mode (individual files)
- [x] Album analysis mode (collective)
- [x] Replay gain calculation (EBU R128 standard)
- [x] Gain adjustment slider (-24dB to +24dB, 0.5dB increments)
- [x] Automatic backup creation (.bak files)
- [x] Audio loudness analysis (LUFS)
- [x] Batch file processing
- [x] Status tracking (Pending → Analyzed → Applied)
- [x] Real-time gain preview
- [x] Metadata tag writing
- [x] File duration display
- [x] Professional UI with controls

### Build & Distribution ✅
- [x] Windows Batch build script
- [x] PowerShell build script
- [x] Linux/macOS Bash script
- [x] Standalone executable support
- [x] Runtime-dependent executable support
- [x] Release packaging ready

### Documentation ✅
- [x] User guide
- [x] Developer guide
- [x] Build instructions
- [x] Deployment guide
- [x] Contributing guidelines
- [x] API documentation (XML comments)
- [x] Quick start guide
- [x] Troubleshooting guide
- [x] This master index

---

## 🔍 Navigation by Role

### I'm a Designer
- Read: [README.md](README.md), [USAGE.md](USAGE.md)
- Explore: UI/MainForm.cs for GUI implementation
- Suggest improvements for the interface

### I'm a QA Tester
- Read: [QUICKSTART.md](QUICKSTART.md), [USAGE.md](USAGE.md)
- Test: All features with various audio files
- Report: Issues to https://github.com/grossda14/AudioNorm-/issues
- Reference: [TROUBLESHOOTING.md](TROUBLESHOOTING.md)

### I'm a Backend Developer
- Read: [GETTING_STARTED.md](GETTING_STARTED.md), [CONTRIBUTING.md](CONTRIBUTING.md)
- Explore: Services/ folder for business logic
- Improve: Audio analysis, gain calculation algorithms
- Contribute: Enhanced DSP, faster processing

### I'm a Frontend Developer
- Read: [GETTING_STARTED.md](GETTING_STARTED.md)
- Explore: UI/MainForm.cs for GUI
- Improve: Layout, styling, user experience
- Add: New UI features, dark mode, themes

### I'm a DevOps Engineer
- Read: [DEPLOYMENT.md](DEPLOYMENT.md), [BUILD.md](BUILD.md)
- Use: build.ps1, build.bat, build.sh scripts
- Deploy: Create automated CI/CD pipelines
- Distribute: GitHub Releases, package management

### I'm a System Administrator
- Read: [INSTALLATION.md](INSTALLATION.md)
- Deploy: To end-user machines
- Support: Help users troubleshoot
- Reference: [TROUBLESHOOTING.md](TROUBLESHOOTING.md)

---

## 📞 Support & Community

### Getting Help
- **Issues**: https://github.com/grossda14/AudioNorm-/issues
- **Discussions**: https://github.com/grossda14/AudioNorm-/discussions
- **Documentation**: Read the comprehensive guides above

### Contributing
- See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines
- Fork repository, make changes, submit pull request
- All contributions welcome!

### License
- **MIT License** - See [LICENSE.txt](LICENSE.txt)
- Open source and free to use, modify, distribute

---

## 🎓 Learning Resources

### Understanding Replay Gain
- [Hydrogen Audio Wiki](http://www.hydrogenaudio.org/rg_specification.html)
- [ReplayGain Specification](http://www.replaygain.org/)

### Understanding EBU R128
- [EBU Technical Documentation](https://tech.ebu.ch/loudness)
- [LUFS Explained](https://en.wikipedia.org/wiki/LUFS)

### Learning .NET & C#
- [Microsoft .NET Docs](https://docs.microsoft.com/dotnet)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Windows Forms](https://docs.microsoft.com/dotnet/desktop/winforms)

### Audio Processing
- [NAudio Library](https://github.com/naudio/NAudio)
- [TagLib# Library](https://github.com/mono/taglib-sharp)

---

## ✨ What's Next?

### For Users
1. Download or build the executable
2. Read [USAGE.md](USAGE.md)
3. Process your audio files
4. Enjoy normalized listening experience

### For Developers
1. Clone the repository
2. Read [GETTING_STARTED.md](GETTING_STARTED.md)
3. Build and run locally
4. Make improvements
5. Contribute via pull requests

### For Distributors
1. Build the standalone executable
2. Follow [DEPLOYMENT.md](DEPLOYMENT.md)
3. Share with users
4. Support with documentation

---

## 🎉 Summary

**AudioNorm+** is a complete, professional-grade replay gain processor ready for:
- ✅ End users who want to normalize their audio
- ✅ Developers who want to contribute improvements
- ✅ Distributors who want to share the application
- ✅ Administrators who want to deploy to users

**Everything you need is documented.** Choose your role above and start exploring!

---

**Questions?** Check the [FAQ.md](FAQ.md) or open an issue on GitHub.

**Ready to start?** Pick your path from the section "For Different Users" above.

**Happy Audio Processing! 🎵**
