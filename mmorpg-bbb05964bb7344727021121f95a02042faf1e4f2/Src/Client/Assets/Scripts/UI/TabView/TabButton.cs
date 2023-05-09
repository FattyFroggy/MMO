using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


    class TabButton:MonoBehaviour
    {
        public Sprite activeImage;
        public Sprite normalImage;

        public TabView tabView;

        public int tabIndex = 0;
        public bool selected = false;

        public Image tabImage;

        private void Start()
        {
            tabImage = this.GetComponent<Image>();
            normalImage = tabImage.sprite;
            this.GetComponent<Button>().onClick.AddListener(OnClick);
        }
        public void Select(bool select)
        {
            tabImage.overrideSprite = select ? activeImage : normalImage;
        }
        private void OnClick()
        {
            this.tabView.SelectTab(tabIndex);
        }
    }

