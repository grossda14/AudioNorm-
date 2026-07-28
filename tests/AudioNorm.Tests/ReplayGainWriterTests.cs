using System;
using AudioNormPlus.Models;
using AudioNormPlus.Services;
using Xunit;

namespace AudioNormPlus.Tests
{
    /// <summary>
    /// Unit tests for format-specific ReplayGain tag writers.
    /// These tests operate directly on in-memory tag objects so no real audio files are required.
    /// </summary>
    public class ReplayGainWriterTests
    {
        // ------------------------------------------------------------------ helpers

        private static ReplayGainData TrackOnlyData(double trackGain = -5.0, double trackPeak = 0.95)
            => new ReplayGainData { TrackGain = trackGain, TrackPeak = trackPeak };

        private static ReplayGainData AlbumData(double trackGain = -5.0, double albumGain = -4.0,
            double trackPeak = 0.95, double albumPeak = 0.90)
            => new ReplayGainData
            {
                TrackGain = trackGain,
                TrackPeak = trackPeak,
                AlbumGain = albumGain,
                AlbumPeak = albumPeak
            };

        // ------------------------------------------------------------------ ReplayGainData format helpers

        [Fact]
        public void FormatGain_PositiveValue_HasPlusSign()
        {
            string result = ReplayGainData.FormatGain(4.5);
            Assert.Equal("+4.50 dB", result);
        }

        [Fact]
        public void FormatGain_NegativeValue_HasMinusSign()
        {
            string result = ReplayGainData.FormatGain(-3.75);
            Assert.Equal("-3.75 dB", result);
        }

        [Fact]
        public void FormatGain_Zero_HasPlusSign()
        {
            string result = ReplayGainData.FormatGain(0.0);
            Assert.Equal("+0.00 dB", result);
        }

        [Fact]
        public void FormatPeak_FourDecimalPlaces()
        {
            string result = ReplayGainData.FormatPeak(0.95);
            Assert.Equal("0.9500", result);
        }

        [Fact]
        public void FormatPeak_One_IsFullScale()
        {
            Assert.Equal("1.0000", ReplayGainData.FormatPeak(1.0));
        }

        // ------------------------------------------------------------------ Id3v2Writer

        [Fact]
        public void Id3v2Writer_TrackOnly_WritesTwoFrames()
        {
            var tag = new TagLib.Id3v2.Tag();
            var data = TrackOnlyData(-5.0, 0.95);

            Id3v2Writer.WriteToTag(tag, data);

            var trackGainFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_GAIN", create: false);
            var trackPeakFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_PEAK", create: false);
            var albumGainFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_ALBUM_GAIN", create: false);
            var albumPeakFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_ALBUM_PEAK", create: false);

            Assert.NotNull(trackGainFrame);
            Assert.NotNull(trackPeakFrame);
            Assert.Null(albumGainFrame);
            Assert.Null(albumPeakFrame);
        }

        [Fact]
        public void Id3v2Writer_TrackOnly_CorrectValues()
        {
            var tag = new TagLib.Id3v2.Tag();
            Id3v2Writer.WriteToTag(tag, TrackOnlyData(-5.0, 0.95));

            var gainFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_GAIN", create: false);
            var peakFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_PEAK", create: false);

            Assert.Equal("-5.00 dB", gainFrame!.Text[0]);
            Assert.Equal("0.9500", peakFrame!.Text[0]);
        }

        [Fact]
        public void Id3v2Writer_AlbumMode_WritesFourFrames()
        {
            var tag = new TagLib.Id3v2.Tag();
            Id3v2Writer.WriteToTag(tag, AlbumData(-5.0, -4.0, 0.95, 0.90));

            var trackGainFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_GAIN", create: false);
            var trackPeakFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_PEAK", create: false);
            var albumGainFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_ALBUM_GAIN", create: false);
            var albumPeakFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_ALBUM_PEAK", create: false);

            Assert.NotNull(trackGainFrame);
            Assert.NotNull(trackPeakFrame);
            Assert.NotNull(albumGainFrame);
            Assert.NotNull(albumPeakFrame);

            Assert.Equal("-5.00 dB", trackGainFrame!.Text[0]);
            Assert.Equal("0.9500", trackPeakFrame!.Text[0]);
            Assert.Equal("-4.00 dB", albumGainFrame!.Text[0]);
            Assert.Equal("0.9000", albumPeakFrame!.Text[0]);
        }

