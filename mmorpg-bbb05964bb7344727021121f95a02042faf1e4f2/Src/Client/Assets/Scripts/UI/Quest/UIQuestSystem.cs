using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


 class UIQuestSystem:UIWindow
    {
        public Text title;


        public GameObject itemPrefab;

        public TabView Tabs;
        public ListView listMain;
        public ListView listBranch;

        public UIQuestInfo questInfo;

        private bool showAvailableList = false;

        private void Start()
        {
            this.listMain.onItemSelected += this.OnQuestSelected;
            this.listBranch.onItemSelected += this.OnQuestSelected;
            this.Tabs.OnTabSelect += this.OnSelectTab;
            RefreshUI();
            QuestManager.Instance.OnQuestChanged += RefreshUI;
        }
        void OnSelectTab(int idx)
        {
            showAvailableList = idx == 1;
            RefreshUI();
        }
        private void OnDestroy()
        {
             QuestManager.Instance.OnQuestChanged -= RefreshUI;
        }

        /// <summary>
        /// //
        /// </summary>
        private void RefreshUI()
        {
            ClearAllQuestList();
            InitAllQuestItems();
        }

        private void InitAllQuestItems()
        {
            foreach(var kv in QuestManager.Instance.allQuests)
            {
                if(showAvailableList)
                {
                    if (kv.Value.Info != null)
                        continue;
                }
                else
                {
                    if (kv.Value.Info == null)
                        continue;
                }

                GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
                UIQuestItem ui = go.GetComponent<UIQuestItem>();
                ui.SetQuestInfo(kv.Value);
                if (kv.Value.Define.Type == QuestType.Main)
                    this.listMain.AddItem(ui as ListView.ListViewItem);
                else
                    this.listBranch.AddItem(ui as ListView.ListViewItem);
            }
        }

        private void ClearAllQuestList()
        {
            this.listBranch.RemoveAll();
            this.listMain.RemoveAll();
        }


        public void OnQuestSelected(ListView.ListViewItem item)
        {
            UIQuestItem questiItem = item as UIQuestItem;
            this.questInfo.SetQuestInfo(questiItem.quest);

        }
}

