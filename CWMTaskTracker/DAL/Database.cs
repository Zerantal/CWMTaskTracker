using System;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;

namespace CWMTaskTracker.DAL
{
    public class Database
    {
        private readonly string _dbFile;

        public Database(string dbFile)
        {
            _dbFile = dbFile;
        }

        public SQLiteConnection OpenDbConnection()
        {
            string dbConnStr = "Data Source=" + _dbFile + ";foreign keys=true;Version = 3";
            SQLiteConnection dbConn = new SQLiteConnection(dbConnStr);
            dbConn.Open();
            return dbConn;
        }

        internal void InitDatabase()
        {
            SQLiteConnection.CreateFile(_dbFile);

            string[] sqlCmds =
            {
                @"CREATE TABLE Job (
                    JobID INTEGER PRIMARY KEY,
                    JobTitle TEXT NOT NULL,
                    ProjectID INTEGER NOT NULL,
                    Description TEXT,
                    ComplexityID INTEGER NOT NULL,
                    PriorityID INTEGER NOT NULL,
                    Deadline TEXT,
                    StatusID INTEGER NOT NULL,
                    FOREIGN KEY(ProjectID) REFERENCES Project(ProjectID),
                    FOREIGN KEY(ComplexityID) REFERENCES Complexity(ComplexityID),
                    FOREIGN KEY(PriorityID) REFERENCES Priority(PriorityID),
                    FOREIGN KEY(StatusID) REFERENCES Status(StatusID))",

                @"CREATE TABLE Project (
                    ProjectID INTEGER PRIMARY KEY,
                    ProjectName TEXT NOT NULL,
                    Description TEXT)",

                @"CREATE TABLE Complexity (
                    ComplexityID INTEGER PRIMARY KEY,
                    ComplexityName TEXT NOT NULL)",

                @"CREATE TABLE Priority (
                    PriorityID INTEGER PRIMARY KEY,
                    PriorityName TEXT NOT NULL)",

                @"CREATE TABLE Status (
                    StatusID INTEGER PRIMARY KEY,
                    StatusName TEXT NOT NULL)"
            };

            using (SQLiteConnection dbConn = OpenDbConnection())
            {
                SQLiteTransaction tr = dbConn.BeginTransaction();

                using (SQLiteCommand cmd = dbConn.CreateCommand())
                {
                    foreach (string sql in sqlCmds)
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }

                }

                // add allowable complexity values
                addLookupValues(tr, "Complexity", new[] { "Low", "Medium", "High", "Very High" });

                // add allowable priority values
                addLookupValues(tr, "Priority", new[] { "Low", "Medium", "High", "Urgent" });

                // add allowable status values
                addLookupValues(tr, "Status", new[] { "Pending", "Partial", "Done", "Cancelled", "Deferred" });                

                tr.Commit();
            }
        }

        private void addLookupValues(SQLiteTransaction tr, string tableName, string[] values)
        {
            using (SQLiteCommand cmd = tr.Connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO " + tableName + " VALUES " +
                                  String.Join(", ", values.Select((val, index) => "(NULL, @v" + index + ")"));

                cmd.Parameters.AddRange(values.Select((val, index) => new SQLiteParameter("@v" + index, val)).ToArray());
                cmd.ExecuteNonQuery();
            }
        }

    }
}
