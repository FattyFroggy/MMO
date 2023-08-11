using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class MasterService :Singleton<MasterService>
    {
        List<MasterAddRequest> masterAddRequest = new List<MasterAddRequest>();
        public MasterService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.MasterAddRequest>(this.OnMasterAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.MasterAddResponse>(this.OnMasterAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.MasterRemoveRequest>(this.OnMasterRemove);

            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.ApprenticeAddRequest>(this.OnApprenticeAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.ApprenticeAddResponse>(this.OnApprenticeAddReponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.ApprenticeRemoveRequest>(this.OnApprenticeRemove);
        }
        public void Init()
        {

        }
        private void OnMasterAddRequest(NetConnection<NetSession> sender, MasterAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.Info("OnMasterAddRequest");
            if (request.ToId == 0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name == request.ToName)
                    {
                        request.ToId = cha.Key;
                        break;
                    }
                }
            }
            NetConnection<NetSession> master = null;
            if (request.ToId > 0)
            {
                if (character.MasterManager.GetMasterInfo() != null)
                {
                    sender.Session.Response.masterAddRes = new MasterAddResponse();
                    sender.Session.Response.masterAddRes.Result = Result.Failed;
                    sender.Session.Response.masterAddRes.Errormsg = "您已经有师父!";
                    sender.SendResponse();
                    return;
                }
                master = SessionManager.Instance.GetSession(request.ToId);
            }
            if (master == null)
            {
                sender.Session.Response.masterAddRes = new MasterAddResponse();
                sender.Session.Response.masterAddRes.Result = Result.Failed;
                sender.Session.Response.masterAddRes.Errormsg = "不存在或离线!";
                sender.SendResponse();
                return;
            }
            master.Session.Response.masterAddReq = request;
            master.SendResponse();

        }

        private void OnMasterAddResponse(NetConnection<NetSession> sender, MasterAddResponse response)
        {
            Character character = sender.Session.Character;//师父
            Log.Info("OnMasterAddResponse");
            if (response.Result== Result.Success)
            {
                var apprentice = SessionManager.Instance.GetSession(response.Request.FromId);//徒弟session
                if (apprentice == null)
                {
                    sender.Session.Response.masterAddRes.Result = Result.Failed;
                    sender.Session.Response.masterAddRes.Errormsg = "对方不存在或者离线";
                }
                else
                {
                    character.MasterManager.AddApprentice(apprentice.Session.Character);
                    apprentice.Session.Character.MasterManager.AddMaster(character);
                    DBService.Instance.Save();
                    apprentice.Session.Response.masterAddRes = response;
                    apprentice.Session.Response.masterAddRes.Result = Result.Failed;
                    apprentice.Session.Response.masterAddRes.Errormsg = "拜师成功!";
                    apprentice.SendResponse();
                }
                
            }
            sender.Session.Response.masterAddRes = response;
            sender.Session.Response.masterAddRes.Result = Result.Success;
            sender.Session.Response.masterAddRes.Errormsg = "收了徒弟一枚!";
            sender.SendResponse();
        }

        private void OnMasterRemove(NetConnection<NetSession> sender, MasterRemoveRequest request)
        {
            Character character = sender.Session.Character;//徒弟
            Log.InfoFormat("OnMasterRemove");
            sender.Session.Response.masterRemove = new MasterRemoveResponse();
            sender.Session.Response.masterRemove.Id = request.Id;

            if (character.MasterManager.RemoveMaster())
            {
                sender.Session.Response.masterRemove.Result = Result.Success;
                //删除别人好友中的自己
                var master = SessionManager.Instance.GetSession(request.masterId);
                if (master != null)
                {//在线
                    master.Session.Character.MasterManager.RemoveAppenticeByID(character.Id);
                }
                else
                {//不在线
                    this.RemoveMaster(request.masterId, character.Id);
                }
            }
            else
            {
                sender.Session.Response.masterRemove.Result = Result.Failed;
            }
            DBService.Instance.Save();
            //sender.SendResponse();
        }

        private void OnApprenticeAddRequest(NetConnection<NetSession> sender, ApprenticeAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.Info("OnApprenticeAddRequest");
            if (request.ToId == 0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name == request.ToName)
                    {
                        request.ToId = cha.Key;
                        break;
                    }
                }
            }
            NetConnection<NetSession> apprentice = null;
            if (request.ToId > 0)
            {
                if (character.MasterManager.GetApprenticeInfo(request.ToId) != null)
                {
                    sender.Session.Response.apprenticeAddRes = new ApprenticeAddResponse();
                    sender.Session.Response.apprenticeAddRes.Result = Result.Failed;
                    sender.Session.Response.apprenticeAddRes.Errormsg = "对方已经是您的徒弟!";
                    sender.SendResponse();
                    return;
                }
                apprentice = SessionManager.Instance.GetSession(request.ToId);
            }
            if (apprentice == null)
            {
                sender.Session.Response.apprenticeAddRes = new ApprenticeAddResponse();
                sender.Session.Response.apprenticeAddRes.Result = Result.Failed;
                sender.Session.Response.apprenticeAddRes.Errormsg = "不存在或离线!";
                sender.SendResponse();
                return;
            }
            Character ToCharacter = CharacterManager.Instance.GetCharacter(request.ToId);
            if (ToCharacter != null)
            {
                if (ToCharacter.MasterManager.GetMasterInfo() != null)
                {
                    sender.Session.Response.apprenticeAddRes = new ApprenticeAddResponse();
                    sender.Session.Response.apprenticeAddRes.Result = Result.Failed;
                    sender.Session.Response.apprenticeAddRes.Errormsg = "对方已有师父!";
                    sender.SendResponse();
                    return;
                }
            }
            apprentice.Session.Response.apprenticeAddReq = request;
            apprentice.SendResponse();
        }

        private void OnApprenticeAddReponse(NetConnection<NetSession> sender, ApprenticeAddResponse response)
        {
            Character character = sender.Session.Character;//徒弟
            Log.Info("OnMasterAddResponse");
            if (response.Result == Result.Success)
            {
                var master = SessionManager.Instance.GetSession(response.Request.FromId);//师父session
                if (master == null)
                {
                    sender.Session.Response.apprenticeAddRes.Result = Result.Failed;
                    sender.Session.Response.apprenticeAddRes.Errormsg = "对方不存在或者离线";
                }
                else
                {
                    character.MasterManager.AddMaster(master.Session.Character);
                    master.Session.Character.MasterManager.AddApprentice(character);
                    DBService.Instance.Save();
                    master.Session.Response.apprenticeAddRes = response;
                    master.Session.Response.apprenticeAddRes.Result = Result.Failed;
                    master.Session.Response.apprenticeAddRes.Errormsg = "收徒成功!";
                    master.SendResponse();
                }

            }
            sender.Session.Response.apprenticeAddRes = response;
            sender.Session.Response.apprenticeAddRes.Result = Result.Success;
            sender.Session.Response.apprenticeAddRes.Errormsg = "获得一个便宜师父!";
            sender.SendResponse();
        }

        private void OnApprenticeRemove(NetConnection<NetSession> sender, ApprenticeRemoveRequest request)
        {
            Character character = sender.Session.Character;//师父
            Log.InfoFormat("OnApprenticeRemove");
            sender.Session.Response.apprenticeRemove = new ApprenticeRemoveResponse();
            sender.Session.Response.apprenticeRemove.Id = request.Id;

            if (character.MasterManager.RemoveAppenticeByID(request.apprenticeId))
            {
                sender.Session.Response.apprenticeRemove.Result = Result.Success;
                //删除徒弟中的自己
                var apprentice = SessionManager.Instance.GetSession(request.apprenticeId);
                if (apprentice != null)
                {//在线
                    apprentice.Session.Character.MasterManager.RemoveMaster();
                }
                else
                {//不在线
                    this.RemoveApprentice(request.apprenticeId, character.Id);
                }
            }
            else
            {
                sender.Session.Response.apprenticeRemove.Result = Result.Failed;
            }
            DBService.Instance.Save();
            //sender.SendResponse();
        }

        private void RemoveMaster(int masterId, int Id)
        {
            var RemoveItem = DBService.Instance.Entities.TCharacterMasters.FirstOrDefault(V => V.MasterId == masterId && V.CharacterID == Id);
            if (RemoveItem != null)
            {
                DBService.Instance.Entities.TCharacterMasters.Remove(RemoveItem);
            }
        }
        private void RemoveApprentice(int ApprenticeId, int Id)
        {
            var RemoveItem = DBService.Instance.Entities.TCharacterApprentices.FirstOrDefault(V => V.ApprenticeId == ApprenticeId && V.CharacterID == Id);
            if (RemoveItem != null)
            {
                DBService.Instance.Entities.TCharacterApprentices.Remove(RemoveItem);
            }


        }
    }
}
