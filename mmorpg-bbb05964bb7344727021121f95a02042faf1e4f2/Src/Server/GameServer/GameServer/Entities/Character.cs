using Common;
using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Character : CharacterBase,IPostResponser
    {
       
        public TCharacter Data;

        public StatusManager statusManager;
        public ItemManager itemManager;
        public QuestManager QuestManager;
        public FriendManager FriendManager;
        public MasterManager MasterManager;

        public Guild Guild;
        public double GuildUpdateTS;


        public Team Team;
        public double TeamUpdateTS;
        public Chat Chat;
        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Entity = this.EntityData;
            this.Info.Ride = 0;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            this.itemManager = new ItemManager(this);
            this.itemManager.GetItemInfos(this.Info.Items);
            //this.Define = DataManager.Instance.Characters[this.Info.Tid];

            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.Info.Bag.Items = this.Data.Bag.Items;

            this.Info.Equips = this.Data.Equips;
            

            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);

            this.statusManager = new StatusManager(this);

            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);


 
            this.MasterManager = new MasterManager(this);
            //this.Info.Master = new NMasterInfo();
            // this.Info.Master.Id = this.Data.Master.MasterId;
            //this.Info.Master.masterInfo =this.
            //this.MasterManager.GetMasterInfo(this.Info.Master);
            this.Info.Master = this.MasterManager.GetMasterInfo();
            this.MasterManager.GetApprenticeInfos(this.Info.Apprentices);

            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildId);
            if (Guild != null)
            {
                this.Info.Guild = this.Guild.GuildInfo(this);
            }

            this.Chat = new Chat(this);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value)
                {
                    return;
                }
                this.statusManager.AddGoldChange((int)value -(int)this.Data.Gold);
                this.Data.Gold = value;
            }

        }
        public int Ride
        {
            get{ return this.Info.Ride; }
            set
            {
                if (this.Info.Ride == value)
                    return;
                this.Info.Ride = value;
            }
        }
        public void PostProcess(NetMessageResponse message)
        {
            this.FriendManager.PostProcess(message);
            this.MasterManager.PostProcess(message);
            if (this.Team != null)
            {
                if (TeamUpdateTS < Team.timestamp)
                {
                    TeamUpdateTS = Team.timestamp;
                    this.Team.PostProcess(message);
                }
            }

            if (this.statusManager.HasStatus)
            {
                this.statusManager.PostProcess(message);
            }

            if (this.Guild != null)
            {
                Log.InfoFormat("PostProcess>Guild:characterID:{0}:{1} {2}<{3}", this.Id, this.Info.Name, GuildUpdateTS, this.Guild.timestamp);
                if (this.Info.Guild == null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (message.mapCharacterEnter != null)
                        GuildUpdateTS = Guild.timestamp;
                }
                if (GuildUpdateTS < Guild.timestamp &&message.mapCharacterEnter==null)
                {
                    GuildUpdateTS = Guild.timestamp;
                    this.Guild.PostProcess(this,message);
                }
               
            }
            this.Chat.PostProcess(message);


        }

        internal void Clear()
        {
            this.FriendManager.offlineNotify();
            this.MasterManager.offlineNotify();
        }

        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = this.Id,
                Name = this.Info.Name,
                Class = this.Info.Class,
                Level = this.Info.Level,
            };
        }
    }
}
