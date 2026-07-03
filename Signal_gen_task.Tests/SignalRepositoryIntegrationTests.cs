using Signal_gen_task.Models;
using Signal_gen_task.Repositories;
using Xunit;

namespace Signal_gen_task.Tests
{
    public class SignalRepositoryIntegrationTests
    {
        [Fact]
        public void SaveSignal_ShouldPersistDataCorrectly()
        {
            var dbName = $"test_{Guid.NewGuid()}.db";
            var connectionString = $"Data Source={dbName}";

            var repo = new SignalRepository(connectionString);

            var record = new SignalRecord
            {
                SignalType = "Sine",
                Amplitude = 5,
                Frequency = 2,
                PointCount = 3,
                CreatedAt = DateTime.UtcNow
            };

            var points = new List<SignalPoint>
                {
                    new(0, 0),
                    new(0.5, 5),
                    new(1, 0)
                };

            repo.SaveSignal(record, points);

            var signals = repo.GetAllSignals();
            var saved = signals[0];

            var loadedPoints = repo.GetPoints(saved.Id);

            Assert.Single(signals);
            Assert.Equal(3, loadedPoints.Count);
            Assert.Equal("Sine", saved.SignalType);
        }
    }
}