using Signal_gen_task.Models;

namespace Signal_gen_task.Repositories;

public interface ISignalRepository
{
    void SaveSignal(SignalRecord record, List<SignalPoint> points);

    List<SignalRecord> GetAllSignals();

    List<SignalPoint> GetPoints(int signalId);
}