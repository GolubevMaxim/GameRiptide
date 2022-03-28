using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public ushort networkID;
    public string nickname;
    public Vector2 position;
    private GameObject _obj;

    public Player(ushort networkID, string nickname, float positionX, float positionY)
    {
        this.networkID = networkID;
        this.nickname = nickname;
        position = new Vector2(positionX, positionY);

    }

    public void Instantiate()
    {
        _obj = GameObject.Instantiate(GameManager.Singleton.playerPrefab);
        Debug.Log("Player instantiated!");
    }
}
