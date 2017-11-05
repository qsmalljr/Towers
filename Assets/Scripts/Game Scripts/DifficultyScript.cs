using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour {

    public bool friendlyHitEdge;

    //UI stuff
    public Text levelTextBox;
    public GameObject upgradePanel;
    public GameObject gameUI;
    public bool effectMuted;


    private UnitSpawn unitSpawn;
    private Queue<GameObject> enemySpawnQueue;
    private Queue<int> numToSpawn;
    private Queue<int> delayTimes;

    private int nextLevel;
    private GameObject[] leftOverUnits;
    private GameObject[] enemyUnits;

    void Start()
    {
        unitSpawn = gameObject.GetComponent<UnitSpawn>();
        enemySpawnQueue = new Queue<GameObject>();
        numToSpawn = new Queue<int>();
        delayTimes = new Queue<int>();
        nextLevel = 1;
        friendlyHitEdge = false;

        //playLevelMain();
    }

    private void manaTicker()
    {
        unitSpawn.mana += unitSpawn.manaIncreasePerQuarterSecond;
        unitSpawn.manaBox.text = "Mana: " + unitSpawn.mana;
    }

    public void toggleMuteEffects()
    {
        effectMuted = !effectMuted;
    }

    //main method, checks player prefs for current level and plays that
    public void playLevelMain()
    {
        //unpause if paused
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        //update UI
        levelTextBox.text = "Level " + nextLevel;
        //update stats
        unitSpawn.updateStats();

        //start mana ticker
        InvokeRepeating("manaTicker", 0.5f, 0.25f);

        //give the player a coin to upgrade every 3 levels
        if (nextLevel % 2 == 0)
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);

        if (nextLevel == 1)
            levelOne();
        else if (nextLevel == 2)
            levelTwo();
        else if (nextLevel == 3)
            levelThree();
        else if (nextLevel == 4)
            levelFour();
        else if (nextLevel == 5)
            levelFive();

    }

    private void Update()
    {
        if (enemySpawnQueue.Count != 0 && !unitSpawn.spawningEnemy)
        {

            GameObject unitToSpawn = enemySpawnQueue.Dequeue();
            int n = numToSpawn.Dequeue();
            int delay = 0;
            if (delayTimes.Count != 0)
                delay = delayTimes.Dequeue();

            if (unitToSpawn.Equals(unitSpawn.basicMeleeEnemy))
            {
                StartCoroutine(unitSpawn.spawnUnit(unitToSpawn, n, false, delay));
            }
            else if (unitToSpawn.Equals(unitSpawn.basicRangedEnemy))
            {
                StartCoroutine(unitSpawn.spawnUnit(unitToSpawn, n, false, delay));
            }
            else if (unitToSpawn.Equals(unitSpawn.meleeEnemyTwo))
            {
                StartCoroutine(unitSpawn.spawnUnit(unitToSpawn, n, false, delay));
            }
        }
        //if the enemy queue is empty and a friendly unit hits edge of screen, go to next level
        //enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemySpawnQueue.Count == 0 && friendlyHitEdge)
        {
            //increment level on
            nextLevel += 1;
            //reset the end game criteria
            friendlyHitEdge = false;
            //clear the queues
            enemySpawnQueue.Clear();
            numToSpawn.Clear();
            delayTimes.Clear();

            //stop mana ticker
            CancelInvoke();

            //clear all active friendlies left alive
            leftOverUnits = GameObject.FindGameObjectsWithTag("Friendly");
            foreach(GameObject unit in leftOverUnits)
            {
                Destroy(unit);
            }

            //bring up upgrade's panel
            //playLevelMain();
            gameUI.SetActive(false);
            upgradePanel.SetActive(true);
        }
    }



    //<----------------ALL OF THE METHODS FOR LEVELS------------------------>
    private void levelOne()
    {
        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(5);
        delayTimes.Enqueue(3);

    }

    private void levelTwo()
    {
        unitSpawn.mana = 400;

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(3);
        delayTimes.Enqueue(3);
    }

    private void levelThree()
    {
        unitSpawn.mana = 500;

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);

        enemySpawnQueue.Enqueue(unitSpawn.basicRangedEnemy);
        numToSpawn.Enqueue(3);
        delayTimes.Enqueue(3);

    }

    private void levelFour()
    {
        unitSpawn.mana = 600;

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);

        enemySpawnQueue.Enqueue(unitSpawn.basicRangedEnemy);
        numToSpawn.Enqueue(3);
        delayTimes.Enqueue(3);
    }

    private void levelFive()
    {
        unitSpawn.mana = 600;

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);

        enemySpawnQueue.Enqueue(unitSpawn.basicMeleeEnemy);
        numToSpawn.Enqueue(4);
        delayTimes.Enqueue(3);
    }

    private void levelSix()
    {

    }

}
