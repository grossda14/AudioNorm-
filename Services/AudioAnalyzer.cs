using System;
using System.Threading.Tasks;
using NAudio.Wave;
using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    public class AudioAnalyzer
    {
        /// <summary>
        /// Analyze the given audio file and populate Duration and LoudnessIntegrated (approximate LUFS).
        /// Uses NAudio's MediaFoundationReader to support common formats (MP3, AAC, WAV).
        /// The LUFS calculation here is an approximation using RMS over the whole file.
        /// For production-grade EBU R128 measurement a dedicated implementation is recommended.
        /// </summary>
        public async Task AnalyzeFileAsync(AudioFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            // Use MediaFoundationReader which supports MP3, AAC (on Windows with Media Foundation), WAV, etc.
            try
            {
                using var reader = new MediaFoundationReader(file.FilePath);
                var sampleProvider = reader.ToSampleProvider();

                const int bufferSamples = 8192;
                float[] buffer = new float[bufferSamples];
                long totalSamples = 0;
                double sumSquares = 0.0;

                int read;
                while ((read = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < read; i++)
                    {
                        double s = buffer[i];
                        sumSquares += s * s;
                    }
                    totalSamples += read;
                }

                double rms = totalSamples > 0 ? Math.Sqrt(sumSquares / totalSamples) : 0.0;

                // Convert RMS (linear) to dBFS. Avoid log of zero.
                double dbfs = rms > 0.0 ? 20.0 * Math.Log10(rms) : -200.0;

                // Approximate LUFS by treating dBFS RMS as LUFS (this is an approximation).
                // TODO: Replace with a proper EBU R128 implementation (K-weighting, gating, etc.).
                double lufsApprox = dbfs;

                file.Duration = reader.TotalTime;
                file.LoudnessIntegrated = Math.Round(lufsApprox, 2);
                file.Status = ProcessingStatus.Analyzed;
            }
            catch (Exception ex)
            {
                file.Status = ProcessingStatus.Error;
                throw new InvalidOperationException($"Failed to analyze '{file.FilePath}': {ex.Message}", ex);
            }

            await Task.CompletedTask;
        }
    }
}
