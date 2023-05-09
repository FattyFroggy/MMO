using Common.Data;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    public void Init()
    {
        NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeShop, OnNpcInvokeShop);
        NpcManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
    }
    private bool OnNpcInvokeShop(NpcDefine npc)
    {
        Debug.LogFormat("TestManager.OnNpcInvokeSho:Npc:[{0}:{1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
        UITest test = UIManager.Instance.Show<UITest>();
        test.SetTitle(npc.Name);
        return true;
    }
    private bool OnNpcInvokeInsrance(NpcDefine npc)
    {
        Debug.LogFormat("TestManager.OnNpcInvokeInsrance:Npc:[{0}:{1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
        MessageBox.Show("点击了Npc:" + npc.Name, "NPC对话");
        return true;
    }


}
