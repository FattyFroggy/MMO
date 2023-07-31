using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


class UIQuestItem : ListView.ListViewItem
{

    public Text title;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    public Quest quest;
    private void Start()
    {

    }

    public void SetQuestInfo(Quest quest)
    {
        this.quest = quest;
        if (this.title != null)
        {
            if (quest.Define.Type == Common.Data.QuestType.Main)
            {

                if (quest.Info != null)
                {
                    this.title.text = "|主线|" + this.quest.Define.Name + "[" + quest.Info.Status.ToString() + "]";
                }
                else
                {
                    this.title.text = "|主线|" + this.quest.Define.Name;
                }

            }
            else
            {
                if (quest.Info != null)
                {
                    this.title.text = "|支线|" + this.quest.Define.Name + "[" + quest.Info.Status.ToString() + "]";
                }
                else
                {
                    this.title.text = "|支线|" + this.quest.Define.Name;
                }

            }
        }
    }



}
