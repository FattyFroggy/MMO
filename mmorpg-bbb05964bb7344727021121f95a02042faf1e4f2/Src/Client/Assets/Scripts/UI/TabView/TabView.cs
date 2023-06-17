
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


    class TabView : MonoBehaviour
    {
        public UnityAction<int> OnTabSelect;
        public TabButton[] tabButtons;
        public GameObject[] tabPages;

        public int index = -1;
        IEnumerator Start()
        {
            for(int i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].tabView = this;
                tabButtons[i].tabIndex = i;
            }

            yield return new WaitForEndOfFrame();
            SelectTab(0);
        }

        public void SelectTab(int index)
        {
            if (this.index != index)
            {
                for(int i = 0; i < tabButtons.Length; i++)
                {
                    tabButtons[i].Select(i == index);
                if (this.tabPages.Length > 0)
                {
                    tabPages[i].SetActive(i == index);
                }
                    

                }
            }
        if (this.OnTabSelect != null)
        {
            this.OnTabSelect(index);
        }
        }

        private void Update()
        {
            
        }
    }


