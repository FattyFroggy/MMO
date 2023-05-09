﻿using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Guild
{
    class UIGuildApplyList:UIWindow
    {
        public GameObject itemPrefab;
        public ListView listMain;
     
        public Transform itemRoot;

        private void Start()
        {
            GuildService.Instance.OnGuildUpdate += UpdateList;
            GuildService.Instance.SendGuildListRequest();
            this.UpdateList();
        }
        private void OnDestroy()
        {
            GuildService.Instance.OnGuildUpdate -= UpdateList;
        }
        private void UpdateList()
        {
            ClearList();
            InitItems();
        }

        private void InitItems()
        {
            foreach(var item in GuildManager.Instance.guildInfo.Applies)
            {
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>();
                ui.SetItemInfo(item);
                this.listMain.AddItem(ui);
            }
        }

        private void ClearList()
        {
            this.listMain.RemoveAll();
        }
    }
}
