using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockLevel : MonoBehaviour
{
    public string levelName; //add your level name 

    void Start()
    {
        PlayerPrefs.SetInt(levelName, 1);
    }


}
