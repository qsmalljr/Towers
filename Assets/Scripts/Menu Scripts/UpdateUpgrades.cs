using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpdateUpgrades : MonoBehaviour {

    public Text coins;

    //intiliazing all text
    public Text basicMeleeDamage;
    public Text basicMeleeHealth;

    public Text meleeTwoDamage;
    public Text meleeTwoHealth;

    public Text basicRangedDamage;
    public Text basicRangedHealth;

	// Update is called once per frame
	void Update () {
        coins.text = "Upgrades: " + PlayerPrefs.GetInt("Coins");

        basicMeleeDamage.text = PlayerPrefs.GetFloat("BasicMeleeDamage").ToString();
        basicMeleeHealth.text = PlayerPrefs.GetFloat("BasicMeleeHealth").ToString();

        meleeTwoDamage.text = PlayerPrefs.GetFloat("MeleeTwoDamage").ToString();
        meleeTwoHealth.text = PlayerPrefs.GetFloat("MeleeTwoHealth").ToString();

        basicRangedDamage.text = PlayerPrefs.GetFloat("BasicRangedDamage").ToString();
        basicRangedHealth.text = PlayerPrefs.GetFloat("BasicRangedHealth").ToString();
    }

    
}
