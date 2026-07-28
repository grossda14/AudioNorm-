using System;
using AudioNormPlus.Services;
using Xunit;

namespace AudioNormPlus.Tests
{
    public class LoudnessMeterTests
    {
        private readonly LoudnessMeter _meter = new LoudnessMeter();

        [Fact]
        public void MeasureLoudness_ReturnsLowValue_ForSilence()
        {
            // All-zero samples represent silence
            float[] samples = new float[1024];
            double lufs = _meter.MeasureLoudness(samples);
            Assert.Equal(-200.0, lufs);
        }

        [Fact]
        public void MeasureLoudness_ReturnsLowValue_ForNullInput()
        {
            double lufs = _meter.MeasureLoudness(null);
            Assert.Equal(-200.0, lufs);
        }

        [Fact]
        public void MeasureLoudness_ReturnsLowValue_ForEmptyArray()
        {
            double lufs = _meter.MeasureLoudness(new float[0]);
            Assert.Equal(-200.0, lufs);
        }

        [Fact]
        public void MeasureLoudness_ReturnsZero_ForFullScaleDcSignal()
        {
            // A DC signal at amplitude 1.0 has RMS = 1.0, so LUFS = 20*log10(1.0) = 0 dBFS
            float[] samples = new float[1024];
            for (int i = 0; i < samples.Length; i++)
                samples[i] = 1.0f;

            double lufs = _meter.MeasureLoudness(samples);
            Assert.Equal(0.0, lufs, 6);
        }

        [Fact]
        public void MeasureLoudness_ReturnsMinus6_ForHalfAmplitudeDcSignal()
        {
            // A DC signal at amplitude 0.5 has RMS = 0.5, LUFS = 20*log10(0.5) ≈ -6.0206 dBFS
            float[] samples = new float[1024];
            for (int i = 0; i < samples.Length; i++)
                samples[i] = 0.5f;

            double lufs = _meter.MeasureLoudness(samples);
            double expected = 20.0 * Math.Log10(0.5);
            Assert.Equal(expected, lufs, 6);
        }

        [Fact]
        public void MeasureLoudness_ReturnsMinus3_ForFullScaleSineWave()
        {
            // A full-scale sine wave has RMS = 1/sqrt(2) ≈ 0.7071, LUFS ≈ -3.0103 dBFS
            int sampleCount = 44100; // 1 second at 44.1 kHz
            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
                samples[i] = (float)Math.Sin(2.0 * Math.PI * 440.0 * i / 44100.0);

            double lufs = _meter.MeasureLoudness(samples);
            double expected = 20.0 * Math.Log10(1.0 / Math.Sqrt(2.0)); // ≈ -3.0103
            Assert.Equal(expected, lufs, 2);
        }

        [Fact]
        public void MeasureLoudness_NegativeResult_ForNormalAudio()
        {
            // Normal audio has loudness below 0 dBFS
            int sampleCount = 4096;
            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
                samples[i] = (float)(0.1 * Math.Sin(2.0 * Math.PI * i / 100.0));

            double lufs = _meter.MeasureLoudness(samples);
            Assert.True(lufs < 0.0);
        }
    }
}
