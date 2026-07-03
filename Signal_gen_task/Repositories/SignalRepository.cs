using Microsoft.Data.Sqlite;
using Signal_gen_task.Models;

namespace Signal_gen_task.Repositories
{
    public class SignalRepository : ISignalRepository
    {
        private readonly string _connectionString;

        public SignalRepository(string connectionString = "Data Source=signals.db")
        {
            _connectionString = connectionString;
            Initialize();
        }

        private SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        private void Initialize()
        {
            using var connection = CreateConnection();

            var cmd = connection.CreateCommand();

            cmd.CommandText =
            @"
        CREATE TABLE IF NOT EXISTS Signals (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            SignalType TEXT NOT NULL,
            Amplitude REAL NOT NULL,
            Frequency REAL NOT NULL,
            PointCount INTEGER NOT NULL,
            CreatedAt TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS SignalPoints (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            SignalRecordId INTEGER NOT NULL,
            Time REAL NOT NULL,
            Value REAL NOT NULL
        );
        ";

            cmd.ExecuteNonQuery();
        }

        public void SaveSignal(SignalRecord record, List<SignalPoint> points)
        {
            using var connection = CreateConnection();
            using var transaction = connection.BeginTransaction();

            var insertSignal = connection.CreateCommand();

            insertSignal.CommandText =
            @"
        INSERT INTO Signals (SignalType, Amplitude, Frequency, PointCount, CreatedAt)
        VALUES ($type, $amp, $freq, $count, $createdAt);
        SELECT last_insert_rowid();
        ";

            insertSignal.Parameters.AddWithValue("$type", record.SignalType);
            insertSignal.Parameters.AddWithValue("$amp", record.Amplitude);
            insertSignal.Parameters.AddWithValue("$freq", record.Frequency);
            insertSignal.Parameters.AddWithValue("$count", record.PointCount);
            insertSignal.Parameters.AddWithValue("$createdAt", DateTime.UtcNow);

            long signalId = (long)insertSignal.ExecuteScalar()!;

            foreach (var p in points)
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText =
                @"
            INSERT INTO SignalPoints (SignalRecordId, Time, Value)
            VALUES ($id, $time, $value);
            ";

                cmd.Parameters.AddWithValue("$id", signalId);
                cmd.Parameters.AddWithValue("$time", p.Time);
                cmd.Parameters.AddWithValue("$value", p.Value);

                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public List<SignalRecord> GetAllSignals()
        {
            var result = new List<SignalRecord>();

            using var connection = CreateConnection();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Signals";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new SignalRecord
                {
                    Id = reader.GetInt32(0),
                    SignalType = reader.GetString(1),
                    Amplitude = reader.GetDouble(2),
                    Frequency = reader.GetDouble(3),
                    PointCount = reader.GetInt32(4),
                    CreatedAt = DateTime.Parse(reader.GetString(5))
                });
            }

            return result;
        }

        public List<SignalPoint> GetPoints(int signalId)
        {
            var result = new List<SignalPoint>();

            using var connection = CreateConnection();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
                "SELECT Time, Value FROM SignalPoints WHERE SignalRecordId = $id";

            cmd.Parameters.AddWithValue("$id", signalId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new SignalPoint(
                    reader.GetDouble(0),
                    reader.GetDouble(1)));
            }

            return result;
        }
    }
}