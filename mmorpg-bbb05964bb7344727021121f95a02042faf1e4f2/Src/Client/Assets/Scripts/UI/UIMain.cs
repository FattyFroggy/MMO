using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ride;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{

    public Text avatarName;
    public Text avatarLevel;

    public UITeam TeamWindow;
    // Use this for initialization
    protected override void  OnStart()
    {
        this.UpdateAvatar();

    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BackToCharSelect()
    {

    }

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        //test.gameObject.SetActive(true);
        test.SetTitle(" 测试");
        test.OnClose += Test_Onclose;
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
  

    }
    public void OnClickEquip()
    {
        UIManager.Instance.Show<UICharEquip>();

    }
    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();

    }

    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriend>();

    }
    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();

    }

    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();

    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }
    public void OnClickSkill()
    {

    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
    private void Test_Onclose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框" + result, "对画框响应结果",MessageBoxType.Information);
    }
}
