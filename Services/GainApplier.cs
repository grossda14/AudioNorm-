using System;
using System.IO;
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
        /// Current implementation writes human-readable ReplayGain info into the general Comment tag
        /// for broad compatibility. For MP3/AAC-specific frames (TXXX/iTunMOVI atoms) this can be
        /// extended to write proper ReplayGain frames.
        /// </summary>
        public async Task ApplyGainAsync(AudioFile file, double gainDb)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            try
            {
                // Create a non-destructive backup
                string backupPath = file.FilePath + ".bak";
                if (!File.Exists(backupPath))
                {
                    File.Copy(file.FilePath, backupPath);
                }

                // Use TagLib# to open and update tags
                using var tfile = TagLib.File.Create(file.FilePath);

                // Write ReplayGain information into a comment field (fallback)
                string gainText = string.Format(System.Globalization.CultureInfo.InvariantCulture, "+{0:0.0;-0.0;0.0} dB", gainDb);
                string rgText = $"REPLAYGAIN_TRACK_GAIN={gainText}";

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

        public async Task ApplyAlbumGainAsync(System.Collections.Generic.IEnumerable<AudioFile> files, double offset, double albumGain)
        {
            foreach (var file in files)
            {
                double gainToApply = file.CalculatedGain ?? albumGain;
                await ApplyGainAsync(file, gainToApply + offset);
            }
        }
    }
}
