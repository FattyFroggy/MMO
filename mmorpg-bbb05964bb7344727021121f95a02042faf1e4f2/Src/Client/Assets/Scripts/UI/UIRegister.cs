using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIRegister : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;

    public GameObject uiLogin;
    void Start()
    {
        UserService.Instance.OnRegister = this.OnRegister;
    }

    //void OnRegister(SkillBridge.Message.Result result, string msg)
    //{
    //    MessageBox.Show(string.Format("�����{0} msg:{1}", result, msg));
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("�������˺�");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("����������");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("������ȷ������");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("������������벻һ��");
            return;
        }
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }

    void OnRegister(Result result, string message)
    {
        if (result == Result.Success)
        {
            //��¼�ɹ��������ɫѡ��
            MessageBox.Show("ע��ɹ�,���¼", "��ʾ", MessageBoxType.Information).OnYes = this.CloseRegister;
        }
        else
            MessageBox.Show(message, "����", MessageBoxType.Error);
    }

    void CloseRegister()
    {
        this.gameObject.SetActive(false);
        uiLogin.SetActive(true);
    }
}
