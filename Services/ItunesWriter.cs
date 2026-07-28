using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    /// <summary>
    /// Writes ReplayGain tags to an iTunes/MP4 tag (AAC/M4A) using freeform "----" atoms
    /// under the "com.apple.iTunes" mean namespace.
    /// Creates or overwrites:
    ///   replaygain_track_gain, replaygain_track_peak,
    ///   replaygain_album_gain, replaygain_album_peak (when album data is present).
    /// </summary>
    public static class ItunesWriter
    {
        private const string Mean = "com.apple.iTunes";

        public static void WriteToTag(TagLib.Mpeg4.AppleTag tag, ReplayGainData data)
        {
            tag.SetDashBox(Mean, "replaygain_track_gain", ReplayGainData.FormatGain(data.TrackGain));
            tag.SetDashBox(Mean, "replaygain_track_peak", ReplayGainData.FormatPeak(data.TrackPeak));
            if (data.AlbumGain.HasValue)
                tag.SetDashBox(Mean, "replaygain_album_gain", ReplayGainData.FormatGain(data.AlbumGain.Value));
            if (data.AlbumPeak.HasValue)
                tag.SetDashBox(Mean, "replaygain_album_peak", ReplayGainData.FormatPeak(data.AlbumPeak.Value));
        }
    }
}
