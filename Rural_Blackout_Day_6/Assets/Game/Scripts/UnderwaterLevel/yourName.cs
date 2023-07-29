using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class yourName : MonoBehaviour
{
	[SerializeField] TMP_Text helpText;

    // Start is called before the first frame update
    void Start()
    {
        //we use this neat little trick to get the player name, it gets the user name of the profile of the computer hopefully the player as put there name as there username(I have tested this in 3 computers and all of them have their user name as their irl name)
        helpText.text = "Hello, " + System.Environment.UserName + ". Long time since we met? Your inner demons want to play.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
