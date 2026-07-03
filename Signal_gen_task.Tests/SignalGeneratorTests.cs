using Signal_gen_task.Models;
using Signal_gen_task.Services;

namespace Signal_gen_task.Tests
{
    public class SignalGeneratorTests
    {
        [Fact]
        public void Generate_SineSignal_ReturnsRequestedPointCount()
        {
            var parameters = new SignalParameters(
                SignalType.Sine,
                amplitude: 5,
                frequency: 2,
                pointCount: 100);

            var points = SignalGenerator.Generate(parameters);

            Assert.Equal(100, points.Count);
        }

        [Fact]
        public void Generate_SineSignal_StartsFromZero()
        {
            var parameters = new SignalParameters(
                SignalType.Sine,
                amplitude: 5,
                frequency: 1,
                pointCount: 100);

            var points = SignalGenerator.Generate(parameters);

            Assert.Equal(0, points[0].Time);
            Assert.Equal(0, points[0].Value, precision: 10);
        }

        [Fact]
        public void Generate_SquareSignal_ContainsOnlyAmplitudeAndNegativeAmplitude()
        {
            var parameters = new SignalParameters(
                SignalType.Square,
                amplitude: 5,
                frequency: 1,
                pointCount: 100);

            var points = SignalGenerator.Generate(parameters);

            Assert.All(points, point =>
            {
                Assert.True(point.Value == 5 || point.Value == -5);
            });
        }

        [Fact]
        public void Generate_TriangleSignal_ReturnsRequestedPointCount()
        {
            var parameters = new SignalParameters(
                SignalType.Triangle,
                amplitude: 5,
                frequency: 2,
                pointCount: 100);

            var points = SignalGenerator.Generate(parameters);

            Assert.Equal(100, points.Count);
        }

        [Fact]
        public void Generate_TriangleSignal_ValuesStayWithinAmplitude()
        {
            var parameters = new SignalParameters(
                SignalType.Triangle,
                amplitude: 5,
                frequency: 2,
                pointCount: 1000);

            var points = SignalGenerator.Generate(parameters);

            Assert.All(points, point =>
            {
                Assert.InRange(point.Value, -5.0, 5.0);
            });
        }

        [Fact]
        public void Generate_TriangleSignal_ReachesMaximumAndMinimumAmplitude()
        {
            var parameters = new SignalParameters(
                SignalType.Triangle,
                amplitude: 5,
                frequency: 1,
                pointCount: 1000);

            var points = SignalGenerator.Generate(parameters);

            Assert.Equal(5, points.Max(point => point.Value), 3);
            Assert.Equal(-5, points.Min(point => point.Value), 3);
        }

        [Fact]
        public void Generate_SawtoothSignal_ReturnsRequestedPointCount()
        {
            var parameters = new SignalParameters(
                SignalType.Sawtooth,
                amplitude: 5,
                frequency: 2,
                pointCount: 100);

            var points = SignalGenerator.Generate(parameters);

            Assert.Equal(100, points.Count);
        }

        [Fact]
        public void Generate_SawtoothSignal_ValuesStayWithinAmplitude()
        {
            var parameters = new SignalParameters(
                SignalType.Sawtooth,
                amplitude: 5,
                frequency: 2,
                pointCount: 1000);

            var points = SignalGenerator.Generate(parameters);

            Assert.All(points, point =>
            {
                Assert.InRange(point.Value, -5.0, 5.0);
            });
        }

        [Fact]
        public void Generate_SawtoothSignal_ReachesMaximumAndMinimumAmplitude()
        {
            var parameters = new SignalParameters(
                SignalType.Sawtooth,
                amplitude: 5,
                frequency: 1,
                pointCount: 1000);

            var points = SignalGenerator.Generate(parameters);

            double max = points.Max(p => p.Value);
            double min = points.Min(p => p.Value);

            Assert.InRange(max, 4.9, 5.0);
            Assert.InRange(min, -5.0, -4.9);
        }
    }
}
