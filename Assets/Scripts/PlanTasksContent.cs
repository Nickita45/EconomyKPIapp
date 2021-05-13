using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Lean.Gui;
using UnityEngine.UI;
public class PlanTasksContent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public AndroidDB database;
    bool isChange = false;
    bool isFinishing = false;
    public GameObject panel_add;
    public GameObject _calendarPanel;
    public TextMeshProUGUI text_button_create;
    public TextMeshProUGUI[] texts;
    public TextMeshProUGUI text_warning;
    public TextMeshProUGUI text_warning_for_finish;
    public TMP_InputField input_number;
    public GameObject _panel_finish_task;
    string name_task;
    int count_task;
   // string finish_date;
    public GameObject _modal_window_delete;
    public void generateContent()
    {
        Utilities.deleteComponents(gameObject);
        GameObject mainobj;
        List<string> list = database.getListTasks();
        for(int i=0;i<list.Count;i++)
        {
            string[] str_split = list[i].Split('|');
            mainobj = Instantiate(prefab,transform);
            TextMeshProUGUI[] textmeshs = mainobj.GetComponentsInChildren<TextMeshProUGUI>();
            for(int j=0;j<str_split.Length-1;j++)
                textmeshs[j].text = str_split[j];
            /*if(str_split.Length !=5 )
                textmeshs[4].text = "";*/
            mainobj.name="Plantask"+str_split[5];
            GameObject gmj = mainobj.gameObject;
            mainobj.GetComponentsInChildren<LeanButton>()[0].OnClick.AddListener(() => removeTask(gmj));
            mainobj.GetComponentsInChildren<LeanButton>()[1].OnClick.AddListener(() => openFinishTaskPanel(gmj));
        }

    }
    public void removeTask(GameObject gmj)
    {

        _modal_window_delete.GetComponent<LeanWindow>().TurnOn();
        _modal_window_delete.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
        _modal_window_delete.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
        TextMeshProUGUI[] textmeshs = gmj.GetComponentsInChildren<TextMeshProUGUI>();
        int id = int.Parse(gmj.name.Replace("Plantask",""));
        _modal_window_delete.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => removeTaskSure(id));
        _modal_window_delete.GetComponentsInChildren<Button>()[1].onClick.AddListener(_modal_window_delete.GetComponent<LeanWindow>().TurnOff);
    
    }
    public void removeTaskSure(int id)
    {
        _modal_window_delete.GetComponent<LeanWindow>().TurnOff();
        database.deleteTask(id);
        generateContent();
    }
    public void openFinishTaskPanel(GameObject gmj)
    {
        _panel_finish_task.GetComponentsInChildren<TextMeshProUGUI>()[0].text="Вы точно хотите завершить '"+ gmj.GetComponentsInChildren<TextMeshProUGUI>()[0].text+"'?";
        name_task = gmj.GetComponentsInChildren<TextMeshProUGUI>()[0].text;
        count_task = int.Parse(gmj.GetComponentsInChildren<TextMeshProUGUI>()[1].text);
        //finish_date = gmj.GetComponentsInChildren<TextMeshProUGUI>()[3].text;
        changePanelFinish();

    }
    
    public void changePanelFinish()
    {
        if(!isFinishing)
        {
            _panel_finish_task.SetActive(true);
            text_button_create.text = "Отмена";
            isFinishing=true;
            isChange = true;
        }
    }
    public void changePanel()
    {
        if(isChange)
        {
            panel_add.SetActive(false);
            text_button_create.text = "Добавить новую задачу";
            isChange=false;
            _calendarPanel.SetActive(false);

            _panel_finish_task.SetActive(false);
            isFinishing=false;
            
        }
        else
        {
            panel_add.SetActive(true);
            text_button_create.text = "Отмена";
            isChange=true;
            
        }
    }
    
    public void addnewTask()
    {   
        
        if(texts[2].text == "Указать дату" || texts[3].text == "Указать дату")
        {    
            text_warning.text = "Укажите дату";
            return ;
        }
        
        if(texts[0].text == "​")
        {
            text_warning.text = "Укажите название";
            return ;
        }
        string[] split_1 = texts[2].text.Split('.');
        string[] split_2 = texts[3].text.Split('.');
        int flag = DateTime.Compare(new DateTime(int.Parse(split_1[2]),int.Parse(split_1[1]),int.Parse(split_1[0])), 
                        new DateTime(int.Parse(split_2[2]),int.Parse(split_2[1]),int.Parse(split_2[0])));
        if(flag == 1)
        {
           text_warning.text = "Дата начала не может быть больше даты конца";
        }
        else
        {  
            string str = input_number.text;
            int number;
            if(!int.TryParse(str,out number))
            {    
                text_warning.text = "Не правильный формат длительности в часах";
                return;
            }//int kek = int.Parse(str);
            database.insertTask(texts[0].text,number,
                                texts[2].text,texts[3].text,texts[4].text);
            generateContent();
            changePanel();
            setNothingText();
        }
        //database.insertTask();
    }
    public void setNothingText()
    {
        texts[0].text = ""; 
        input_number.text = "";
        texts[2].text= "Указать дату";
        texts[3].text = "Указать дату";
        texts[4].text = "";
        text_warning.text = "";

        text_warning_for_finish.text="";
        _panel_finish_task.GetComponentsInChildren<TextMeshProUGUI>()[4].text = "Указать дату";
        _panel_finish_task.GetComponentsInChildren<TMP_InputField>()[0].text = "";

    }
    public void finishTask()
    {
        int number;
        if(!int.TryParse(_panel_finish_task.GetComponentsInChildren<TMP_InputField>()[0].text,out number))
        {    
            text_warning_for_finish.text = "Не правильный формат длительности в часах";
            return;    
        }
        if(_panel_finish_task.GetComponentsInChildren<TextMeshProUGUI>()[4].text == "Указать дату")
        {
            text_warning_for_finish.text = "Укажите дату";
            return ;
        }
        //!!! НЕ МОЖЕТ ЗАКОНЧИТСЯ РАНЬШЕ НАЧАТОГО НАДО УСЛОВИЕ
        database.updatePlanTask(name_task,count_task,number,_panel_finish_task.GetComponentsInChildren<TextMeshProUGUI>()[4].text,_panel_finish_task.GetComponentsInChildren<TMP_InputField>()[1].text);
        generateContent();
        changePanel();

    }
    
}
