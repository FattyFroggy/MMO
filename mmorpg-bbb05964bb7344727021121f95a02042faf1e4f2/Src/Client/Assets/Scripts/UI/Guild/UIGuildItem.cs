﻿using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    class UIGuildItem:ListView.ListViewItem
    {
        public Text GuildId;
        public Text GuildName;
        public Text GuildMemberCount;
        public Text Leader;
        private NGuildInfo info;
        public NGuildInfo Info
        {
            get { return info; }
            set { this.info = value; }
        }

        internal void SetGuildInfo(NGuildInfo item)
        {
            this.Info = item;
            if (this.GuildId != null) this.GuildId.text = this.Info.Id.ToString();
            if (this.GuildName != null) this.GuildName.text = this.Info.Guildname;
            if (this.GuildMemberCount != null) this.GuildMemberCount.text = this.Info.memberCount.ToString();
            if (this.Leader != null) this.Leader.text = this.Info.leaderName;

        }
    }
}
