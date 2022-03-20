using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class DataBaseManager : MonoBehaviour
{
    private static DataBaseManager _singleton;

    public static DataBaseManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(DataBaseManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    private const string filename = "playerdata.db";
    private static string DBPath = @"/DataBases/playerdata.db";
    private static SqliteConnection connection;
    private static SqliteCommand command;

    private void Awake()
    {
        Singleton = this;
    }

    public static void OpenConnection()
    {
        connection = new SqliteConnection("URI=file:" + Application.dataPath + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    public static User SelectPlayerLogPas(string login, string password)
    {
        User user = null;
        OpenConnection();
        using (IDbCommand dbcmd = connection.CreateCommand())
        {
            dbcmd.CommandText = $"SELECT id, characters from user where login = \"{login}\" and password = \"{password}\"";
            using (IDataReader reader = dbcmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User(reader.GetInt32(0), login, 0);
                }
            }
        }
        CloseConnection();
        return user;
    }
}
