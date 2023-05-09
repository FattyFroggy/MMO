using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using SkillBridge.Message;
namespace Assets.Scripts.Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;
        public EquipDefine EquipInfo;

        public Item(NItemInfo item): this(item.Id, item.Count)
        {

        }
        
        public Item(int id,int count)
        {
            this.Id = id;
            this.Count = count;
            this.Define = DataManager.Instance.Items[id];
            if (this.Define.Type ==ItemType.Equip)
            {
                this.EquipInfo = DataManager.Instance.Equips[id];
            }
            //this.EquipInfo = DataManager.Instance.Equips[id];
        }
        public override string ToString()
        {
            return string.Format("Id:{0},Count:{1}", this.Id, this.Count);
        }
    }
}
