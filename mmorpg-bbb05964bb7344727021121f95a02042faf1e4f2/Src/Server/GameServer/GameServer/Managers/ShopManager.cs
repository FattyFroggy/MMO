using Common;
using Common.Data;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ShopManager:Singleton<ShopManager>
    {
        public Result BuyItem(NetConnection<NetSession> sender,int shopId,int shopItemId)
        {
            Log.Info("BuyItem");
            if (!DataManager.Instance.Shops.ContainsKey(shopId))
                return Result.Failed;
            ShopItemDefine shopItem;
            if(DataManager.Instance.ShopItems[shopId].TryGetValue(shopItemId,out shopItem))
            {
                if (sender.Session.Character.Gold > shopItem.Price)
                {
                    //int id;
                    //id = DataManager.Instance.ShopItems[shopId][shopItemId].ItemID;
                    sender.Session.Character.itemManager.AddItem(shopItem.ItemID, shopItem.Count);
                    sender.Session.Character.Gold -= shopItem.Price;
                }
                DBService.Instance.Save();
                return Result.Success;
            }
            return Result.Failed;
        } 

    }
}
