using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    class UIGuildInfo:MonoBehaviour
    {
        public Text guildName;
        public Text guildID;
        public Text leader;

        public Text notice;
        public Text memberNumber;

        private NGuildInfo info;
        public NGuildInfo Info
        {
            get { return info; }
            set { this.info = value;this.UpdateUI(); }
        }
        
        void UpdateUI()
        {
            if (this.info == null){
                this.guildName.text = "0";
                this.guildID.text = "0";
                this.leader.text = "0";
                this.notice.text = "0";
                this.memberNumber.text = "0";
            }
            else
            {
                this.guildName.text = this.info.Guildname;
                this.guildID.text = "ID:"+this.info.Id;
                this.leader.text = "会长:"+this.info.leaderName;
                this.notice.text = "公会宣言:"+this.info.Notice;

                this.memberNumber.text =string.Format("成员数量:{0}/{1}",this.info.memberCount,50);
            }
            //GameDefine.GuildMaxMemberCount
        }
    }
}
