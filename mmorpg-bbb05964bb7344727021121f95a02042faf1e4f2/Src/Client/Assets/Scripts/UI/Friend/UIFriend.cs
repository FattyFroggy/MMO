using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriend : UIWindow
{
    // Start is called before the first frame update
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIFriendItem selectedItem;
    void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected;
        RefreshUI();
        
    }
    private void OnDestroy()
    {
        Debug.Log("OnDestory");
        FriendService.Instance.OnFriendUpdate = null;
    }
    private void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }
    public void OnClickFriendAdd()
    {
        InputBox.Show("输入要添加好友id或名称", "添加好友").OnSubmit += OnFriendAddSubmit;
    }
    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input, out friendId))
            friendName = input;
        if (friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
            tips = "无法添加自己";
            return false;
        }

        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 拜师
    /// </summary>
    public void OnClickMasterInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("请选择在线好友");
            return;
        }
        if (User.Instance.CurrentCharacter.Master == null)
        {
            MessageBox.Show(string.Format("确定要拜[{0}]为师吗?", selectedItem.info.friendInfo.Name), "拜师请求,", MessageBoxType.Confirm, "邀请", "取消").OnYes = () =>
            {
                MasterService.Instance.SendMasterRequest(this.selectedItem.info.friendInfo.Id, this.selectedItem.info.friendInfo.Name);
            };
        }
        else
        {
            MessageBox.Show("你已有师父,无法再次拜师");
        }

    }
    /// <summary>
    /// 收徒
    /// </summary>
    public void OnClickApprenticeInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("请选择在线好友");
            return;
        }
        MessageBox.Show(string.Format("确定要收[{0}]为徒吗?", selectedItem.info.friendInfo.Name), "收徒请求,", MessageBoxType.Confirm, "邀请", "取消").OnYes = () =>
        {
            MasterService.Instance.SendApprenticeRequest(this.selectedItem.info.friendInfo.Id, this.selectedItem.info.friendInfo.Name);
        };
    }
    public void OnClickFriendTeamInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("请选择在线好友");
            return;
        }
        MessageBox.Show(string.Format("确定要邀请好友[{0}]吗?", selectedItem.info.friendInfo.Name), "邀请好友组队,", MessageBoxType.Confirm, "邀请", "取消").OnYes = () =>
             {
                 TeamService.Instance.SendTeamInviteRequest(this.selectedItem.info.friendInfo.Id, this.selectedItem.info.friendInfo.Name);
             };
    }
    public void OnClickFriendChat()
    {
        ChatManager.Instance.StartPrivateChat(selectedItem.info.friendInfo.Id, selectedItem.info.friendInfo.Name);
        this.Close(WindowResult.None);
    }

    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择");
            return;
        }
        MessageBox.Show(string.Format("确定要删除好友[{0}]吗?", selectedItem.info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.info.Id, this.selectedItem.info.friendInfo.Id);
        };
    }

    private void RefreshUI()
    {
        ClearFriendList();
        InitFriednItems();
    }


    private void InitFriednItems()
    {

        foreach(var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearFriendList()
    {
        this.listMain.RemoveAll();
    }
}
