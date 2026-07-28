using System.Globalization;

namespace AudioNormPlus.Models
{
    /// <summary>
    /// Holds the four canonical ReplayGain values that are written as metadata tags.
    /// </summary>
    public class ReplayGainData
    {
        /// <summary>Track gain in dB (e.g. -5.0 or +3.5).</summary>
        public double TrackGain { get; init; }

        /// <summary>Track peak sample value in the range 0.0–1.0.</summary>
        public double TrackPeak { get; init; }

        /// <summary>Album gain in dB. Null when writing in track-only mode.</summary>
        public double? AlbumGain { get; init; }

        /// <summary>Album peak sample value 0.0–1.0. Null when AlbumGain is null.</summary>
        public double? AlbumPeak { get; init; }

        /// <summary>Formats a gain value as "+4.00 dB" / "-3.50 dB" / "+0.00 dB".</summary>
        public static string FormatGain(double gainDb)
            => string.Format(CultureInfo.InvariantCulture, "{0:+0.00;-0.00;+0.00} dB", gainDb);

        /// <summary>Formats a peak value as a 4-decimal string (e.g. "0.9500").</summary>
        public static string FormatPeak(double peak)
            => peak.ToString("F4", CultureInfo.InvariantCulture);
    }
}
