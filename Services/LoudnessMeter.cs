using System;

namespace AudioNormPlus.Services
{
    /// <summary>
    /// Measures loudness from audio samples using an RMS-based approach.
    /// Formula: LUFS ≈ 20 * log10(RMS) where RMS is the root mean square of all
    /// audio samples normalized to [-1.0, 1.0]. This is an approximation of
    /// integrated loudness in Loudness Units relative to Full Scale (LUFS/dBFS).
    /// For production-grade EBU R128 measurement, K-weighting and gating would
    /// be required.
    /// </summary>
    public class LoudnessMeter
    {
        // Sentinel value returned for silence or invalid input
        private const double SilenceDbfs = -200.0;

        /// <summary>
        /// Calculates the approximate integrated loudness (LUFS) from pre-computed
        /// sum-of-squares and sample count (for memory-efficient streaming).
        /// </summary>
        /// <param name="sumSquares">Sum of squares of all audio samples</param>
        /// <param name="sampleCount">Total number of samples processed</param>
        /// <returns>
        /// Approximate LUFS value as a negative dBFS number (e.g. -18.0),
        /// or -200.0 for silence or zero sample count.
        /// </returns>
        public double MeasureLoudness(double sumSquares, long sampleCount)
        {
            if (sampleCount <= 0) return -200.0;
            double rms = Math.Sqrt(sumSquares / sampleCount);
            return rms > 0.0 ? 20.0 * Math.Log10(rms) : -200.0;
        }

        /// <summary>
        /// Calculates the approximate integrated loudness (LUFS) from an array of
        /// normalized audio samples. Samples must be in the range [-1.0, 1.0].
        /// </summary>
        /// <param name="samples">Audio samples normalized to [-1.0, 1.0]</param>
        /// <returns>
        /// Approximate LUFS value as a negative dBFS number (e.g. -18.0),
        /// or -200.0 for silence or empty input.
        /// </returns>
        public double MeasureLoudness(float[] samples)
        {
            if (samples == null || samples.Length == 0)
                return SilenceDbfs;

            double sumSquares = 0.0;
            for (int i = 0; i < samples.Length; i++)
            {
                double s = samples[i];
                sumSquares += s * s;
            }

            double rms = Math.Sqrt(sumSquares / samples.Length);
            return rms > 0.0 ? 20.0 * Math.Log10(rms) : SilenceDbfs;
        }
    }
}
