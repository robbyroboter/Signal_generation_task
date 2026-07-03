using Signal_gen_task.Models;

namespace Signal_gen_task.Tests
{
    public class SignalParametersTests
    {
        [Fact]
        public void Constructor_AmplitudeIsZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SignalParameters(
                    SignalType.Sine,
                    amplitude: 0,
                    frequency: 1,
                    pointCount: 100));
        }

        [Fact]
        public void Constructor_FrequencyIsZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SignalParameters(
                    SignalType.Sine,
                    amplitude: 5,
                    frequency: 0,
                    pointCount: 100));
        }

        [Fact]
        public void Constructor_PointCountLessThanMinimum_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SignalParameters(
                    SignalType.Sine,
                    amplitude: 5,
                    frequency: 1,
                    pointCount: 99));
        }

        [Fact]
        public void Constructor_PointCountGreaterThanMaximum_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SignalParameters(
                    SignalType.Sine,
                    amplitude: 5,
                    frequency: 1,
                    pointCount: 10001));
        }
    }
}