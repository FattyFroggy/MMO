using Common.Utils;
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
    class UIGuildMemberItem:ListView.ListViewItem
    {
        public Text nickname;
        public Text @class;
        public Text level;
        public Text title;
        public Text joinTime;
        public Text status;

        public Image background;
        public Sprite normalBg;
        public Sprite selectedBg;

        public override void onSelected(bool selected)
        {
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
        public NGuildMemberInfo Info;
        
        public void SetGuildMemberInfo(NGuildMemberInfo item)
        {
            this.Info = item;
            if (this.nickname != null) this.nickname.text = this.Info.Info.Name;
            if(this.@class != null) this.@class.text = this.Info.Info.Class.ToString();
            if(this.level != null) this.level.text = this.Info.Info.Level.ToString();
            if(this.title != null) this.title.text = this.Info.Title.ToString();
            if (this.joinTime != null) this.joinTime.text = TimeUtil.timestamp.ToString();
            if (this.status != null) this.status.text = this.Info.joinTime==1?"在线":"离线";


        }


    }

}
