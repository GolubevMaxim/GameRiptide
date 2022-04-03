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

    private const string Filename = "playerdata.db";
    private static readonly string DBPath = @"/DataBases/playerdata.db";
    
    private static SqliteConnection _connection;
    private static SqliteCommand _command;

    private void Awake()
    {
        Singleton = this;
    }

    public static void OpenConnection()
    {
        _connection = new SqliteConnection("URI=file:" + Application.dataPath + DBPath);
        _command = new SqliteCommand(_connection);
        _connection.Open();
    }

    public static void CloseConnection()
    {
        _connection.Close();
        _command.Dispose();
    }

    public static User SelectPlayerLogPas(string login, string password)
    {
        User user = null;
        
        OpenConnection();
        
        using (IDbCommand dataBaseCommand = _connection.CreateCommand())
        {
            dataBaseCommand.CommandText = $"SELECT id, characters from user where login = \"{login}\" and password = \"{password}\"";
            using (var reader = dataBaseCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User(reader.GetInt32(0), login);
                }
            }
        }
        
        CloseConnection();
        return user;
    }
}
