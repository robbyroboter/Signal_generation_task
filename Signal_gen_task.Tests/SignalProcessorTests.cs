using Signal_gen_task.Models;
using Signal_gen_task.Services;

namespace Signal_gen_task.Tests
{
    public class SignalProcessorTests
    {
        private readonly SignalProcessor _processor = new();

        [Fact]
        public void FindMaxValue_ReturnsMaximumSignalValue()
        {
            var points = new List<SignalPoint>
            {
                new(0, -2),
                new(1, 5),
                new(2, 3)
            };

            double result = _processor.FindMaxValue(points);

            Assert.Equal(5, result);
        }

        [Fact]
        public void FindMinValue_ReturnsMinimumSignalValue()
        {
            var points = new List<SignalPoint>
            {
                new(0, -2),
                new(1, 5),
                new(2, 3)
            };

            double result = _processor.FindMinValue(points);

            Assert.Equal(-2, result);
        }

        [Fact]
        public void FindAverageValue_ReturnsAverageSignalValue()
        {
            var points = new List<SignalPoint>
            {
                new(0, 1),
                new(1, 2),
                new(2, 3)
            };

            double result = _processor.FindAverageValue(points);

            Assert.Equal(2, result);
        }

        [Fact]
        public void CalculateZeroCrossings_ReturnsNumberOfSignChanges()
        {
            var points = new List<SignalPoint>
            {
                new(0, 1),
                new(1, -1),
                new(2, 2),
                new(3, -2)
            };

            int result = _processor.CalculateZeroCrossings(points);

            Assert.Equal(3, result);
        }
    }
}