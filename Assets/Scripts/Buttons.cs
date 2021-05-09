using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void OpenOrClose(GameObject obj) {
        if(obj.GetComponent<Animation>().isPlaying == false) {
        if(obj.activeInHierarchy == false) {
            obj.SetActive(true);
            obj.GetComponent<Animation>().clip =  obj.GetComponent<Animation>().GetClip("Open");
            obj.GetComponent<Animation>().Play();
        } else {
             obj.GetComponent<Animation>().clip =  obj.GetComponent<Animation>().GetClip("Close");
             obj.GetComponent<Animation>().Play();
            StartCoroutine(Helper(obj));
        }
    }
    }
    IEnumerator Helper(GameObject obj) {
        while(obj.GetComponent<Animation>().isPlaying) {
            yield return new WaitForSeconds(0.1f);
        }
        obj.SetActive(false);
    }
}
