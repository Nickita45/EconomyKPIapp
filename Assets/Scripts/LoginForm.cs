using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoginForm : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField login;
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private TMP_Text waringText;

    public AndroidDB database; //!!!
    public GameObject menu;

    //public GameObject formLogin,formMenu;
    public void checkLogin()
    {
        //string tempLogin = PlayerPrefs.GetString("login");
        //string tempPassword = PlayerPrefs.GetString("password");
        if (login.text == "" || password.text == "")
        {
           
            waringText.text = "Необходимо ввести значения во все поля!";
        }
        else
        {
            string[] information = database.checkLogin(login.text);
            if (information[0] == login.text && information[1] == password.text) 
            {
                print("Work");
                menu.GetComponent<setProfileData>().setData(information);
                menu.SetActive(true);
                gameObject.SetActive(false);
            
            }
            else
            {
                waringText.text = "Не верный логин или пароль!";
            }
        }
    }  
}
