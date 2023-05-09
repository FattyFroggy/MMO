using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Common.Data;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class StatusManager : Singleton<StatusManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id,item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        private bool OnItemNotify(NStatus status)
        {
            if (status.Action == StatusAction.Add)
            {
                
                this.AddItem(status.Id, status.Value);
                
            }
            if (status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
            return true;
        }

        public void AddItem(int itemId,int count)
        {
           
            Item item = null;
            if(this.Items.TryGetValue(itemId,out item))
            {
                item.Count += count;
            }
            else
            {
                this.Items.Add(itemId, new Item(itemId, count));
            }
            //Debug.LogFormat("[{0}]",item.)
            BagManager.Instance.AddItem(itemId, count);
        }

        public void RemoveItem(int itemId, int count)
        {
          
            if (this.Items.ContainsKey(itemId))
            {
                return;
            }
            Item item = this.Items[itemId];
            if (item.Count < count)
                return;
            item.Count -= count;
            BagManager.Instance.RemoveItem(itemId, count);
        }

        public bool UseItem(int itemId)
        {
            return false;
        }
        public bool UseItem(ItemDefine item)
        {
            return false;
        }
        public StatusManager()
        {
            
        }

    }
}
