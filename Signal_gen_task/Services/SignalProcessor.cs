using Signal_gen_task.Models;

namespace Signal_gen_task.Services
{
    public class SignalProcessor
    {
        public double FindMaxValue(List<SignalPoint> points)
        {
            ValidatePoints(points);

            return points.Max(point => point.Value);
        }

        public double FindMinValue(List<SignalPoint> points)
        {
            ValidatePoints(points);

            return points.Min(point => point.Value);
        }

        public double FindAverageValue(List<SignalPoint> points)
        {
            ValidatePoints(points);

            return points.Average(point => point.Value);
        }

        public int CalculateZeroCrossings(List<SignalPoint> points)
        {
            ValidatePoints(points);

            int crossings = 0;
            int previousSign = Math.Sign(points[0].Value);

            for (int i = 1; i < points.Count; i++)
            {
                int currentSign = Math.Sign(points[i].Value);

                if (currentSign == 0) continue;

                if (previousSign != 0 && currentSign != previousSign) crossings++;

                previousSign = currentSign;
            }

            return crossings;
        }

        private void ValidatePoints(List<SignalPoint> points)
        {
            if (points.Count == 0)
            {
                throw new ArgumentException("Список точек сигнала не должен быть пустым.", nameof(points));
            }
        }
    }
}
