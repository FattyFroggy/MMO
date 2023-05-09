using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Services
{
    class ItemService : Singleton<ItemService>, IDisposable
    {

        public ItemService()
        {
            
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.ItemEquipResponse>(this.OnItemEquip);


        }



        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.ItemEquipResponse>(this.OnItemEquip);
        }

        public void SendBuyItem(int shopId,int shopItemId)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnItemBuy(object sender, ItemBuyResponse response)
        {
            MessageBox.Show("购买结果" + response.Result + "\n" + response.Errormsg, "购买完成");

        }

        Item pendingEquip = null;
        bool isEquip;
        public bool SendItemEquip(Item equip, bool isEquip)
        {
            if (pendingEquip != null)
                return false;
            Debug.Log("SendItemEquip");

            pendingEquip = equip;
            this.isEquip = isEquip;

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot = (int)equip.EquipInfo.Slot;
            message.Request.itemEquip.itemId = equip.Id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        private void OnItemEquip(object sender, ItemEquipResponse message)
        {
            Debug.Log("ReceiveOnItemEquip");
            if (message.Result == Result.Success)
            {
                if (pendingEquip != null)
                {
                    if (this.isEquip)
                        EquipManager.Instance.OnEquipItem(pendingEquip);
                    else
                        EquipManager.Instance.OnUnEquipItem(pendingEquip.EquipInfo.Slot);
                    pendingEquip = null;
                }
            }
        }
    }
}

