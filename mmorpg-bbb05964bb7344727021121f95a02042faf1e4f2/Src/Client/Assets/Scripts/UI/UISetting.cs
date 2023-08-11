﻿using Assets.Scripts.UI.Chat;
using Managers;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI
{
    class UISetting:UIWindow
    {
        public void ExitToCharSelect()
        {
            //UIPopCharMenu menu= UIManager.Instance.Show<UIPopCharMenu>();


            //Destroy(MinimapManager.Instance.minimap.gameObject);
            //MinimapManager.Instance.minimap = null;
            SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
            UserService.Instance.SendGameLeave();
            SceneManager.Instance.LoadScene("CharSelect");
        }
        public void SystemConfig()
        {
            UIManager.Instance.Show<UISystemConfig>();
            this.Close();
        }
        public void ExitGame()
        {
            UserService.Instance.SendGameLeave(true);
        }
    }
}
