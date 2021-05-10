using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lean.Gui;
public class JobDutiesContent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public AndroidDB database;
    public Slider slider_number;
    public TMP_InputField name_task,comment;
    public TextMeshProUGUI warning_text;
    public GameObject panel_add;
    public TextMeshProUGUI text_button_create;
    public bool isChange = false;
    public void generateContent()
    {
        Utilities.deleteComponents(gameObject);
        GameObject mainobj;
        List<string> list = database.getListJobDutis();
        for(int i=0;i<list.Count;i++)
        {
        string[] str_split = list[i].Split('|');
        mainobj = Instantiate(prefab,transform);
        TextMeshProUGUI[] textmeshs = mainobj.GetComponentsInChildren<TextMeshProUGUI>();
        for(int j=0;j<str_split.Length;j++)
            textmeshs[j].text = str_split[j];
        GameObject gmj = mainobj.gameObject;
        mainobj.GetComponentsInChildren<LeanButton>()[0].OnClick.AddListener(() => removeTask(gmj));
        }
        
    }
    public void removeTask(GameObject gmj)
    {
        TextMeshProUGUI[] textmeshs = gmj.GetComponentsInChildren<TextMeshProUGUI>();
        database.deleteJobDuties(textmeshs[0].text,int.Parse(textmeshs[1].text),textmeshs[2].text);
        generateContent();
    }
    public void addnewElement()
    {
        if(name_task.text != "")
        {    
            database.insertJobDuties(name_task.text,(int)slider_number.value,comment.text);
            generateContent();
            //text_button_create.text = "Добавить должностную обязанность";
            changePanel();
            setTextNothing();
        }
        else
        {
            warning_text.text = "Введите название!";//!!
        }
    }
    public void setTextNothing()
    {
        name_task.text = "";
        slider_number.value = 1;
        comment.text = "";
    }
    public void changePanel()
    {
        if(isChange)
        {
            panel_add.SetActive(false);
            text_button_create.text = "Добавить должностную обязанность";
            isChange=false;
            
        }
        else
        {
            panel_add.SetActive(true);
            text_button_create.text = "Отмена";
            isChange=true;
        }
    }
}
