using Signal_gen_task.Models;
using Signal_gen_task.Services;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Signal_gen_task.Repositories;


namespace Signal_gen_task
{
    public partial class MainWindow : Window
    {
        private readonly SignalProcessor _processor = new();
        private readonly ISignalRepository _repository = new SignalRepository();

        private SignalParameters? _currentParameters;
        private List<SignalPoint>? _currentPoints;
        public MainWindow() => InitializeComponent();

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorTextBlock.Text = string.Empty;

                var selectedItem = (ComboBoxItem)SignalTypeComboBox.SelectedItem;

                SignalType signalType = Enum.Parse<SignalType>(selectedItem.Content!.ToString()!);

                double amplitude = ParseDouble(AmplitudeTextBox.Text, "амплитуда");
                double frequency = ParseDouble(FrequencyTextBox.Text, "частота");
                int pointCount = ParseInt(PointCountTextBox.Text, "количество точек");

                var parameters = new SignalParameters(signalType, amplitude, frequency, pointCount);
                var points = SignalGenerator.Generate(parameters);

                double max = _processor.FindMaxValue(points);
                double min = _processor.FindMinValue(points);
                double average = _processor.FindAverageValue(points);
                int zeroCrossings = _processor.CalculateZeroCrossings(points);

                ResultTextBlock.Text =
                   $"Максимум: {max:F3}; Минимум: {min:F3}; Среднее: {average:F3}; Пересечения нуля: {zeroCrossings}";

                SignalDataGrid.ItemsSource = points;

                DrawSignal(points);

                _currentParameters = parameters;
                _currentPoints = points;
                SaveButton.IsEnabled = true;
            }

            catch (Exception ex)
            {
                ErrorTextBlock.Text = ex.Message;
                ResultTextBlock.Text = string.Empty;
                SignalDataGrid.ItemsSource = null;

                SignalCanvas.Children.Clear();

                _currentParameters = null;
                _currentPoints = null;
                SaveButton.IsEnabled = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentParameters is null || _currentPoints is null)
            {
                ErrorTextBlock.Text = "Сначала сгенерируйте сигнал.";
                return;
            }

            try
            {
                _repository.SaveSignal(
                    new SignalRecord
                    {
                        SignalType = _currentParameters.Type.ToString(),
                        Amplitude = _currentParameters.Amplitude,
                        Frequency = _currentParameters.Frequency,
                        PointCount = _currentParameters.PointCount,
                        CreatedAt = DateTime.UtcNow
                    },
                    _currentPoints);

                ErrorTextBlock.Text = string.Empty;
                ResultTextBlock.Text += "; Сохранено в SQLite";
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Ошибка сохранения: {ex.Message}";
            }
        }

        private int ParseInt(string text, string fieldName)
        {
            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out int value))
            {
                throw new ArgumentException($"Некорректное значение поля: {fieldName}.");
            }

            return value;
        }

        private double ParseDouble(string text, string fieldName)
        {
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out double value))
            {
                throw new ArgumentException($"Некорректное значение поля: {fieldName}.");
            }

            return value;
        }

        private void SignalCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_currentPoints is not null)
            {
                DrawSignal(_currentPoints);
            }
        }

        private void DrawSignal(List<SignalPoint> points)
        {
            SignalCanvas.Children.Clear();

            if (points.Count < 2)
            {
                return;
            }

            double width = SignalCanvas.ActualWidth;
            double height = SignalCanvas.ActualHeight;

            if (width <= 0 || height <= 0)
            {
                return;
            }

            double maxAbsValue = points.Max(point => Math.Abs(point.Value));

            if (maxAbsValue == 0)
            {
                maxAbsValue = 1;
            }

            double centerY = height / 2;

            var zeroLine = new Line
            {
                X1 = 0,
                Y1 = centerY,
                X2 = width,
                Y2 = centerY,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1
            };

            SignalCanvas.Children.Add(zeroLine);

            var polyline = new Polyline
            {
                Stroke = Brushes.DodgerBlue,
                StrokeThickness = 2
            };

            for (int i = 0; i < points.Count; i++)
            {
                double x = (double)i / (points.Count - 1) * width;
                double y = centerY - points[i].Value / maxAbsValue * (height / 2 - 10);

                polyline.Points.Add(new Point(x, y));
            }

            SignalCanvas.Children.Add(polyline);
        }
    }
}