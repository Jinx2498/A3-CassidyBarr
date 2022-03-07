using GameBrains.Extensions.MonoBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameBrains.Messages
{
    [AddComponentMenu("Scripts/GameBrains/Messages/Message Displayer")]
    public class MessageDisplayer : ExtendedMonoBehaviour
    {
        public MessageQueue messageQueue;
        public TextMeshProUGUI uiProText;
        public TextMeshPro proText;
        public Text uiText;

        public override void Update()
        {
            base.Update();
            var messages = messageQueue.GetMessages();

            if (uiProText) { uiProText.text = messages; }
            if (proText) { proText.text = messages; }
            if (uiText) { uiText.text = messages; }
        }
    }
}