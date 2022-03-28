using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _singleton;

    public static GameManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    public Dictionary<ushort, Player> Players;
    public GameObject playerPrefab;
    private Queue<Player> _playersToSpawn;
    public bool loadingRoom = false;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Singleton.Players = new Dictionary<ushort, Player>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (loadingRoom)
        {
            Debug.Log("Scene loaded!");
            NetworkManager.Singleton.SendEnterGameRequest();
            loadingRoom = false;
        }
    }

    public static void SetRoom(string name)
    {
        Singleton.loadingRoom = true;
        SceneManager.LoadScene(name);
    }

    public void AddPlayer(Player player)
    {
        Players.Add(player.networkID, player);
        player.Instantiate();
    }
}
