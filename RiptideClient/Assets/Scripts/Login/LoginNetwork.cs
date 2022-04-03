using RiptideNetworking;
using UnityEngine;

namespace Login
{
    public class LoginNetwork : MonoBehaviour
    {
        [MessageHandler((ushort) ServerToClientId.Logpas)]
        private static void ReceiveLogPas(Message message)
        {
            if (message.GetBool())
            {
                Debug.Log($"Successfully authorized! Your id: {message.GetInt()}");
                UIManager.Singleton.SetUI((int) UIs.Character);
            }
            else
            {
                Debug.Log("Wrong login or password.");
            }
        }

        public static void SendLogPas()
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.Logpas);
        
            message.AddString(UIManager.Singleton.Login);
            message.AddString(UIManager.Singleton.Password);
        
            NetworkManager.Singleton.Client.Send(message);
        }
    }
}