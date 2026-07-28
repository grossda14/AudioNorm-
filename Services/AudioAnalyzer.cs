using System;
using System.Threading.Tasks;
using NAudio.Wave;
using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    public class AudioAnalyzer
    {
        private readonly LoudnessMeter _loudnessMeter;

        public AudioAnalyzer() : this(new LoudnessMeter()) { }

        public AudioAnalyzer(LoudnessMeter loudnessMeter)
        {
            _loudnessMeter = loudnessMeter ?? throw new ArgumentNullException(nameof(loudnessMeter));
        }

        /// <summary>
        /// Analyze the given audio file and populate Duration and LoudnessIntegrated (approximate LUFS).
        /// Uses NAudio's MediaFoundationReader to support common formats (MP3, AAC, WAV).
        /// Delegates loudness calculation to <see cref="LoudnessMeter"/> (RMS-based approximation).
        /// Samples are processed in chunks to avoid loading the full audio file into memory.
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

                double lufs = _loudnessMeter.MeasureLoudness(sumSquares, totalSamples);

                file.Duration = reader.TotalTime;
                file.LoudnessIntegrated = Math.Round(lufs, 2);
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
