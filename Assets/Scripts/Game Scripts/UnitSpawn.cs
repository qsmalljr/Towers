using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawn : MonoBehaviour {

    public GameObject tower;

    //friendlies
    public GameObject basicMelee;
    public GameObject basicRanged;
    public GameObject meleeTwo;
    public GameObject meleeThree;
    public GameObject rangedTwo;

    //enemies
    public GameObject basicMeleeEnemy;
    public GameObject meleeEnemyTwo;
    public GameObject meleeEnemyThree;
    public GameObject basicRangedEnemy;
    public GameObject rangedEnemy;
    public GameObject boss;

    //UI Components
    public Text manaBox;

    public int mana = 100;
    public int manaIncreasePerQuarterSecond = 5;

    private float height;
    private float width;


    public bool spawningFriendly;
    public bool spawningEnemy;
    private Queue<GameObject> spawnQueue;
    private Queue<GameObject> spawnedQueue;
    private Queue<GameObject> spawnedQueueEnemy;
    private GameObject lastSpawned;
    private GameObject lastSpawnedEnemy;
    private GameObject[] friendlies;

    private void Start()
    {
        //get the screen height and width based on camera (located in center of page)
        Camera cam = FindObjectOfType<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        spawnQueue = new Queue<GameObject>();
        spawnedQueue = new Queue<GameObject>();
        spawnedQueueEnemy = new Queue<GameObject>();

        spawningFriendly = false;
        spawningEnemy = false;
        lastSpawned = null;

        //set default stats
        setStatsDefault();
        //update stats
        updateStats();

        //for testing
        //StartCoroutine(spawnUnit(meleeEnemyTwo, 5, false));
        //StartCoroutine(spawnUnit(basicMelee, 5, true));
    }

    private void Update()
    {
        //if the queue has an element call spawn unit
        if(spawnQueue.Count != 0 && !spawningFriendly)
        {
            GameObject unitToSpawn = spawnQueue.Dequeue();

            if (unitToSpawn.Equals(basicMelee))
            {
                StartCoroutine(spawnUnit(unitToSpawn, 1, true, 0));
            }
            else if (unitToSpawn.Equals(basicRanged))
            {
                StartCoroutine(spawnUnit(unitToSpawn, 1, true, 0));
            }
            else if (unitToSpawn.Equals(meleeTwo))
            {
                StartCoroutine(spawnUnit(unitToSpawn, 1, true, 0));
            }
        }

        if(spawnedQueue.Count != 0 && (lastSpawned == null || ((lastSpawned.transform.position.x - lastSpawned.GetComponent<BoxCollider2D>().size.x / 2f) > spawnedQueue.Peek().transform.position.x + spawnedQueue.Peek().GetComponent<BoxCollider2D>().size.x)))
        {
            //reenable the colliders, move it again, dequeue, assign last spawned
            GameObject spawnUnit = spawnedQueue.Dequeue();

            lastSpawned = spawnUnit;
            spawnUnit.GetComponent<BoxCollider2D>().enabled = true;
            if (spawnUnit.GetComponent<UnitStats>().isRanged)
                spawnUnit.GetComponent<CircleCollider2D>().enabled = true;
            spawnUnit.GetComponent<UnitStats>().isMoving = true;

        }

        if (spawnedQueueEnemy.Count != 0 && (lastSpawnedEnemy == null || ((lastSpawnedEnemy.transform.position.x + lastSpawnedEnemy.GetComponent<BoxCollider2D>().size.x / 2f) < spawnedQueueEnemy.Peek().transform.position.x - spawnedQueueEnemy.Peek().GetComponent<BoxCollider2D>().size.x)))
        {
            //reenable the colliders, move it again, dequeue, assign last spawned
            GameObject spawnUnit = spawnedQueueEnemy.Dequeue();

            lastSpawned = spawnUnit;
            spawnUnit.GetComponent<BoxCollider2D>().enabled = true;
            if (spawnUnit.GetComponent<UnitStats>().isRanged)
                spawnUnit.GetComponent<CircleCollider2D>().enabled = true;
            spawnUnit.GetComponent<UnitStats>().isMoving = true;
        }

    }


    //reusable private method for spawning a certain number of a certain unit both friendly and unfriendly
    public IEnumerator spawnUnit(GameObject unit, int n, bool isFriendly, int delayTime)
    {
        if (isFriendly)
            spawningFriendly = true;
        else
            spawningEnemy = true;

        //keep track of last spawn position here
        Vector2 pos;
        if(isFriendly)
            pos = new Vector2(-width / 2f, (-height / 2f) + ((unit.transform.lossyScale.y * unit.GetComponent<BoxCollider2D>().size.y) / 2f) + height/10f);
        else
            pos = new Vector2(width / 2f, (-height / 2f) + ((unit.transform.lossyScale.y * unit.GetComponent<BoxCollider2D>().size.y) / 2f) + height/10f);



        //disable colliders, disable movement, enqueue into the spawned queue
        unit.GetComponent<BoxCollider2D>().enabled = false;
        if (unit.GetComponent<UnitStats>().isRanged)
            unit.GetComponent<CircleCollider2D>().enabled = false;
        unit.GetComponent<UnitStats>().isMoving = false;
        

            

        //loop through all the units we must spawn minus the first one
        //separate the spawn interval constant
        for (int i = 0; i < n; i++)
        {
            if (isFriendly)
                spawnedQueue.Enqueue(Instantiate(unit, pos, Quaternion.identity));
            else
                spawnedQueueEnemy.Enqueue(Instantiate(unit, pos, Quaternion.identity));

            yield return new WaitForSeconds(1f);
        }

        //if (delayTime != 0)
        //    yield return new WaitForSeconds((float)delayTime);

        if (isFriendly)
            spawningFriendly = false;
        else
            spawningEnemy = false;
    }



    public void setStatsDefault()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetFloat("TowerHealth", 100f);

        PlayerPrefs.SetFloat("BasicMeleeHealth", 10f);
        PlayerPrefs.SetFloat("BasicMeleeDamage", 2f);

        PlayerPrefs.SetFloat("BasicRangedHealth", 8f);
        PlayerPrefs.SetFloat("BasicRangedDamage", 1.5f);

        PlayerPrefs.SetFloat("MeleeTwoHealth", 25f);
        PlayerPrefs.SetFloat("MeleeTwoDamage", 5f);
    }

    public void updateStats()
    {
        tower.GetComponent<UnitStats>().health = PlayerPrefs.GetFloat("TowerHealth");

        basicMelee.GetComponent<UnitStats>().health = PlayerPrefs.GetFloat("BasicMeleeHealth");
        basicMelee.GetComponent<UnitStats>().damage = PlayerPrefs.GetFloat("BasicMeleeDamage");

        basicRanged.GetComponent<UnitStats>().health = PlayerPrefs.GetFloat("BasicRangedHealth");
        basicRanged.GetComponent<UnitStats>().damage = PlayerPrefs.GetFloat("BasicRangedDamage");

        meleeTwo.GetComponent<UnitStats>().health = PlayerPrefs.GetFloat("MeleeTwoHealth");
        meleeTwo.GetComponent<UnitStats>().damage = PlayerPrefs.GetFloat("MeleeTwoDamage");

    }

    


    //<-------------------public methods to be pressed by button to spawn specific unit ----------------------------->//
    public void spawnBasicMelee()
    {
        //subtract 100 mana, enqueue unit
        if(mana >= 50)
        {
            mana -= 50;
            spawnQueue.Enqueue(basicMelee);
        }
    }

    public void spawnBasicRanged()
    {

        if(mana >= 100)
        {
            mana -= 100;
            spawnQueue.Enqueue(basicRanged);
        }
    }

    public void spawnMeleeTwo()
    {
        if(mana >= 200)
        {
            mana -= 200;
            spawnQueue.Enqueue(meleeTwo);
        }
    }

}
