using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem
{
    // Start is called before the first frame update
    public Text nickname;
    public Image classIcon;
    public Image leaderIcon;

    public Image background;


    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    private void Start()
    {
        this.background.enabled = false;
    }
    public int idx;
    public NCharacterInfo info;

    public void SetMemberInfo(int idx,NCharacterInfo item,bool isLeader)
    {
        this.idx = idx;
        this.info = item;
        if (this.nickname != null) this.nickname.text = this.info.Name.ToString();
        if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)item.Class - 1];
        if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLeader);

    }

}
