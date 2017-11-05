using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour {

    public float health;
    public float damage;
    public float speed;


    public bool isRanged;
    public bool isEnemy;
    public bool isMoving = true;

    public AudioClip attackSound;
    private AudioSource audioSource;

    private float height;
    private float width;
    private DifficultyScript difficultyScript;
    private Animator animator;

    private void Start()
    {
        //get the screen height and width based on camera (located in center of page)
        Camera cam = FindObjectOfType<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        //get the animator for messing with animations
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        difficultyScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<DifficultyScript>();
    }

    private void Update()
    {
        //move the object
        if(isMoving)
            transform.Translate(Time.deltaTime * speed, 0, 0);
        //destroy this unit if its health is death worthy or if it is off the screen
        if (health <= 0)
            Destroy(gameObject);
        //kill unfriendlies if they are off the screen
        if (isEnemy && transform.position.x < -width / 2f - ((transform.lossyScale.x * gameObject.GetComponent<BoxCollider2D>().size.x)/2f))
            Destroy(gameObject);
        //kill friendly if they are off the screen
        if (!isEnemy && transform.position.x > width / 2f + ((transform.lossyScale.x * gameObject.GetComponent<BoxCollider2D>().size.x)/2f))
            Destroy(gameObject);
        //if a friendly hits the edge, signal that this was done so game can end
        if (transform.position.x >= width / 2f && !isEnemy && !difficultyScript.friendlyHitEdge)
            difficultyScript.friendlyHitEdge = true;

    }

    IEnumerator attack(UnitStats unitToAttack)
    {

        while(unitToAttack != null)
        {
            if(audioSource != null && !GameObject.FindGameObjectWithTag("GameController").GetComponent<DifficultyScript>().effectMuted)
                audioSource.PlayOneShot(attackSound);

            unitToAttack.health -= damage;
            yield return new WaitForSeconds(1f);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get stats of other collided object
        UnitStats unitStats = collision.gameObject.GetComponent<UnitStats>();



        //when friendly unit encounters enemy unit (and the friendly isnt the base)
        if (!isEnemy && unitStats.isEnemy && tag != "Castle")
        {
            //if bumped into ranged:
            if (unitStats.isRanged)
            {
                //only stop and attack if encountered the box collider
                if(collision == collision.GetComponent<BoxCollider2D>())
                {
                    isMoving = false;
                    //initiate attack animation

                    //subtract health from unit
                    animator.SetBool("isAttacking", true);

                    StartCoroutine(attack(unitStats));
                }

            }
            //if bumped into melee
            else
            {
                isMoving = false;
                animator.SetBool("isAttacking", true);

                StartCoroutine(attack(unitStats));
            }

        }
        //when friendly unit encounters friendly unit
        else if(!isEnemy && !unitStats.isEnemy)
        {
            //melee hit melee - stop
            //melee hit range - go through
            //range hit melee - go through
            //range hit range - stop
            if (!unitStats.isRanged && !isRanged && collision.tag != "Castle")
            {

                isMoving = false;

                //stop walking animation

                //if ranged and in attacking distance, do attacking animation
            }
            else if (isRanged && unitStats.isRanged && collision.tag != "Castle")
            {
                //if box collider hits box collider
                if (collision.IsTouching(gameObject.GetComponent<BoxCollider2D>()) && collision == collision.GetComponent<BoxCollider2D>())
                    isMoving = false;
            }
        }
        //when enemy unit encounters friendly unit
        else if(isEnemy && !unitStats.isEnemy)
        {
            //only do actions if encountered a box collider(only box collider has the isTrigger option
            if (unitStats.isRanged)
            {
                //only stop if encountered the box collider
                if (collision == collision.GetComponent<BoxCollider2D>())
                {
                    //stop
                    isMoving = false;

                    //attack + animate
                    animator.SetBool("isAttacking", true);
                    StartCoroutine(attack(unitStats));
                }

            }
            else
            {
                isMoving = false;
                animator.SetBool("isAttacking", true);
                StartCoroutine(attack(unitStats));

            }

        }
        //when enemy unit encounters enemy unit
        else
        {
            if (!unitStats.isRanged && !isRanged)
            {
                isMoving = false;
            }
            else if (isRanged && unitStats.isRanged)
            {
                //if box collider hits box collider
                if (collision.IsTouching(gameObject.GetComponent<BoxCollider2D>()) && collision == collision.GetComponent<BoxCollider2D>())
                    isMoving = false;
            }

        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isEnemy && collision.tag == "Castle" && collision != null)
            isMoving = false;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //get stats of other collided object
        //UnitStats unitStats = collision.gameObject.GetComponent<UnitStats>();

        //stop attacking animation if attacking
        //start walking animation
        if (isEnemy && collision.tag == "Castle" && collision != null)
        {

        }
        else
        {
            //start moving
            if (!isMoving)
                isMoving = true;
            if (tag != "Castle")
                animator.SetBool("isAttacking", false);
        }

    }


}
