using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour {

    public Text text;
    public bool resetPrefs = false;
    
    public void increaseDamage(string playerPref)
    {
        if (PlayerPrefs.GetInt("Coins") <= 0) { //if player has no coins, do nothing
            return;
        }
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 1); //subtract a coin
        float currentDamage = PlayerPrefs.GetFloat(playerPref);
        if (currentDamage >= 9) { //enforcing a cap to number of upgrades in damage category
            return;
        }
        float upgradedDamage = currentDamage + .5f;
        PlayerPrefs.SetFloat(playerPref, upgradedDamage);
        text.text = PlayerPrefs.GetFloat(playerPref).ToString(); //updating displayed damage
    }

    public void increaseHealth(string playerPref) {
        if (PlayerPrefs.GetInt("Coins") <= 0) //if player has no coins, do nothing
        {
            return;
        }
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 1); //subtract a coin
        float currentHealth = PlayerPrefs.GetFloat(playerPref);
        if (currentHealth >= 38) //enforcing a cap to number of upgrades in health category
        {
            return;
        }
        float upgradedHealth = currentHealth + 2;
        PlayerPrefs.SetFloat(playerPref, upgradedHealth);
        text.text = PlayerPrefs.GetFloat(playerPref).ToString(); //updating displayed health
    }

    public void Update()
    {
        if (resetPrefs == true) //used for testing purposes to reset player prefs
        {
            PlayerPrefs.DeleteAll(); 
        }
    }
}
