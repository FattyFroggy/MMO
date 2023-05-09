﻿using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Equip
{

    class UIEquipItem: MonoBehaviour,IPointerClickHandler
    {
        public Image icon;
        public Text title;
        public Text level;
        public Text litmitClass;
        public Text litmitCategory;

        public Image background;
        public Sprite normalBg;
        public Sprite selectedBg;

        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
        }

        public int index;
        private UICharEquip owner;
        private Item item;
        bool isEquiped = false;
        internal void SetEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
        {
            this.owner = owner;
            this.index = idx;
            this.item = item;
            this.isEquiped = equiped;

            if (this.title != null) this.title.text = this.item.Define.Name;
            if (this.level != null) this.level.text = this.item.Define.Level.ToString();
            if (this.litmitClass != null) this.litmitClass.text = this.item.Define.LimitClass.ToString();
            if (this.litmitCategory != null) this.litmitCategory.text = this.item.Define.Category;
            if (this.icon != null) this.icon.overrideSprite =Resloader.Load<Sprite>(this.item.Define.Icon);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.isEquiped)
            {
                UnEquip();
            }
            else
            {
                if (this.selected)
                {
                    DoEquip();
                    this.selected = false;

                }
                else
                {
                    this.selected = true;
                }
            }
        }

        private void DoEquip()
        {
            var msg = MessageBox.Show(string.Format("要装备[{0}]吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
            msg.OnYes = () =>
            {
                var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
                if (oldEquip != null)
                {
                    var newmsg = MessageBox.Show(string.Format("要替换[{0}]吗?", oldEquip.Define.Name), "确认", MessageBoxType.Confirm);
                    newmsg.OnYes = () =>
                    {
                        this.owner.DoEquip(this.item);
                    };
                }
                else
                    this.owner.DoEquip(this.item);
            };
        }

        private void UnEquip()
        {
            var msg = MessageBox.Show(string.Format("要取下装备[{0}]吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
            msg.OnYes = () =>
            {
                this.owner.UnEquip(this.item);
            };
        }
    }
}
