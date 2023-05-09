using Assets.Scripts.UI.Chat;
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
         
            SceneManager.Instance.LoadScene("CharSelect");
            //
            SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
            UserService.Instance.SendGameLeave();
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
