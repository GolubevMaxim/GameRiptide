﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chat
{
    public class Chat : MonoBehaviour
    {
        [Header("Chat Log")] [SerializeField] private GameObject chatLog;
        [SerializeField] private GameObject chatMessageButtonPrefab;
        [SerializeField] private int maxMessageCount = 10;
        [SerializeField] private TMP_InputField chatField;

        private readonly Queue<GameObject> _chatMessageButtons = new();


        private void Start()
        {
            ChatNetwork.CurrentChat = this;
        }

        public void SendChatClicked()
        {
            if (chatField.text.Length > 0) ChatNetwork.SendChatMessage(chatField.text);
            chatField.text = "";
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