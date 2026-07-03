namespace Signal_gen_task.Models
{
    public class SignalParameters
    {
        public const int MinPointCount = 100;
        public const int MaxPointCount = 10000;

        public SignalParameters(SignalType type, double amplitude, double frequency, int pointCount)
        {
            if (amplitude <= 0) throw new ArgumentOutOfRangeException(nameof(amplitude), "Значение амплитуды должно быть больше нуля.");
            if (frequency <= 0) throw new ArgumentOutOfRangeException(nameof(frequency), "Значение частоты должно быть больше нуля.");
            if (pointCount < MinPointCount || pointCount > MaxPointCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pointCount), $"Количество точек должно быть от {MinPointCount} до {MaxPointCount}.");
            }

            Type = type;
            Amplitude = amplitude;
            Frequency = frequency;
            PointCount = pointCount;
        }

        public SignalType Type { get; }
        public double Amplitude { get; }
        public double Frequency { get; }
        public int PointCount { get; }
    }
}
