using System;
using System.IO;

namespace AudioNormPlus.Models
{
    public enum ProcessingStatus
    {
        Pending,
        Analyzed,
        Applied,
        Error
    }

    public enum AnalysisMode
    {
        Track,
        Album
    }

    public class AudioFile
    {
        public string FilePath { get; }
        public string FileName { get; }
        public string Format { get; }
        public TimeSpan Duration { get; set; }
        public double? LoudnessIntegrated { get; set; }
        public double? TrackPeak { get; set; }
        public double? AlbumPeak { get; set; }
        public double? CalculatedGain { get; set; }
        public double AppliedGain { get; set; }
        public ProcessingStatus Status { get; set; }

        public AudioFile(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            FileName = Path.GetFileName(filePath);
            Format = Path.GetExtension(filePath)?.TrimStart('.')?.ToUpperInvariant() ?? "UNKNOWN";
            Duration = TimeSpan.Zero;
            LoudnessIntegrated = null;
            CalculatedGain = null;
            AppliedGain = 0.0;
            Status = ProcessingStatus.Pending;
        }
    }
}
