using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    /// <summary>
    /// Writes ReplayGain tags to an ID3v2 tag (MP3) using TXXX user-defined text frames.
    /// Creates or overwrites the following frames:
    ///   REPLAYGAIN_TRACK_GAIN, REPLAYGAIN_TRACK_PEAK,
    ///   REPLAYGAIN_ALBUM_GAIN, REPLAYGAIN_ALBUM_PEAK (when album data is present).
    /// </summary>
    public static class Id3v2Writer
    {
        public static void WriteToTag(TagLib.Id3v2.Tag tag, ReplayGainData data)
        {
            SetFrame(tag, "REPLAYGAIN_TRACK_GAIN", ReplayGainData.FormatGain(data.TrackGain));
            SetFrame(tag, "REPLAYGAIN_TRACK_PEAK", ReplayGainData.FormatPeak(data.TrackPeak));
            if (data.AlbumGain.HasValue)
                SetFrame(tag, "REPLAYGAIN_ALBUM_GAIN", ReplayGainData.FormatGain(data.AlbumGain.Value));
            if (data.AlbumPeak.HasValue)
                SetFrame(tag, "REPLAYGAIN_ALBUM_PEAK", ReplayGainData.FormatPeak(data.AlbumPeak.Value));
        }

        private static void SetFrame(TagLib.Id3v2.Tag tag, string description, string value)
        {
            // Get existing frame or create a new one (create: true)
            var frame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, description, create: true);
            frame.Text = new[] { value };
        }
    }
}
