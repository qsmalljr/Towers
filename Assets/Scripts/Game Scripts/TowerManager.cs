using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {

    float height;
    float width;
    public GameObject tower;
	
	void Start () {

        //get the screen height and width based on camera (located in center of page)
        Camera cam = FindObjectOfType<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;


    }

    public void spawnTower()
    {
        //put tower on the left side of the screen
        Vector2 pos = new Vector2(-width / 2f + ((tower.transform.lossyScale.x * tower.GetComponent<BoxCollider2D>().size.x) / 2f), (-height / 2f) + ((tower.transform.lossyScale.y * tower.GetComponent<BoxCollider2D>().size.y) / 2f));


        //spawn the tower
        Instantiate(tower, pos, Quaternion.identity);
    }

}
