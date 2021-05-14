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
    public GameObject _modal_window_delete;
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
        for(int j=0;j<str_split.Length-1;j++)
            textmeshs[j].text = str_split[j];
        //textmeshs[0].text = "Название:"+str_split[0];
        //textmeshs[1].text = "Критерий:"+str_split[1];
        //textmeshs[2].text = "Комментарий:"+str_split[2];
        mainobj.name = "Job"+str_split[3];
        GameObject gmj = mainobj.gameObject;
        mainobj.GetComponentsInChildren<LeanButton>()[0].OnClick.AddListener(() => removeTask(gmj));
        }
        
    }
    public void removeTask(GameObject gmj)
    {
        _modal_window_delete.GetComponent<LeanWindow>().TurnOn();
        _modal_window_delete.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
        _modal_window_delete.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
        TextMeshProUGUI[] textmeshs = gmj.GetComponentsInChildren<TextMeshProUGUI>();
        int id = int.Parse(gmj.name.Replace("Job",""));
        _modal_window_delete.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => removeTaskSure(id));
        _modal_window_delete.GetComponentsInChildren<Button>()[1].onClick.AddListener(_modal_window_delete.GetComponent<LeanWindow>().TurnOff);
        
        
        //database.deleteJobDuties(id);
        //generateContent();
    }
    public void removeTaskSure(int id)
    {
        /*TextMeshProUGUI[] textmeshs = gmj.GetComponentsInChildren<TextMeshProUGUI>();
        int id = int.Parse(gmj.name.Replace("Job",""));*/
        _modal_window_delete.GetComponent<LeanWindow>().TurnOff();
        database.deleteJobDuties(id);
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
