using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : Singleton<MasterManager>
{
    public NMasterInfo master;
    public List<NApprenticeInfo> apprentices;
    public void Init(NMasterInfo master, List<NApprenticeInfo> apprentices)
    {
        this.master = master;
        this.apprentices = apprentices;
    }

}
