namespace Signal_gen_task.Models;

public class SignalRecord
{
    public int Id { get; set; }

    public string SignalType { get; set; } = string.Empty;

    public double Amplitude { get; set; }

    public double Frequency { get; set; }

    public int PointCount { get; set; }

    public DateTime CreatedAt { get; set; }
}