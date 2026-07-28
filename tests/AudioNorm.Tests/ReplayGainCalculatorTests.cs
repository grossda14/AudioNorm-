using System.Collections.Generic;
using AudioNormPlus.Models;
using AudioNormPlus.Services;
using Xunit;

namespace AudioNormPlus.Tests
{
    public class ReplayGainCalculatorTests
    {
        [Fact]
        public void CalculateTrackGain_Returns_TargetMinusMeasured()
        {
            var calc = new ReplayGainCalculator();
            var f = new AudioFile("test.mp3") { LoudnessIntegrated = -16.0 };
            double gain = calc.CalculateTrackGain(f);
            Assert.Equal(2.0, gain, 3);
        }

        [Fact]
        public void CalculateAlbumGain_Computes_MeanBasedGain()
        {
            var calc = new ReplayGainCalculator();
            var files = new List<AudioFile>
            {
                new AudioFile("a.mp3") { LoudnessIntegrated = -12.0 },
                new AudioFile("b.mp3") { LoudnessIntegrated = -16.0 },
            };

            double albumGain = calc.CalculateAlbumGain(files);
            // mean = -14.0, target -14 => 0.0
            Assert.Equal(0.0, albumGain, 3);
        }

        [Fact]
        public void NormalizeGainIncrement_Rounds_To_HalfDb_And_Clamp()
        {
            var calc = new ReplayGainCalculator();
            Assert.Equal(1.5, calc.NormalizeGainIncrement(1.49));
            Assert.Equal(24.0, calc.NormalizeGainIncrement(1000));
            Assert.Equal(-24.0, calc.NormalizeGainIncrement(-1000));
        }
    }
}
