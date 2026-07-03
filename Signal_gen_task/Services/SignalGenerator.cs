using Signal_gen_task.Models;

namespace Signal_gen_task.Services
{
    public static class SignalGenerator
    {
        public static List<SignalPoint> Generate(SignalParameters parameters)
        {
            return parameters.Type switch
            {
                SignalType.Sine => GenerateSine(parameters),
                SignalType.Square => GenerateSquare(parameters),
                SignalType.Triangle => GenerateTriangle(parameters),
                SignalType.Sawtooth => GenerateSawtooth(parameters),
                _ => throw new ArgumentException("Неизвестный тип сигнала.", nameof(parameters))
            };
        }

        private static List<SignalPoint> GenerateSine(SignalParameters parameters)
        {
            var points = new List<SignalPoint>();

            for (int i = 0; i < parameters.PointCount; i++)
            {
                double time = (double)i / parameters.PointCount;
                double value = parameters.Amplitude * Math.Sin(2 * Math.PI * parameters.Frequency * time);

                points.Add(new SignalPoint(time, value));
            }

            return points;
        }

        private static List<SignalPoint> GenerateSquare(SignalParameters parameters)
        {
            var points = new List<SignalPoint>();

            for (int i = 0; i < parameters.PointCount; i++)
            {
                double time = (double)i / parameters.PointCount;
                double sinValue = Math.Sin(2 * Math.PI * parameters.Frequency * time);
                double value = sinValue >= 0 ? parameters.Amplitude : -parameters.Amplitude;

                points.Add(new SignalPoint(time, value));
            }

            return points;
        }

        private static List<SignalPoint> GenerateTriangle(SignalParameters parameters)
        {
            var points = new List<SignalPoint>();

            for (int i = 0; i < parameters.PointCount; i++)
            {
                double time = (double)i / parameters.PointCount;
                double phase = (time * parameters.Frequency) % 1.0;

                double value;

                if (phase < 0.5)
                {
                    value = -parameters.Amplitude + 4 * parameters.Amplitude * phase;
                }
                else
                {
                    value = 3 * parameters.Amplitude - 4 * parameters.Amplitude * phase;
                }

                points.Add(new SignalPoint(time, value));
            }

            return points;
        }

        private static List<SignalPoint> GenerateSawtooth(SignalParameters parameters)
        {
            var points = new List<SignalPoint>();

            for (int i = 0; i < parameters.PointCount; i++)
            {
                double time = (double)i / parameters.PointCount;
                double phase = (time * parameters.Frequency) % 1.0;

                double value = -parameters.Amplitude + 2 * parameters.Amplitude * phase;

                points.Add(new SignalPoint(time, value));
            }

            return points;
        }
    }

}

