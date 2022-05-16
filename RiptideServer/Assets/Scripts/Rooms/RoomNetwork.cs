using System.Linq;
using Player;
using RiptideNetworking;

namespace Rooms
{
    public class RoomNetwork
    {
        private Room _room;

        public RoomNetwork(Room room)
        {
            _room = room;
        }

        private void AddPlayerDataToMessage(Message message, Player.Player player)
        {
            message.AddUShort(player.NetworkId);
            message.AddString(player.NickName);
            message.AddVector2(player.transform.localPosition);
            message.AddInt(player.playerHealth.HealthMax);
            message.AddInt(player.playerHealth.Health);
        }
        
        public void SendNewPlayer(Player.Player player)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
            
            AddPlayerDataToMessage(message, player);
            
            foreach (var playerNetworkID in _room.Players.Keys.Where(playerNetworkID => playerNetworkID != player.NetworkId))
                NetworkManager.Singleton.Server.Send(message, playerNetworkID);
            
            SendAllPlayers(player.NetworkId);
        }
        
        public void SendAllPlayers(ushort playerId)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
            
            foreach (var otherPlayer in _room.Players.Values)
                AddPlayerDataToMessage(message, otherPlayer);

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
    }
}