using System.IO;
using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    /// <summary>
    /// Orchestrates format-specific ReplayGain tag writing.
    /// Detects the audio format from the file extension and delegates to the appropriate
    /// writer (<see cref="Id3v2Writer"/>, <see cref="ItunesWriter"/>, or <see cref="VorbisWriter"/>).
    /// Falls back to a plain comment entry when the format is not recognised.
    /// </summary>
    public class ReplayGainWriter
    {
        /// <summary>
        /// Writes ReplayGain tags to an already-opened <paramref name="tfile"/>.
        /// The caller is responsible for calling <c>tfile.Save()</c> after this method returns.
        /// </summary>
        public void WriteGainToFile(TagLib.File tfile, string filePath, ReplayGainData data)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            bool written = false;

            switch (ext)
            {
                case ".mp3":
                    if (tfile.GetTag(TagLib.TagTypes.Id3v2, true) is TagLib.Id3v2.Tag id3v2Tag)
                    {
                        Id3v2Writer.WriteToTag(id3v2Tag, data);
                        written = true;
                    }
                    break;

                case ".m4a":
                case ".aac":
                    if (tfile.GetTag(TagLib.TagTypes.Apple, true) is TagLib.Mpeg4.AppleTag appleTag)
                    {
                        ItunesWriter.WriteToTag(appleTag, data);
                        written = true;
                    }
                    break;

                case ".flac":
                case ".ogg":
                    if (tfile.GetTag(TagLib.TagTypes.Xiph, true) is TagLib.Ogg.XiphComment xiphTag)
                    {
                        VorbisWriter.WriteToTag(xiphTag, data);
                        written = true;
                    }
                    break;
            }

            if (!written)
            {
                WriteFallbackComment(tfile, data);
            }
        }

        private static void WriteFallbackComment(TagLib.File tfile, ReplayGainData data)
        {
            string gainText = ReplayGainData.FormatGain(data.TrackGain);
            string rgText = $"REPLAYGAIN_TRACK_GAIN={gainText}";
            var existing = tfile.Tag.Comment;
            if (string.IsNullOrEmpty(existing))
            {
                tfile.Tag.Comment = rgText;
            }
            else if (!existing.Contains("REPLAYGAIN_TRACK_GAIN"))
            {
                tfile.Tag.Comment = existing + " | " + rgText;
            }
        }
    }
}
