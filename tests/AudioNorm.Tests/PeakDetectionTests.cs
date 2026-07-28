using System;
using System.Collections.Generic;
using AudioNormPlus.Models;
using AudioNormPlus.Services;
using Xunit;

namespace AudioNormPlus.Tests
{
    public class PeakDetectionTests
    {
        private readonly LoudnessMeter _meter = new LoudnessMeter();

        // ── MeasurePeak tests ──────────────────────────────────────────────────

        [Fact]
        public void MeasurePeak_ReturnsZero_ForFullScaleDcSignal()
        {
            // DC signal at +1.0 has peak = 1.0 → 20*log10(1.0) = 0.0 dBFS
            float[] samples = new float[1024];
            for (int i = 0; i < samples.Length; i++)
                samples[i] = 1.0f;

            double peak = _meter.MeasurePeak(samples);
            Assert.Equal(0.0, peak, 6);
        }

        [Fact]
        public void MeasurePeak_ReturnsMinus6_ForHalfScaleDcSignal()
        {
            // DC signal at +0.5 has peak = 0.5 → 20*log10(0.5) ≈ -6.0206 dBFS
            float[] samples = new float[1024];
            for (int i = 0; i < samples.Length; i++)
                samples[i] = 0.5f;

            double peak = _meter.MeasurePeak(samples);
            double expected = 20.0 * Math.Log10(0.5); // ≈ -6.0206
            Assert.Equal(expected, peak, 6);
        }

        [Fact]
        public void MeasurePeak_ReturnsNearZero_ForFullScaleSineWave()
        {
            // A full-scale sine wave (amplitude 1.0) has peak ≈ 1.0 → ≈ 0.0 dBFS
            int sampleCount = 44100; // 1 second at 44.1 kHz
            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
                samples[i] = (float)Math.Sin(2.0 * Math.PI * 440.0 * i / 44100.0);

            double peak = _meter.MeasurePeak(samples);
            // Peak of sine wave is very close to 1.0, so dBFS ≈ 0.0 (within 0.1 dB)
            Assert.True(peak > -0.1 && peak <= 0.0);
        }

        [Fact]
        public void MeasurePeak_ReturnsSilence_ForAllZeroSamples()
        {
            // Silence → peak = 0.0 → sentinel -200.0 dBFS
            float[] samples = new float[1024];
            double peak = _meter.MeasurePeak(samples);
            Assert.Equal(-200.0, peak);
        }

        [Fact]
        public void MeasurePeak_ReturnsSilence_ForNullInput()
        {
            double peak = _meter.MeasurePeak(null);
            Assert.Equal(-200.0, peak);
        }

        [Fact]
        public void MeasurePeak_ReturnsSilence_ForEmptyArray()
        {
            double peak = _meter.MeasurePeak(new float[0]);
            Assert.Equal(-200.0, peak);
        }

        [Fact]
        public void MeasurePeak_UsesAbsoluteValue_ForNegativeSamples()
        {
            // Peak should use absolute value, so -1.0 gives same result as +1.0
            float[] samples = new float[1024];
            for (int i = 0; i < samples.Length; i++)
                samples[i] = -1.0f;

            double peak = _meter.MeasurePeak(samples);
            Assert.Equal(0.0, peak, 6);
        }

        // ── PeakToDbfs tests ───────────────────────────────────────────────────

        [Fact]
        public void PeakToDbfs_ReturnsZero_ForLinearOne()
        {
            Assert.Equal(0.0, _meter.PeakToDbfs(1.0), 6);
        }

        [Fact]
        public void PeakToDbfs_ReturnsSilence_ForLinearZero()
        {
            Assert.Equal(-200.0, _meter.PeakToDbfs(0.0));
        }

        [Fact]
        public void PeakToDbfs_ReturnsMinus6_ForLinearHalf()
        {
            double expected = 20.0 * Math.Log10(0.5);
            Assert.Equal(expected, _meter.PeakToDbfs(0.5), 6);
        }

        // ── CalculateAlbumPeak tests ───────────────────────────────────────────

        [Fact]
        public void CalculateAlbumPeak_ReturnsMax_OfTrackPeaks()
        {
            var calc = new ReplayGainCalculator();
            var files = new List<AudioFile>
            {
                new AudioFile("a.mp3") { TrackPeak = -3.0 },
                new AudioFile("b.mp3") { TrackPeak = -1.5 },
                new AudioFile("c.mp3") { TrackPeak = -6.0 },
            };

            double albumPeak = calc.CalculateAlbumPeak(files);
            Assert.Equal(-1.5, albumPeak, 6);
        }

        [Fact]
        public void CalculateAlbumPeak_ReturnsSilence_WhenNoPeaksAvailable()
        {
            var calc = new ReplayGainCalculator();
            var files = new List<AudioFile>
            {
                new AudioFile("a.mp3"), // TrackPeak is null
            };

            double albumPeak = calc.CalculateAlbumPeak(files);
            Assert.Equal(-200.0, albumPeak);
        }

        [Fact]
        public void CalculateAlbumPeak_IgnoresFilesWithNullPeak()
        {
            var calc = new ReplayGainCalculator();
            var files = new List<AudioFile>
            {
                new AudioFile("a.mp3") { TrackPeak = -3.0 },
                new AudioFile("b.mp3"), // no peak
                new AudioFile("c.mp3") { TrackPeak = -1.5 },
            };

            double albumPeak = calc.CalculateAlbumPeak(files);
            Assert.Equal(-1.5, albumPeak, 6);
        }

        // ── AudioFile model tests ──────────────────────────────────────────────

        [Fact]
        public void AudioFile_TrackPeak_InitializesAsNull()
        {
            var file = new AudioFile("test.mp3");
            Assert.False(file.TrackPeak.HasValue);
        }

        [Fact]
        public void AudioFile_AlbumPeak_InitializesAsNull()
        {
            var file = new AudioFile("test.mp3");
            Assert.False(file.AlbumPeak.HasValue);
        }
    }
}
