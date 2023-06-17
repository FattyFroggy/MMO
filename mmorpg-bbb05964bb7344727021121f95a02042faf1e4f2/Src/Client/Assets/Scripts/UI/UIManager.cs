using Assets.Scripts.UI;
using Assets.Scripts.UI.Chat;
using Assets.Scripts.UI.Guild;
using Assets.Scripts.UI.Ride;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // Start is called before the first frame updat 
    class UIElement
    {
        public string Resource;
        public bool cache;
        public GameObject Instance;
    }

    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

    public UIManager()
    {
        this.UIResources.Add(typeof(UISetting), new UIElement() { Resource = "UI/UISetting", cache = true });
        this.UIResources.Add(typeof(UITest), new UIElement() { Resource = "UI/UITest", cache = true });
        this.UIResources.Add(typeof(UIBag), new UIElement() { Resource = "UI/Bag/UIBag", cache = false });
        this.UIResources.Add(typeof(UIShop), new UIElement() { Resource = "UI/Shop/UIShop", cache = false });
        this.UIResources.Add(typeof(UICharEquip), new UIElement() { Resource = "UI/Equip/UIEquip", cache = false });
        this.UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resource = "UI/Quest/UIQuestSystem", cache = false });
        this.UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resource = "UI/Quest/UIQuestDialog", cache = false });
        this.UIResources.Add(typeof(UIFriend), new UIElement() { Resource = "UI/Friend/UIFriend", cache = false });
        this.UIResources.Add(typeof(UIFriendItem), new UIElement() { Resource = "UI/Friend/UIFriendItem", cache = false });
        this.UIResources.Add(typeof(UIGuild), new UIElement() { Resource = "UI/Guild/UIGuild", cache = false });
        this.UIResources.Add(typeof(UIGuildList), new UIElement() { Resource = "UI/Guild/UIGuildList", cache = false });
        this.UIResources.Add(typeof(UIGuildPopNoGuild), new UIElement() { Resource = "UI/Guild/UIGuildPopNoGuild", cache = false });
        this.UIResources.Add(typeof(UIGuildPopCreate), new UIElement() { Resource = "UI/Guild/UIGuildPopCreate", cache = false });
        this.UIResources.Add(typeof(UIGuildApplyList), new UIElement() { Resource = "UI/Guild/UIGuildApplyList", cache = false });
        this.UIResources.Add(typeof(UIPopCharMenu), new UIElement() { Resource = "UI/UIPopCharMenu", cache = false });
        this.UIResources.Add(typeof(UISystemConfig), new UIElement() { Resource = "UI/UISystemConfig", cache = false });
        this.UIResources.Add(typeof(UIRide), new UIElement() { Resource = "UI/Ride/UIRide", cache = false });
    }

    ~UIManager()
    {

    }




    public T Show<T>()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Open);
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resource);
                if (prefab == null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }

            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public void Close(Type type)
    {
        //Debug.LogWarning("UIClose");
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
