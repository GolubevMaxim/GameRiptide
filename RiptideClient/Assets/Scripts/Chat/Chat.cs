using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Chat
{
    public class Chat : MonoBehaviour
    {
        [Header("Chat Log")]
        [SerializeField] private GameObject chatLog;
        [SerializeField] private GameObject chatMessageButtonPrefab;
        [SerializeField] private int maxMessageCount = 10;
        [SerializeField] private TMP_InputField chatField;

        private readonly Queue<GameObject> _chatMessageButtons = new();

        public bool IsFocused => chatField.isFocused;

        private void Start()
        {
            ChatNetwork.CurrentChat = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                chatField.Select();
                chatField.ActivateInputField();
            }
        }

        public void SendChatClicked()
        {
            if (chatField.text.Length > 0) ChatNetwork.SendChatMessage(chatField.text);
            chatField.text = "";
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void ChatLogAdd(int playerID, string str)
        {
            var button = Instantiate(chatMessageButtonPrefab, chatLog.transform, true);
            
            button.transform.GetComponent<Text>().text = $"{playerID}: {str}";
            button.transform.GetComponent<Text>().fontSize = 28;
            
            Debug.Log($"{playerID}: {str}");
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatLog.GetComponent<RectTransform>());

            _chatMessageButtons.Enqueue(button);

            if (_chatMessageButtons.Count > maxMessageCount)
            {
                var oldButton = _chatMessageButtons.Dequeue();
                Destroy(oldButton);
            }
        }
    }
}