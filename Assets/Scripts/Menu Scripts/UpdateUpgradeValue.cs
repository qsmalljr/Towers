using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUpgradeValue : MonoBehaviour {

    public Text text;

    public void Start()
    {
        
    }
    public void updateText(string playerPref)
    {
        text.text = PlayerPrefs.GetInt(playerPref).ToString();
    }

    
}
