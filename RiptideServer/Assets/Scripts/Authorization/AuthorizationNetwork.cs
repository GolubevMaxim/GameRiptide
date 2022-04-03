using RiptideNetworking;

namespace Authorization
{
    public class AuthorizationNetwork
    {
        [MessageHandler((ushort) ClientToServerId.Logpas)]
        private static void ReceiveLogPas(ushort fromClientId, Message message)
        {
            var user = DataBaseManager.SelectPlayerLogPas(message.GetString(), message.GetString());

            SendAnswerLogPas(user != null, fromClientId, user);
        
            if (user == null)
            {
                NetworkManager.Singleton.Server.DisconnectClient(fromClientId);
            }
            else
            {
                AuthorizationManager.Singleton.Users.Add(fromClientId, user);
                AuthorizationManager.Singleton.UsersInMenu.Add(fromClientId, user);
            }
        }

        private static void SendAnswerLogPas(bool userExists, ushort userId, User user)
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.Logpas);
        
            message.AddBool(userExists);
        
            if (userExists)
            {
                message.AddInt(user.id);
            }
        
            NetworkManager.Singleton.Server.Send(message, userId);
        }
    }
}