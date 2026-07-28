using System;
using System.Collections.Generic;
using System.Linq;
using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    public class ReplayGainCalculator
    {
        // Target loudness in LUFS
        private const double TargetLufs = -14.0;

        // Calculate gain for a single track: target - measured
        public double CalculateTrackGain(AudioFile file)
        {
            if (file?.LoudnessIntegrated == null) return 0.0;
            return TargetLufs - file.LoudnessIntegrated.Value;
        }

        // Calculate album gain: compute mean loudness and return target - mean
        public double CalculateAlbumGain(IEnumerable<AudioFile> files)
        {
            var loudness = files.Where(f => f.LoudnessIntegrated.HasValue).Select(f => f.LoudnessIntegrated!.Value).ToList();
            if (!loudness.Any()) return 0.0;
            double mean = loudness.Average();
            return TargetLufs - mean;
        }

        // Calculate album peak: maximum track peak across all files
        public double CalculateAlbumPeak(IEnumerable<AudioFile> files)
        {
            var peaks = files.Where(f => f.TrackPeak.HasValue).Select(f => f.TrackPeak!.Value).ToList();
            if (!peaks.Any()) return -200.0;
            return peaks.Max();
        }

        // Normalize to 0.5 dB increments and clamp to reasonable range
        public double NormalizeGainIncrement(double gain)
        {
            double rounded = Math.Round(gain * 2.0) / 2.0; // 0.5 dB steps
            return Math.Max(-24.0, Math.Min(24.0, rounded));
        }
    }
}
