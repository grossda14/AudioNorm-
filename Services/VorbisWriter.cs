using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    /// <summary>
    /// Writes ReplayGain tags to a Vorbis comment block (FLAC / Ogg Vorbis) using the
    /// standard uppercase field names defined in the ReplayGain specification.
    /// Creates or overwrites:
    ///   REPLAYGAIN_TRACK_GAIN, REPLAYGAIN_TRACK_PEAK,
    ///   REPLAYGAIN_ALBUM_GAIN, REPLAYGAIN_ALBUM_PEAK (when album data is present).
    /// </summary>
    public static class VorbisWriter
    {
        public static void WriteToTag(TagLib.Ogg.XiphComment tag, ReplayGainData data)
        {
            tag.SetField("REPLAYGAIN_TRACK_GAIN", new[] { ReplayGainData.FormatGain(data.TrackGain) });
            tag.SetField("REPLAYGAIN_TRACK_PEAK", new[] { ReplayGainData.FormatPeak(data.TrackPeak) });
            if (data.AlbumGain.HasValue)
                tag.SetField("REPLAYGAIN_ALBUM_GAIN", new[] { ReplayGainData.FormatGain(data.AlbumGain.Value) });
            if (data.AlbumPeak.HasValue)
                tag.SetField("REPLAYGAIN_ALBUM_PEAK", new[] { ReplayGainData.FormatPeak(data.AlbumPeak.Value) });
        }
    }
}
