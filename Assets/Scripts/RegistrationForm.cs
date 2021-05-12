using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RegistrationForm : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TMP_InputField login;
    [SerializeField]
    private TMP_InputField password;
    [SerializeField]
    private TMP_InputField confirmPassword;
    [SerializeField]
    private TMP_InputField firstName,secondName,email;
    public GameObject registration, authorization;

    public AndroidDB database;
    public void onTryRegistr()
    {
        if (password.text == confirmPassword.text)
        {
            database.insertUser(login.text,firstName.text,secondName.text,email.text,password.text);
            authorization.SetActive(true);
            registration.SetActive(false);
            //PlayerPrefs.SetString("test","");//!!!!
        }
        else
        {
            Debug.Log("Wrong password");
        }
    }
}
