using Assets.Scripts.Managers;
using Candlelight.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Chat
{
    class UIChat : MonoBehaviour
    {
        public HyperText textArea;
        public TabView channelTab;
        public InputField chatText;
        public Text chatTarget;
        public Dropdown channelSelect;
        private void Start()
        {
            this.channelTab.OnTabSelect += OnDisplayChannelSelected;
            ChatManager.Instance.OnChat += RefreshUI;
        }
        private void OnDestroy()
        {
            ChatManager.Instance.OnChat -= RefreshUI;
        }

        private void Update()
        {
            InputManager.Instance.IsInputMode = chatText.isFocused;

        }
        void OnDisplayChannelSelected(int idx)
        {
            ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
            RefreshUI();
        }
        public void RefreshUI()
        {
 
            this.textArea.text = ChatManager.Instance.GetCurrentMessage();
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
         
            if (ChatManager.Instance.SendChannel == SkillBridge.Message.ChatChannel.Private)
            {
                this.chatTarget.gameObject.SetActive(true);
                if (ChatManager.Instance.privateID != 0)
                {
                    this.chatTarget.text = ChatManager.Instance.PrivateName + ":";
                }
                else
                {
                    this.chatTarget.text = "<无>";
                }
            }
            else
            {
                this.chatTarget.gameObject.SetActive(false);
            }

        }
        public void OnClickChatLink(HyperText text,HyperText.LinkInfo link)
        {
            //Debug.LogWarning(link.Name) ;
            if (string.IsNullOrEmpty(link.Name))
                return;
            //<A NAME="C:1001:NAME" class="player">Name</a>

            //<I NAME="C:1001:NAME" class="iteam">Name</a>
            if (link.Name.StartsWith("c"))
            {
                string[] strs = link.Name.Split(":".ToCharArray());
                UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();

                menu.targetId = int.Parse(strs[1]);
                menu.targetName = strs[2];
            }
        }

        public void OnClickSend()
        {
            OnEndInput(this.chatText.text);
        }
        public void OnEndInput(string text)
        {
            if (!string.IsNullOrEmpty(text.Trim()))
                this.SendChat(text);

            this.chatText.text = "";
        }
        void SendChat(string content)
        {
            ChatManager.Instance.SendChat(content, ChatManager.Instance.privateID, ChatManager.Instance.PrivateName);
        }
        public void OnSendChannelChanged(int idx)
        {
            if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
                return;
            if (!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)idx + 1))
            {
                this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;

;            }
            else
            {
                this.RefreshUI();
            }
        }
    }   
}
