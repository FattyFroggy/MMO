﻿using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Ride
{
    class UIRideItem:ListView.ListViewItem
    {
        public Text title;
        public Image icon;
        public Text level;

        public Image background;
        public Sprite normalBg;
        public Sprite selectedBg;

        public override void onSelected(bool selected)
        {
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
        public Item item;
        private void Start()
        {

        }

        public void SetEquipItem(Item item,UIRide owner,bool equiped)
        {
            this.item = item;

            if (this.title != null) this.title.text = this.item.Define.Name;
            if (this.level != null) this.level.text = this.item.Define.Level.ToString();
            if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon); 

        }

    }
}
