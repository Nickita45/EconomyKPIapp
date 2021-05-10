using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Utilities: MonoBehaviour
{
    public static void deleteComponents(GameObject gmj)
    {
        
        for(int i=gmj.transform.childCount-1;i>=0;i--)
        {
            Destroy(gmj.transform.GetChild(i).gameObject);
        }
    }
}