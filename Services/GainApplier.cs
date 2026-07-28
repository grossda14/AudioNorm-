using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AudioNormPlus.Models;

namespace AudioNormPlus.Services
{
    public class GainApplier
    {
        private readonly ReplayGainWriter _writer;

        public GainApplier() : this(new ReplayGainWriter()) { }

        public GainApplier(ReplayGainWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Applies track gain to a single file by writing format-native ReplayGain tags.
        /// Creates a .bak backup of the original file before modifying tags.
        /// </summary>
        /// <param name="file">The audio file to tag.</param>
        /// <param name="trackGain">Track gain in dB.</param>
        public async Task ApplyGainAsync(AudioFile file, double trackGain)
            => await ApplyGainAsync(file, trackGain, albumGain: null);

        /// <summary>
        /// Applies track and optional album gain to a single file by writing all four
        /// ReplayGain fields (track gain, track peak, album gain, album peak).
        /// Creates a .bak backup of the original file before modifying tags.
        /// </summary>
        /// <param name="file">The audio file to tag.</param>
        /// <param name="trackGain">Track gain in dB.</param>
        /// <param name="albumGain">Album gain in dB, or null to omit album fields.</param>
        public async Task ApplyGainAsync(AudioFile file, double trackGain, double? albumGain)
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

                var data = new ReplayGainData
                {
                    TrackGain = trackGain,
                    // TODO: Implement actual peak detection in AudioAnalyzer and store it on AudioFile.
                    // Until then, 1.0 (0 dBFS) is used as a conservative placeholder — it tells
                    // players that no headroom is guaranteed and prevents false peak-based normalization.
                    TrackPeak = 1.0,
                    AlbumGain = albumGain,
                    AlbumPeak = albumGain.HasValue ? 1.0 : null
                };

                using var tfile = TagLib.File.Create(file.FilePath);
                _writer.WriteGainToFile(tfile, file.FilePath, data);
                tfile.Save();

                file.AppliedGain = trackGain;
                file.Status = ProcessingStatus.Applied;
            }
            catch (Exception ex)
            {
                file.Status = ProcessingStatus.Error;
                throw new InvalidOperationException($"Failed to apply gain to '{file.FilePath}': {ex.Message}", ex);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Applies album-mode gain to a collection of files.
        /// Writes both track gain (per-file calculated gain + slider offset) and album gain
        /// (shared across all files) into the format-native ReplayGain fields.
        /// </summary>
        /// <param name="files">Files to process.</param>
        /// <param name="sliderOffset">Additional dB offset from the UI gain slider.</param>
        public async Task ApplyAlbumGainAsync(IEnumerable<AudioFile> files, double sliderOffset)
        {
            var fileList = files.ToList();
            // In album mode every file shares the same CalculatedGain (set by ReplayGainCalculator.CalculateAlbumGain).
            // We derive the shared album gain from the first analysed file; all others should have the same value.
            double baseAlbumGain = fileList.FirstOrDefault(f => f.CalculatedGain.HasValue)?.CalculatedGain ?? 0.0;
            double effectiveAlbumGain = baseAlbumGain + sliderOffset;

            foreach (var file in fileList)
            {
                double trackGain = (file.CalculatedGain ?? baseAlbumGain) + sliderOffset;
                await ApplyGainAsync(file, trackGain, effectiveAlbumGain);
            }
        }
    }
}
