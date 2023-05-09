using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Guild
{
    class UIGuild:UIWindow
    {
        public GameObject itemPrefab;
        public ListView listMain;
        public Transform itemRoot;
        public UIGuildInfo uiInfo;
        public UIGuildMemberItem selectedItem;

        public GameObject panelAdmin;
        public GameObject panelLeader;
        private void Start()
        {
            UpdateUI();
            GuildService.Instance.OnGuildUpdate = UpdateUI;
            this.listMain.onItemSelected += this.OnGuildMemberSelected;
            this.UpdateUI();
        }
        private void OnDestroy()
        {
            GuildService.Instance.OnGuildUpdate -= UpdateUI;
        }
        void UpdateUI()
        {

            this.uiInfo.Info = GuildManager.Instance.guildInfo;
            ClearList();
            InitItems();

            this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
            this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
        }



        public void OnGuildMemberSelected(ListView.ListViewItem item)
        {
            this.selectedItem = item as UIGuildMemberItem;
        }

        private void InitItems()
        {
            foreach (var item in GuildManager.Instance.guildInfo.Members)
            {
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
                ui.SetGuildMemberInfo(item);
                this.listMain.AddItem(ui);
            }
        }

        private void ClearList()
        {
            this.listMain.RemoveAll();
        }

        public void OnClickAppliesList()
        {
            UIManager.Instance.Show<UIGuildApplyList>();
        }
        public void OnClickLeave()
        {

        }
        public void OnClickChat()
        {

        }
        public void OnClickKickout()
        {
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要踢出的成员");
                return;
            }
            MessageBox.Show(string.Format("要踢出[{0}]公会吗?", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
            };

        }
        public void OnClickPromote()
        {
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要升级的成员");
                return;
            }
            if (selectedItem.Info.Title != GuildTitle.None)
            {
                MessageBox.Show("对方已身份高贵");
                return;
            }
            MessageBox.Show(string.Format("要晋升[{0}]吗?", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
            };
        }
        public void OnClickDepose()
        {
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要降级的成员");
                return;
            }
            if (selectedItem.Info.Title != GuildTitle.None)
            {
                MessageBox.Show("对方已是平民");
                return;
            }
            MessageBox.Show(string.Format("要降职[{0}]吗?", this.selectedItem.Info.Info.Name), "降职", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depose, this.selectedItem.Info.Info.Id);
            };
        }
        public void OnClickTransfer()
        {
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要降级的成员");
                return;
            }
            MessageBox.Show(string.Format("要把会长转让给[{0}]吗?", this.selectedItem.Info.Info.Name), "会长转让", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
            };
        }
        public void OnClickSetNotice()
        {

        }
    }
    
}
