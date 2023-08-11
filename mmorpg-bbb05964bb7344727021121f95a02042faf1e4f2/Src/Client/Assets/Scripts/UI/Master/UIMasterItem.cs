using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMasterItem : ListView.ListViewItem
{
    // Start is called before the first frame update
    public Text nickname;
    public Text @class;
    public Text level;
    public Text status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public bool isMaster;
    public override void onSelected(bool selected)
    {
        Debug.Log("UIMasterItemonSelected");
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    private void Start()
    {

    }
    public NMasterInfo masterInfo;
    public NApprenticeInfo apprenticeInfo;


    internal void SetMasterInfo(bool isMaster,NMasterInfo master=null,NApprenticeInfo apprentice=null)
    {
        this.isMaster = isMaster;
        if (isMaster == true)
        {
            this.masterInfo = master;
            if (this.nickname != null) this.nickname.text = this.masterInfo.masterInfo.Name;
            if (this.@class != null) this.@class.text = this.masterInfo.masterInfo.Class.ToString();
            if (this.level != null) this.level.text = this.masterInfo.masterInfo.Level.ToString();
            if (this.status != null) this.status.text = this.masterInfo.Status == 1 ? "在线" : "离线";
        }
        else
        {
            this.apprenticeInfo = apprentice;
            if (this.nickname != null) this.nickname.text = this.apprenticeInfo.apprenticeInfo.Name;
            if (this.@class != null) this.@class.text = this.apprenticeInfo.apprenticeInfo.Class.ToString();
            if (this.level != null) this.level.text = this.apprenticeInfo.apprenticeInfo.Level.ToString();
            if (this.status != null) this.status.text = this.apprenticeInfo.Status == 1 ? "在线" : "离线";
        }

    }
}
