using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MasterService : Singleton<MasterService>, IDisposable
{
    public UnityAction OnMasterUpdate { get; internal set; }

    public MasterService()
    {
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MasterAddRequest>(this.OnMasterAddRequest);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MasterAddResponse>(this.OnMasterAddResponse);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MasterListResponse>(this.OnMasterList);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.MasterRemoveResponse>(this.OnMasterRemove);

        MessageDistributer.Instance.Subscribe<SkillBridge.Message.ApprenticeAddRequest>(this.OnApprenticeAddRequest);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.ApprenticeAddResponse>(this.OnApprenticeAddResponse);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.ApprenticeListResponse>(this.OnApprenticeList);
        MessageDistributer.Instance.Subscribe<SkillBridge.Message.ApprenticeRemoveResponse>(this.OnApprenticeRemove);
    }
    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MasterAddRequest>(this.OnMasterAddRequest);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MasterAddResponse>(this.OnMasterAddResponse);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MasterListResponse>(this.OnMasterList);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MasterRemoveResponse>(this.OnMasterRemove);

        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.ApprenticeAddRequest>(this.OnApprenticeAddRequest);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.ApprenticeAddResponse>(this.OnApprenticeAddResponse);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.ApprenticeListResponse>(this.OnApprenticeList);
        MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.ApprenticeRemoveResponse>(this.OnApprenticeRemove);
    }



    public void Init()
    {

    }
    private void OnMasterAddRequest(object sender, MasterAddRequest request)
    {
        Debug.Log("OnMasterAddRequest");
        var confirm = MessageBox.Show(string.Format("{0}������Ӱ���Ϊʦ", request.FromName), "��ʦ����", MessageBoxType.Confirm, "����", "�ܾ�");
        confirm.OnYes = () =>
        {
            this.SendMasterAddResponse(true, request);
        };
        confirm.OnNo = () =>
        {
            this.SendMasterAddResponse(false, request);
        };
    }



    private void OnMasterAddResponse(object sender, MasterAddResponse message)
    {
        Debug.Log("OnMasterAddResponse");
        if (message.Result == Result.Success)
        {
            MessageBox.Show(message.Request.ToName + "�����˰�ʦ����", "��ʦ�ɹ�");

        }
        else
        {
            MessageBox.Show(message.Errormsg, "��ʦʧ��");
        }
    }

    private void OnMasterList(object sender, MasterListResponse message)
    {
        Debug.Log("OnMasterList");
        MasterManager.Instance.master = message.Master;
        if (this.OnMasterUpdate != null)
        {
            this.OnMasterUpdate();
        }
    }

    private void OnMasterRemove(object sender, MasterRemoveResponse message)
    {
        throw new NotImplementedException();
    }

    private void OnApprenticeAddRequest(object sender, ApprenticeAddRequest request)
    {
        var confirm = MessageBox.Show(string.Format("{0}�����������Ϊͽ", request.FromName), "��ͽ����", MessageBoxType.Confirm, "����", "�ܾ�");
        confirm.OnYes = () =>
        {
            this.SendApprenticeAddResponse(true, request);
        };
        confirm.OnNo = () =>
        {
            this.SendApprenticeAddResponse(false, request);
        };
    }

    private void OnApprenticeAddResponse(object sender, ApprenticeAddResponse message)
    {
        if (message.Result == Result.Success)
        {
            MessageBox.Show(message.Request.ToName + "�����������ͽ����", "��ͽ�ɹ�");

        }
        else
        {
            MessageBox.Show(message.Errormsg, "��ͽʧ��");
        }
    }

    private void OnApprenticeList(object sender, ApprenticeListResponse message)
    {
        Debug.Log("OnApprenticeList");
        MasterManager.Instance.apprentices = message.Apprentices;
        if (this.OnMasterUpdate != null)
        {
            this.OnMasterUpdate();
        }
    }

    private void OnApprenticeRemove(object sender, ApprenticeRemoveResponse message)
    {
        throw new NotImplementedException();
    }


    public void SendMasterRequest(int masterId,string masterName)
    {
        Debug.Log("SendMasterRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.masterAddReq = new MasterAddRequest();
        message.Request.masterAddReq.FromId = User.Instance.CurrentCharacter.Id;
        message.Request.masterAddReq.FromName = User.Instance.CurrentCharacter.Name;
        message.Request.masterAddReq.ToId = masterId;
        message.Request.masterAddReq.ToName = masterName;
        NetClient.Instance.SendMessage(message);
    }
    public void SendApprenticeRequest(int apprenticeId, string apprenticeName)
    {
        Debug.Log("SendMasterRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.apprenticeAddReq = new ApprenticeAddRequest();
        message.Request.apprenticeAddReq.FromId = User.Instance.CurrentCharacter.Id;
        message.Request.apprenticeAddReq.FromName = User.Instance.CurrentCharacter.Name;
        message.Request.apprenticeAddReq.ToId = apprenticeId;
        message.Request.apprenticeAddReq.ToName = apprenticeName;
        NetClient.Instance.SendMessage(message);
    }
    private void SendMasterAddResponse(bool accept, MasterAddRequest request)
    {
        Debug.Log("SendMasterAddResponse");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.masterAddRes = new MasterAddResponse();
        message.Request.masterAddRes.Result = accept ? Result.Success : Result.Failed;
        message.Request.masterAddRes.Errormsg= accept ? "�Է�ͬ��" : "�Է��ܾ�";
        message.Request.masterAddRes.Request = request;
        NetClient.Instance.SendMessage(message);
    }
    private void SendApprenticeAddResponse(bool accept, ApprenticeAddRequest request)
    {
        Debug.Log("SendApprenticeAddResponse");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.apprenticeAddRes = new ApprenticeAddResponse();
        message.Request.apprenticeAddRes.Result = accept ? Result.Success : Result.Failed;
        message.Request.apprenticeAddRes.Errormsg = accept ? "�Է�ͬ��" : "�Է��ܾ�";
        message.Request.apprenticeAddRes.Request = request;
        NetClient.Instance.SendMessage(message);
    }

    internal void SendMasterRemoveRequest(int id, int masterId)
    {
        Debug.Log("SendMasterRemoveRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.masterRemove = new MasterRemoveRequest();
        message.Request.masterRemove.Id = id;
        message.Request.masterRemove.masterId = masterId;
        NetClient.Instance.SendMessage(message);
    }

    internal void SendApprenticeRemoveRequest(int id, int apprenticeId)
    {
        Debug.Log("SendApprenticeRemoveRequest");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.apprenticeRemove = new ApprenticeRemoveRequest();
        message.Request.apprenticeRemove.Id = id;
        message.Request.apprenticeRemove.apprenticeId = apprenticeId;
        NetClient.Instance.SendMessage(message);
    }
}
