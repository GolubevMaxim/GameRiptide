using System.Linq;
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
            message.AddVector2(player.transform.position);
        }
        
        public void SendNewPlayer(Player.Player player)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
            
            AddPlayerDataToMessage(message, player);
            
            foreach (var playerNetworkID in _room.Players.Keys.Where(playerNetworkID => playerNetworkID != player.NetworkId))
                NetworkManager.Singleton.Server.Send(message, playerNetworkID);
            
            message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomPlayers);
            
            foreach (var otherPlayer in _room.Players.Values)
                AddPlayerDataToMessage(message, otherPlayer);

            NetworkManager.Singleton.Server.Send(message, player.NetworkId);
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
                message.AddVector2(player.transform.position);
            }
            foreach(var playerNetworkID in _room.Players.Keys)
            {
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
            }
        }
    }
}