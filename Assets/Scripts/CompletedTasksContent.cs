using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Gui;
using UnityEngine.UI;
public class CompletedTasksContent : MonoBehaviour
{
    public GameObject prefab;
    public AndroidDB database;
    public TextMeshProUGUI _result;
    public GameObject _modal_window_delete;
    public void generateContent()
    {
        Utilities.deleteComponents(gameObject);
        GameObject mainobj;
        List<string> list = database.getListCompletedTasks();
        List<float> sum_koef = new List<float>();
        for(int i=0;i<list.Count;i++)
        {
            string[] str_split = list[i].Split('|');
            mainobj = Instantiate(prefab,transform);
            TextMeshProUGUI[] textmeshs = mainobj.GetComponentsInChildren<TextMeshProUGUI>();
            textmeshs[0].text = "Название:"+str_split[0];
            textmeshs[1].text = "Фактическое затраченное время на выполнение:" + str_split[1] + " часов";
            textmeshs[2].text = "Дата начала:" + str_split[2];
            textmeshs[3].text = "Дата завершения:" + str_split[3];
            textmeshs[4].text = str_split[4];
            mainobj.name = "Tasks"+str_split[6];

            float koef = float.Parse(str_split[1])/float.Parse(str_split[5]);
            if(koef > 1 )
                textmeshs[5].color = Color.green; 
            else if(koef < 0.7)
                textmeshs[5].color = Color.red;
            else
                textmeshs[5].color = Color.yellow;
            textmeshs[5].text = string.Format("{0:0.00}", koef);
            sum_koef.Add(koef);

            GameObject gmj = mainobj.gameObject;
            mainobj.GetComponentsInChildren<LeanButton>()[0].OnClick.AddListener(() => removeTask(gmj));
        }
        float sum = 0;
        
        for(int i=0;i<sum_koef.Count;i++)
        {
            sum+=sum_koef[i];
        }
        float effectivy = sum/(float) sum_koef.Count;
        _result.text = string.Format("{0:0.00}", effectivy);
        if(effectivy > 1)
        {
            _result.color = Color.green;
            _result.text += " - Эффективность высокая";
        }
        else if(effectivy < 1)
        {
            _result.color = Color.red;
            _result.text += " - Эффективность низкая";
        }
        else 
        _result.text += " - Эффективность стандартная";
        if(sum_koef.Count == 0)
        {
            _result.text = "-";
        }
        
    }
    public void removeTask(GameObject gmj)
    {
        _modal_window_delete.GetComponent<LeanWindow>().TurnOn();
        _modal_window_delete.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
        _modal_window_delete.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
        TextMeshProUGUI[] textmeshs = gmj.GetComponentsInChildren<TextMeshProUGUI>();
        int id = int.Parse(gmj.name.Replace("Tasks",""));
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
        database.deleteTask(id);
        generateContent();
    }
    
}