        [Fact]
        public void Id3v2Writer_UpdateExistingFrame_Overwrites()
        {
            var tag = new TagLib.Id3v2.Tag();
            // Write initial value
            Id3v2Writer.WriteToTag(tag, TrackOnlyData(-3.0, 0.80));
            // Overwrite with new value
            Id3v2Writer.WriteToTag(tag, TrackOnlyData(-7.0, 0.70));

            var frame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_GAIN", create: false);
            Assert.Equal("-7.00 dB", frame!.Text[0]);

            var peakFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_PEAK", create: false);
            Assert.Equal("0.7000", peakFrame!.Text[0]);
        }

        [Fact]
        public void Id3v2Writer_PositiveTrackGain_HasPlusSign()
        {
            var tag = new TagLib.Id3v2.Tag();
            Id3v2Writer.WriteToTag(tag, TrackOnlyData(+3.5, 0.95));

            var frame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, "REPLAYGAIN_TRACK_GAIN", create: false);
            Assert.Equal("+3.50 dB", frame!.Text[0]);
        }

        // ------------------------------------------------------------------ VorbisWriter

        [Fact]
        public void VorbisWriter_TrackOnly_WritesTwoFields()
        {
            var tag = new TagLib.Ogg.XiphComment();
            VorbisWriter.WriteToTag(tag, TrackOnlyData(-5.0, 0.95));

            string[] gainValues = tag.GetField("REPLAYGAIN_TRACK_GAIN");
            string[] peakValues = tag.GetField("REPLAYGAIN_TRACK_PEAK");
            string[] albumGainValues = tag.GetField("REPLAYGAIN_ALBUM_GAIN");
            string[] albumPeakValues = tag.GetField("REPLAYGAIN_ALBUM_PEAK");

            Assert.Single(gainValues);
            Assert.Single(peakValues);
            Assert.Empty(albumGainValues);
            Assert.Empty(albumPeakValues);
        }

        [Fact]
        public void VorbisWriter_TrackOnly_CorrectValues()
        {
            var tag = new TagLib.Ogg.XiphComment();
            VorbisWriter.WriteToTag(tag, TrackOnlyData(-5.0, 0.95));

            Assert.Equal("-5.00 dB", tag.GetField("REPLAYGAIN_TRACK_GAIN")[0]);
            Assert.Equal("0.9500", tag.GetField("REPLAYGAIN_TRACK_PEAK")[0]);
        }

        [Fact]
        public void VorbisWriter_AlbumMode_WritesFourFields()
        {
            var tag = new TagLib.Ogg.XiphComment();
            VorbisWriter.WriteToTag(tag, AlbumData(-5.0, -4.0, 0.95, 0.90));

            Assert.Equal("-5.00 dB", tag.GetField("REPLAYGAIN_TRACK_GAIN")[0]);
            Assert.Equal("0.9500", tag.GetField("REPLAYGAIN_TRACK_PEAK")[0]);
            Assert.Equal("-4.00 dB", tag.GetField("REPLAYGAIN_ALBUM_GAIN")[0]);
            Assert.Equal("0.9000", tag.GetField("REPLAYGAIN_ALBUM_PEAK")[0]);
        }

        [Fact]
        public void VorbisWriter_UpdateExistingField_Overwrites()
        {
            var tag = new TagLib.Ogg.XiphComment();
            VorbisWriter.WriteToTag(tag, TrackOnlyData(-3.0, 0.80));
            VorbisWriter.WriteToTag(tag, TrackOnlyData(-7.0, 0.70));

            Assert.Equal("-7.00 dB", tag.GetField("REPLAYGAIN_TRACK_GAIN")[0]);
            Assert.Equal("0.7000", tag.GetField("REPLAYGAIN_TRACK_PEAK")[0]);
        }

