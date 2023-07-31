using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Manager;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class UIQuestInfo : MonoBehaviour
{
    public Button navButton;
    private int npc = 0;
    public Text title;
    public Text description;
    public Text rewardMoney;
    public Text rewardExp;
    public UIIconItem[] rewardItems;
    public Text overview;

    public Text Target;


    private void Start()
    {

    }

    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        if (this.overview != null) this.overview.text = quest.Define.Overview;

        if (this.description != null)
        {
            if (quest.Info == null)
            {
                this.description.text = quest.Define.Dialog;
            }
            else
            {
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
                else
                {
                    this.description.text = quest.Define.Overview;
                }
            }

        }
        for (int i = 0; i < rewardItems.Length; i++)
        {
            rewardItems[i].mainImage.sprite = null;
            rewardItems[i].mainText.text = null;
        }
        if (quest.Define.RewardItem1 != 0) this.rewardItems[0].SetMainIcon(DataManager.Instance.Items[quest.Define.RewardItem1].Icon, quest.Define.RewardItem1Count.ToString());
        if (quest.Define.RewardItem2 != 0) this.rewardItems[1].SetMainIcon(DataManager.Instance.Items[quest.Define.RewardItem2].Icon, quest.Define.RewardItem2Count.ToString());
        if (quest.Define.RewardItem3 != 0) this.rewardItems[2].SetMainIcon(DataManager.Instance.Items[quest.Define.RewardItem3].Icon, quest.Define.RewardItem3Count.ToString());
        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        if (quest.Info == null)
        {
            this.npc = quest.Define.AcceptNpc;
        }
        else if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
        {
            this.npc = quest.Define.SubmitNpc;
        }
        //this.Target.text = DataManager.Instance.Npcs[npc].Name;
        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    private void Update()
    {

    }

    public void OnClickAbandon()
    {

    }
    public void OnClickNav()
    {
        Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
        User.Instance.CurrentCharacterObject.StartNav(pos);
        //UIManager.Instance.Close<UIQuestSystem>();
    }
}

