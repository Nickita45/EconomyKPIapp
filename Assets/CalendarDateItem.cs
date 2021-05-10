using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class CalendarDateItem : MonoBehaviour {

    public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        
    }
}
