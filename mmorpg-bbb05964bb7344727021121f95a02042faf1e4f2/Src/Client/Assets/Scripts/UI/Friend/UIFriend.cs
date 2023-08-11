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
        InputBox.Show("����Ҫ��Ӻ���id������", "��Ӻ���").OnSubmit += OnFriendAddSubmit;
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
            tips = "�޷�����Լ�";
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
    /// ��ʦ
    /// </summary>
    public void OnClickMasterInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("��ѡ�����ߺ���");
            return;
        }
        if (User.Instance.CurrentCharacter.Master == null)
        {
            MessageBox.Show(string.Format("ȷ��Ҫ��[{0}]Ϊʦ��?", selectedItem.info.friendInfo.Name), "��ʦ����,", MessageBoxType.Confirm, "����", "ȡ��").OnYes = () =>
            {
                MasterService.Instance.SendMasterRequest(this.selectedItem.info.friendInfo.Id, this.selectedItem.info.friendInfo.Name);
            };
        }
        else
        {
            MessageBox.Show("������ʦ��,�޷��ٴΰ�ʦ");
        }

    }
    /// <summary>
    /// ��ͽ
    /// </summary>
    public void OnClickApprenticeInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("��ѡ�����ߺ���");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ��[{0}]Ϊͽ��?", selectedItem.info.friendInfo.Name), "��ͽ����,", MessageBoxType.Confirm, "����", "ȡ��").OnYes = () =>
        {
            MasterService.Instance.SendApprenticeRequest(this.selectedItem.info.friendInfo.Id, this.selectedItem.info.friendInfo.Name);
        };
    }
    public void OnClickFriendTeamInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("��ѡ�����ߺ���");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ�������[{0}]��?", selectedItem.info.friendInfo.Name), "����������,", MessageBoxType.Confirm, "����", "ȡ��").OnYes = () =>
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
            MessageBox.Show("��ѡ��");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫɾ������[{0}]��?", selectedItem.info.friendInfo.Name), "ɾ������", MessageBoxType.Confirm, "ɾ��", "ȡ��").OnYes = () =>
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
