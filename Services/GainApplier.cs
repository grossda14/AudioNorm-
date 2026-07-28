using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AudioNormPlus.Models;
using TagLib;

namespace AudioNormPlus.Services
{
    public class GainApplier
    {
        /// <summary>
        /// Apply gain to a single file by writing non-destructive metadata tags.
        /// Creates a .bak backup of the original file before modifying tags.
        /// Writes all available ReplayGain fields (track gain, track peak, album gain,
        /// album peak) into the general Comment tag for broad compatibility.
        /// </summary>
        public async Task ApplyGainAsync(AudioFile file, double gainDb, double? trackPeak = null, double? albumGain = null, double? albumPeak = null)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            try
            {
                // Create a non-destructive backup
                string backupPath = file.FilePath + ".bak";
                if (!System.IO.File.Exists(backupPath))
                {
                    System.IO.File.Copy(file.FilePath, backupPath);
                }

                // Use TagLib# to open and update tags
                using var tfile = TagLib.File.Create(file.FilePath);

                // Build ReplayGain comment with all available fields
                var ci = System.Globalization.CultureInfo.InvariantCulture;
                var sb = new StringBuilder();
                sb.Append(string.Format(ci, "REPLAYGAIN_TRACK_GAIN={0:+0.0;-0.0;0.0} dB", gainDb));
                if (trackPeak.HasValue)
                    sb.Append(string.Format(ci, " | REPLAYGAIN_TRACK_PEAK={0:F4}", trackPeak.Value));
                if (albumGain.HasValue)
                    sb.Append(string.Format(ci, " | REPLAYGAIN_ALBUM_GAIN={0:+0.0;-0.0;0.0} dB", albumGain.Value));
                if (albumPeak.HasValue)
                    sb.Append(string.Format(ci, " | REPLAYGAIN_ALBUM_PEAK={0:F4}", albumPeak.Value));
                string rgText = sb.ToString();

                // Append to existing comment rather than replacing it
                var existing = tfile.Tag.Comment;
                if (string.IsNullOrEmpty(existing))
                {
                    tfile.Tag.Comment = rgText;
                }
                else if (!existing.Contains("REPLAYGAIN_TRACK_GAIN"))
                {
                    tfile.Tag.Comment = existing + " | " + rgText;
                }

                tfile.Save();

                file.AppliedGain = gainDb;
                file.Status = ProcessingStatus.Applied;
            }
            catch (Exception ex)
            {
                file.Status = ProcessingStatus.Error;
                throw new InvalidOperationException($"Failed to apply gain to '{file.FilePath}': {ex.Message}", ex);
            }

            await Task.CompletedTask;
        }

        public async Task ApplyAlbumGainAsync(System.Collections.Generic.IEnumerable<AudioFile> files, double offset, double albumGain, double? albumPeak = null)
        {
            foreach (var file in files)
            {
                double gainToApply = file.CalculatedGain ?? albumGain;
                await ApplyGainAsync(file, gainToApply + offset, file.TrackPeak, albumGain + offset, albumPeak);
            }
        }
    }
}
