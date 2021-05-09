using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SliderNumber : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    void Start()
    {
        changeText();
    }
    public void changeText()
    {
        text.text = GetComponent<Slider>().value.ToString();
    }
}
