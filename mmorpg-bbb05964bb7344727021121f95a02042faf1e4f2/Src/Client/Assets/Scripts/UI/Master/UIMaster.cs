using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : UIWindow
{
    // Start is called before the first frame update
    public GameObject itemPrefab;
    public ListView MasterlistMain;
    public ListView ApprenticelistMain;
    public Transform itemRoot;
    public UIMasterItem selectedItem;
    void Start()
    {
        MasterService.Instance.OnMasterUpdate = RefreshUI;
        this.MasterlistMain.onItemSelected += this.OnMasterSelected;
        this.ApprenticelistMain.onItemSelected += this.OnMasterSelected;
        RefreshUI();
        
    }
    private void OnMasterSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIMasterItem;
    }

    private void OnDestroy()
    {
        MasterService.Instance.OnMasterUpdate = null;
        this.MasterlistMain.onItemSelected -= this.OnMasterSelected;
        this.ApprenticelistMain.onItemSelected -= this.OnMasterSelected;
    }

    public void OnClickMasterRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��");
            return;
        }
        if (selectedItem.isMaster)
        {
            MessageBox.Show(string.Format("ȷ��Ҫ��[{0}]���ʦͽ��ϵ����?", selectedItem.masterInfo.masterInfo.Name), "���ʦͽ��ϵ", MessageBoxType.Confirm, "ɾ��", "ȡ��").OnYes = () =>
            {
                MasterService.Instance.SendMasterRemoveRequest(this.selectedItem.masterInfo.Id, this.selectedItem.masterInfo.masterInfo.Id);
            };
        }else if (!selectedItem.isMaster)
        {
            MessageBox.Show(string.Format("ȷ��Ҫ��[{0}]���ʦͽ��ϵ����?", selectedItem.apprenticeInfo.apprenticeInfo.Name), "���ʦͽ��ϵ", MessageBoxType.Confirm, "ɾ��", "ȡ��").OnYes = () =>
            {
                MasterService.Instance.SendApprenticeRemoveRequest(this.selectedItem.apprenticeInfo.Id, this.selectedItem.apprenticeInfo.apprenticeInfo.Id);
            };
        }

    }

    private void RefreshUI()
    {
        ClearMasterList();
        InitMasterItems();
    }


    private void InitMasterItems()
    {
        var item = MasterManager.Instance.master;
        if (item != null) {
            GameObject go = Instantiate(itemPrefab, this.MasterlistMain.transform);
            UIMasterItem ui = go.GetComponent<UIMasterItem>();
            ui.SetMasterInfo(true, item);
            this.MasterlistMain.AddItem(ui);
        }


        foreach (var apprentice in MasterManager.Instance.apprentices)
        {
            GameObject appgo = Instantiate(itemPrefab, this.ApprenticelistMain.transform);
            UIMasterItem appui = appgo.GetComponent<UIMasterItem>();
            appui.SetMasterInfo(false,null,apprentice);
            this.ApprenticelistMain.AddItem(appui);
        }
    }

    private void ClearMasterList()
    {
        this.MasterlistMain.RemoveAll();
        this.ApprenticelistMain.RemoveAll();
    }
}
