using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Room
{
    private readonly Dictionary<ushort, Player> _players;
    private readonly Chat _roomChat;
    private Message _playersMessage;

    public Room()
    {
        _players = new Dictionary<ushort, Player>();
        _roomChat = new Chat(_players);
        _playersMessage = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
    }

    public void AddMessageToChat(ushort playerID, string chatMessage)
    {
        _roomChat.AddMessageToBuffer(playerID, chatMessage);
    }

    public void SendChat()
    {
        _roomChat.SendMessages();
    }

    public void AddPlayer(ushort networkID, Player player)
    {
        //_roomChat.AddUser(id);
        _players.Add(networkID, player);
        PutDataToMessage(_playersMessage, player);
        SendNewPlayer(player);
        
        Debug.Log($"Room accepted the player {networkID}");

        player.EnterRoom(this);
    }

    public bool RemovePlayer(ushort networkID)
    {
        if (_players.Remove(networkID))
        {
            //_roomChat.RemoveUser(id);
            SendRemovePlayer(networkID);
            BuildAllPlayersMessage();
            Debug.Log($"Room kicked out the player {networkID}.");
            return true;
        }
        
        Debug.LogWarning($"Failed to remove player {networkID}.");
        return false;
    }

    public void PutDataToMessage(Message message, Player player)
    {
        message.AddUShort(player.networkID);
        message.AddString(player.user.nickname);
        message.AddFloat(player.position.x);
        message.AddFloat(player.position.y);
    }

    public void BuildAllPlayersMessage()
    {
        _playersMessage = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
        foreach(var player in _players.Values)
        {
            PutDataToMessage(_playersMessage, player);
        }
    }

    public void SendNewPlayer(Player player)
    {
        var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
        PutDataToMessage(message, player);
        foreach (var playerNetworkID in _players.Keys)
            if(playerNetworkID != player.networkID) NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
        NetworkManager.Singleton.Server.Send(_playersMessage, player.networkID, false);
    }

    public void SendRemovePlayer(ushort removedPlayerNetworkID)
    {
        var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RemovePlayerFromRoom);
        message.AddUShort(removedPlayerNetworkID);
        foreach (var playerNetworkID in _players.Keys)
            if (playerNetworkID != removedPlayerNetworkID) NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
    }
}
