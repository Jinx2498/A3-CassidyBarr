using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBrains.Extensions.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Messages
{
    using QueueItem = KeyValuePair<float, string>;

    #if UNITY_EDITOR
    [FilePath(
        "ScriptableObjects/Messages/MessageQueue",
        FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(
        fileName = "MessageQueue",
        menuName = "GameBrains/Messages/MessageQueue")]
    #endif
    public class MessageQueue : ExtendedScriptableObject
    {
        [SerializeField] bool reverseOrder;
        [SerializeField] float messageLifespan = 20;
        [SerializeField] int maximumMessages = 30;
        [SerializeField] Color defaultMessageColor;

        Queue<QueueItem> messages;
        StringBuilder messageBuilder;

        public override void OnEnable()
        {
            base.OnEnable();
            messages = new Queue<QueueItem>();
            messageBuilder = new StringBuilder();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            messages = null;
            messageBuilder = null;
        }

        public int MaximumMessages => maximumMessages;

        public void AddMessage(string message, Color messageColor)
        {
            string rbgaColor = ColorUtility.ToHtmlStringRGBA(messageColor);
            string colorMessage = $"<color=#{rbgaColor}>{message}</color>";
            messages.Enqueue(new QueueItem(Time.time, colorMessage));
            Trim();
        }

        public void AddMessage(string message) { AddMessage(message, defaultMessageColor); }

        public void Clear() { messages.Clear(); }

        public string GetMessages()
        {
            Trim();

            messageBuilder.Clear();

            if (messages.Count <= 0) { return messageBuilder.ToString(); }

            var messagesAsList = reverseOrder ? messages : messages.Reverse();

            foreach (var queueItem in messagesAsList)
            {
                messageBuilder.AppendLine(queueItem.Value);
            }

            return messageBuilder.ToString();
        }

        void Trim()
        {
            while (messages.Count > 0 && messages.Peek().Key + messageLifespan < Time.time)
            {
                messages.Dequeue();
            }

            while (messages.Count > MaximumMessages) { messages.Dequeue(); }
        }
    }
}