using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onlyAppearIfCompletedLevel1 : MonoBehaviour
{
    //in unlock level script there is a player prefs if that player prefs is true then we show this button(make sure the names of the player prefs are the same or it ight think your working on a new level)

    public string levelName; //add your level name

    public GameObject button;
    public GameObject Button2;

    void Start()
    {
        if (PlayerPrefs.GetInt(levelName) == 1)
        {
            button.SetActive(true);
        }

        if (PlayerPrefs.GetInt(levelName) == 1)
        {
            Button2.SetActive(true);
        }
    }

}
