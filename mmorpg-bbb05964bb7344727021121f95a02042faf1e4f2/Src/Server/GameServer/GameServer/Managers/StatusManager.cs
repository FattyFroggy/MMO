using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class StatusManager
    {
        Character Owner;

        private List<NStatus> Status { get; set; }
        public bool HasStatus 
        { 
            get { return this.Status.Count>0; }
        }

        public StatusManager(Character owner)
        {
            this.Owner = owner;
            this.Status = new List<NStatus>();
        }

        private  void AddGoldStatus(StatusType type,int id,int value,StatusAction action)
        {
            this.Status.Add(new NStatus()
            {
                Type = type,
                Id=id,
                Value=value,
                Action=action,
            }) ;
        }

        public void AddGoldChange(int goldDelata)
        {
            if (goldDelata > 0)
            {
                this.AddGoldStatus(StatusType.Money, 0, goldDelata, StatusAction.Add);
            }
            if (goldDelata < 0)
            {
                this.AddGoldStatus(StatusType.Money, 0, goldDelata, StatusAction.Add);
            }
        }
        public void AddItemChange(int id,int count ,StatusAction action)
        {
            this.AddGoldStatus(StatusType.Item, id, count, action);
        }
        internal void PostProcess(NetMessageResponse message)
        {

            if (message.statusNotify == null)
                message.statusNotify = new StatusNotify();
            foreach(var status in this.Status){
                message.statusNotify.Status.Add(status);
            }
            this.Status.Clear();
        }
    }
}
