using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class MasterManager
    {
        Character Owner;
        NMasterInfo master = new NMasterInfo();
        List<NApprenticeInfo> apprentices = new List<NApprenticeInfo>();

        bool masterChanged = false;

        bool ApprenticeChanged = false;

        public MasterManager(Character owner)
        {
            this.Owner = owner;
            this.InitMaster();
            this.InitApprentices();
        }

        private void InitMaster()
        {
            this.master = null;
            var mas = this.Owner.Data.Master;
            this.master = this.GetMasterInfo(mas);

        }
        private void InitApprentices()
        {
            this.apprentices.Clear();
            foreach (var apprentice in this.Owner.Data.Apprentices)
            {
                apprentices.Add(GetApprenticeInfo(apprentice));
            }
        }
        public void AddMaster(Character master)
        {
            TCharacterMaster tm = new TCharacterMaster()
            {
                MasterId = master.Id,
                MasterName = master.Data.Name,
                Class = master.Data.Class,
                Level = master.Data.Level,
                CharacterID = this.Owner.Id,
            };
            this.Owner.Data.Master=tm;
            this.master = GetMasterInfo(tm);
            masterChanged = true;
        }
        public void AddApprentice(Character apprentice)
        {
            TCharacterApprentice ta = new TCharacterApprentice()
            {
                ApprenticeId = apprentice.Id,
                ApprenticeName = apprentice.Data.Name,
                Class = apprentice.Data.Class,
                Level = apprentice.Data.Level,
                CharacterID = this.Owner.Id,
            };
            this.Owner.Data.Apprentices.Add(ta);
            this.apprentices.Add(this.GetApprenticeInfo(ta));
            ApprenticeChanged = true;
        }
        public bool RemoveMaster()
        {
            var removeItem = this.Owner.Data.Master;
            if (removeItem != null)
            {
                DBService.Instance.Entities.TCharacterMasters.Remove(removeItem);
            }
            masterChanged=true;
            return true;
        }
        public bool RemoveAppenticeByID(int apprenticeID)
        {
            var removeItem = this.Owner.Data.Apprentices.FirstOrDefault(v=>v.ApprenticeId==apprenticeID);
            if (removeItem != null)
            {
                DBService.Instance.Entities.TCharacterApprentices.Remove(removeItem);
            }
            ApprenticeChanged = true;
            return true;
        }

        public NMasterInfo GetMasterInfo(TCharacterMaster master)
        {
            if (master == null) return null;
            NMasterInfo masterInfo = new NMasterInfo();
            var character = CharacterManager.Instance.GetCharacter(master.MasterId);
            masterInfo.masterInfo = new NCharacterInfo();
            masterInfo.Id = master.Id;
            if (character == null)
            {
                masterInfo.masterInfo.Id = master.MasterId;
                masterInfo.masterInfo.Name = master.MasterName;
                masterInfo.masterInfo.Class = (CharacterClass)master.Class;
                masterInfo.masterInfo.Level = master.Level;
                masterInfo.Status = 0;
            }
            else
            {
                masterInfo.masterInfo = character.GetBasicInfo();
                masterInfo.masterInfo.Name = character.Info.Name;
                masterInfo.masterInfo.Class = character.Info.Class;
                masterInfo.masterInfo.Level = character.Info.Level;

                if (master.Level != character.Info.Level)
                {
                    master.Level = character.Info.Level;
                }

                character.MasterManager.UpdateMasterInfo(this.Owner.Info, 1);
                masterInfo.Status = 1;
            }
            return masterInfo;
        }
        public NMasterInfo GetMasterInfo()
        {

            return this.master;
        }
        //public void GetMasterInfo(NMasterInfo Master)
        //{
        //    master = this.master;
        //}
        public NApprenticeInfo GetApprenticeInfo(TCharacterApprentice apprentice)
        {
            NApprenticeInfo apprenticeInfo = new NApprenticeInfo();
            var character = CharacterManager.Instance.GetCharacter(apprentice.ApprenticeId);
            apprenticeInfo.apprenticeInfo = new NCharacterInfo();
            apprenticeInfo.Id = apprentice.Id;
            if (character == null)
            {
                apprenticeInfo.apprenticeInfo.Id = apprentice.ApprenticeId;
                apprenticeInfo.apprenticeInfo.Name = apprentice.ApprenticeName;
                apprenticeInfo.apprenticeInfo.Class = (CharacterClass)apprentice.Class;
                apprenticeInfo.apprenticeInfo.Level = apprentice.Level;
                apprenticeInfo.Status = 0;
            }
            else
            {
                apprenticeInfo.apprenticeInfo = character.GetBasicInfo();
                apprenticeInfo.apprenticeInfo.Name = character.Info.Name;
                apprenticeInfo.apprenticeInfo.Class = character.Info.Class;
                apprenticeInfo.apprenticeInfo.Level = character.Info.Level;

                if (apprentice.Level != character.Info.Level)
                {
                    apprentice.Level = character.Info.Level;
                }

                character.MasterManager.UpdateApprenticeInfo(this.Owner.Info, 1);
                apprenticeInfo.Status = 1;
            }
            return apprenticeInfo;
        }



        public NApprenticeInfo GetApprenticeInfo(int apprenticeId)
        {
            foreach (var f in this.apprentices)
            {
                if (f.apprenticeInfo.Id == apprenticeId)
                {
                    return f;
                }

            }
            return null;
        }
        public void GetApprenticeInfos(List<NApprenticeInfo> list)
        {
            foreach (var apprentice in this.apprentices)
            {
                list.Add(apprentice);
            }
        }

        public void UpdateApprenticeInfo(NCharacterInfo apprenticeInfo, int status)
        {
            foreach (var f in this.apprentices)
            {
                if (f.apprenticeInfo.Id == apprenticeInfo.Id)
                {
                    f.Status = status;
                    break;
                }
            }
            this.ApprenticeChanged = true;
        }
        public void UpdateMasterInfo(NCharacterInfo MasterInfo, int status)
        {
            if (this.master == null) return;
            this.master.Status = status;
            this.masterChanged = true;
        }

        public void offlineNotify()
        {

            if(this.master!=null)
            {
                var master = CharacterManager.Instance.GetCharacter(this.master.masterInfo.Id);
                if (master != null)
                {
                    master.MasterManager.UpdateApprenticeInfo(this.Owner.Info, 0);
                }
            }


            foreach (var apprenticeInfo in this.apprentices)
            {
                var apprentice = CharacterManager.Instance.GetCharacter(apprenticeInfo.apprenticeInfo.Id);
                if (apprentice != null)
                {
                    apprentice.MasterManager.UpdateMasterInfo(this.Owner.Info, 0);
                }
            }
        }
        public void PostProcess(NetMessageResponse message)
        {
            if (masterChanged)
            {
                this.InitMaster();
                if (message.masterList == null)
                {
                    message.masterList = new MasterListResponse();
                    message.masterList.Master = this.master;
                }
            }
            if (ApprenticeChanged)
            {
                this.InitApprentices();
                if (message.apprenticeList == null)
                {
                    message.apprenticeList = new ApprenticeListResponse();
                    message.apprenticeList.Apprentices.AddRange(this.apprentices);
                }
                ApprenticeChanged = false;
            }
        }

    }
}

