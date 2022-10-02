using Microsoft.Data.Sqlite;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    internal class Simple02
    {
        static void Main(string[] args)
        {
            LoggSaver Oracle = new LoggSaver(new OracleClientFactory());
            Oracle.Save(new  LoggEntry[]{ new  LoggEntry(), new LoggEntry()});

            LoggSaver SqlLite = new LoggSaver(SqliteFactory.Instance);
            SqlLite.Save(new LoggEntry[] { new LoggEntry(), new LoggEntry() });
        }
        //DbProviderFactory //при помощи этого класса можно создавать провайдер для взаимодействия с разными базами данных(абстрактная фабрика)
        //  SqliteFactory //все этти классы унаследованны от DbProviderFactory
        //OracleClientFactory 

    }

    public class LoggEntry
    {
        public string? Text { get; set; }
    }

    public class LoggSaver
    {
        private readonly DbProviderFactory _factories;
        public LoggSaver(DbProviderFactory factory)
        {
            _factories = factory;
        }

        public void Save(IEnumerable<LoggEntry> loggEntries)
        {
            using (var dbConection = _factories.CreateConnection())
            {
                SetConnectionString(dbConection);
                using(var dbCommand= _factories.CreateCommand())
                {
                    SetCommadnArguments(loggEntries);
                    dbCommand.ExecuteNonQuery();
                }
            }
        }
        private void SetConnectionString(DbConnection dbConnection) { }
        private void SetCommadnArguments(IEnumerable<LoggEntry> loggs) { }

    }
        
}