        [Fact]
        public void VorbisWriter_PositiveGain_HasPlusSign()
        {
            var tag = new TagLib.Ogg.XiphComment();
            VorbisWriter.WriteToTag(tag, TrackOnlyData(+2.0, 0.85));

            Assert.Equal("+2.00 dB", tag.GetField("REPLAYGAIN_TRACK_GAIN")[0]);
        }

        // ------------------------------------------------------------------ ItunesWriter

        [Fact]
        public void ItunesWriter_TrackOnly_WritesTwoAtoms()
        {
            var udtaBox = new TagLib.Mpeg4.IsoUserDataBox();
            var tag = new TagLib.Mpeg4.AppleTag(udtaBox);
            ItunesWriter.WriteToTag(tag, TrackOnlyData(-5.0, 0.95));

            string trackGain = tag.GetDashBox("com.apple.iTunes", "replaygain_track_gain");
            string trackPeak = tag.GetDashBox("com.apple.iTunes", "replaygain_track_peak");
            string albumGain = tag.GetDashBox("com.apple.iTunes", "replaygain_album_gain");
            string albumPeak = tag.GetDashBox("com.apple.iTunes", "replaygain_album_peak");

            Assert.Equal("-5.00 dB", trackGain);
            Assert.Equal("0.9500", trackPeak);
            Assert.Null(albumGain);
            Assert.Null(albumPeak);
        }

        [Fact]
        public void ItunesWriter_AlbumMode_WritesFourAtoms()
        {
            var udtaBox = new TagLib.Mpeg4.IsoUserDataBox();
            var tag = new TagLib.Mpeg4.AppleTag(udtaBox);
            ItunesWriter.WriteToTag(tag, AlbumData(-5.0, -4.0, 0.95, 0.90));

            Assert.Equal("-5.00 dB", tag.GetDashBox("com.apple.iTunes", "replaygain_track_gain"));
            Assert.Equal("0.9500",   tag.GetDashBox("com.apple.iTunes", "replaygain_track_peak"));
            Assert.Equal("-4.00 dB", tag.GetDashBox("com.apple.iTunes", "replaygain_album_gain"));
            Assert.Equal("0.9000",   tag.GetDashBox("com.apple.iTunes", "replaygain_album_peak"));
        }

        [Fact]
        public void ItunesWriter_UpdateExistingAtom_Overwrites()
        {
            var udtaBox = new TagLib.Mpeg4.IsoUserDataBox();
            var tag = new TagLib.Mpeg4.AppleTag(udtaBox);
            ItunesWriter.WriteToTag(tag, TrackOnlyData(-3.0, 0.80));
            ItunesWriter.WriteToTag(tag, TrackOnlyData(-7.0, 0.70));

            Assert.Equal("-7.00 dB", tag.GetDashBox("com.apple.iTunes", "replaygain_track_gain"));
            Assert.Equal("0.7000",   tag.GetDashBox("com.apple.iTunes", "replaygain_track_peak"));
        }

        // ------------------------------------------------------------------ GainApplier + ReplayGainData construction

        [Fact]
        public void ReplayGainData_TrackOnly_AlbumFieldsNull()
        {
            var data = new ReplayGainData { TrackGain = -5.0, TrackPeak = 1.0 };
            Assert.Null(data.AlbumGain);
            Assert.Null(data.AlbumPeak);
        }

        [Fact]
        public void ReplayGainData_WithAlbum_AllFieldsSet()
        {
            var data = new ReplayGainData
            {
                TrackGain = -5.0,
                TrackPeak = 1.0,
                AlbumGain = -4.0,
                AlbumPeak = 1.0
            };
            Assert.NotNull(data.AlbumGain);
            Assert.NotNull(data.AlbumPeak);
            Assert.Equal(-4.0, data.AlbumGain!.Value);
            Assert.Equal(1.0, data.AlbumPeak!.Value);
        }
    }
}
