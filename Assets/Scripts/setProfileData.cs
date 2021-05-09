using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class setProfileData : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI user_name;
    public GameObject[] inputsfield;
    public AndroidDB database;
    public void setData(string[] array)
    {   
        user_name.text=array[0];
        for(int i=0;i<inputsfield.Length;i++)
        {
            if(i==0)
                inputsfield[i].GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = array[0];
            else
                inputsfield[i].GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = array[i+2];
        }

    }
    public void updateData()
    {
        string[] sentData = new string[inputsfield.Length]; 
        for(int i=0;i<inputsfield.Length;i++)
        {
            if(inputsfield[i].GetComponent<TMP_InputField>().text == "")
            sentData[i] = inputsfield[i].GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text;
            else
            sentData[i] = inputsfield[i].GetComponent<TMP_InputField>().text;
        }
        for(int i=0;i<inputsfield.Length;i++)
        {
            print(sentData[i]);
        }
        database.updateUserData(sentData[1],sentData[2],sentData[3],sentData[0],sentData[4]);
        string[] information = database.checkLogin(sentData[0]);
        setData(information);

    }

    
}
