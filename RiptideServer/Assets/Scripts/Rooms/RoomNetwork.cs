using System.Linq;
using Enemies;
using Player;
using RiptideNetworking;
using UnityEngine;

namespace Rooms
{
    public class RoomNetwork
    {
        private Room _room;

        public RoomNetwork(Room room)
        {
            _room = room;
        }

        
        
        public void SendNewPlayer(Player.Player player)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
            
            message.AddPlayer(player);
            
            foreach (var playerNetworkID in _room.Players.Keys.Where(playerNetworkID => playerNetworkID != player.NetworkId))
                NetworkManager.Singleton.Server.Send(message, playerNetworkID);
            
            SendAllPlayers(player.NetworkId);
        }
        
        public void SendAllPlayers(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
            
            foreach (var otherPlayer in _room.Players.Values)
                message.AddPlayer(otherPlayer);

            NetworkManager.Singleton.Server.Send(message, playerId);
        }

        public void SendRemovePlayer(ushort removedPlayerNetworkID)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RemovePlayerFromRoom);
            message.AddUShort(removedPlayerNetworkID);
            
            foreach (var playerNetworkID in _room.Players.Keys.Where(playerNetworkID => playerNetworkID != removedPlayerNetworkID))
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
        }

        public void SendPlayerPositions()
        {
            var message = Message.Create(MessageSendMode.unreliable, ServerToClientId.PlayerPositionChange);
            
            foreach (var player in _room.Players.Values)
            {
                message.AddUShort(player.NetworkId);
                message.AddVector2(player.transform.localPosition);
            }
            
            foreach(var playerNetworkID in _room.Players.Keys)
            {
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
            }
        }

        public void LoadRoomRequest(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.LoadRoom);
            
            message.AddUShort(_room.RoomId);
            
            NetworkManager.Singleton.Server.Send(message, playerId);
        }

        [MessageHandler((ushort) ClientToServerId.AllPlayersPosition)]
        public static void GetAllPlayerPositionRequest(ushort fromClientId, Message message)
        {
            if (Players.Dictionary.TryGetValue(fromClientId, out var player))
            {
                player.CurrentRoom.SendAllPlayers(fromClientId);
            }
        }

        [MessageHandler((ushort)ClientToServerId.SpellCreateRequest)]
        public static void GetCreateSpellRequest(ushort fromClientId, Message message)
        {
            if (Players.Dictionary.TryGetValue(fromClientId, out var player))
            {
                player.CurrentRoom.CreateSpell(player, message.GetUShort(), message.GetUShort(), message.GetUShort());
            }
        }

        public void SendSpellCreated(ushort spellNetworkID, ushort spellID, Player.Player caster)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.SpellCreated);
            message.AddUShort(spellNetworkID);
            message.AddUShort(spellID);
            message.AddUShort(caster.NetworkId);
            foreach (var playerNetworkID in _room.Players.Keys)
            {
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
            }
        }

        public void SendSpellUpdatePos()
        {
            var message = Message.Create(MessageSendMode.unreliable, ServerToClientId.SpellUpdate);
            if (_room.Spells.AddUpdatableToMessage(message))
            {
                foreach (var playerNetworkID in _room.Players.Keys)
                {
                    NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
                }
            }
        }

        public void SendSpellDestroyed(ushort spellNetworkID)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.SpellDestroyed);
            
            message.AddUShort(spellNetworkID);
            
            foreach (var playerNetworkID in _room.Players.Keys)
            {
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
            }
        }
        
        public void SendEnemies()
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.Enemies);

            foreach (var enemy in _room.Enemies.Values)
            {
                message.AddEnemy(enemy);
            }
            
            foreach (var playerNetworkID in _room.Players.Keys)
            {
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
            }
        }
    }
}