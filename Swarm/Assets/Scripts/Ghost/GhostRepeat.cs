using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GhostRepeat : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;
    public GameObject ghostWeapon;
    public GameObject ghostAttack;

    //current run, saved as the next ghost
    public List<GameLogic.ghostInput> moveset;

    int listSize;

    GameObject boss;

    //last instruction and make loop only run one time
    bool isLast;
    bool hasRun;

    // Use this for initialization
    void Start() {
        boss = GameObject.Find("Boss");
        rb = GetComponent<Rigidbody>();
        //ghost Spawn Point
        transform.position = new Vector3(5, 0.572f, -0.658f);
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        isLast = false;
        hasRun = false;
    }


    void FixedUpdate() {
        transform.LookAt(boss.transform);

        if (moveset.Count > 0) {
            listSize = moveset.Count;
            if (!hasRun) {
                //runs through the motions... :P
                for (int i = 0; i < listSize; i++) {
                    if (i + 1 == listSize) {
                        isLast = true;
                    }
                    //Debug.Log("Button: " + moveset[i].keyPress + " Start: " + moveset[i].pressStart + " End: " + moveset[i].pressTime);
                    StartCoroutine(ghostMovement(moveset[i].keyPress, moveset[i].pressStart, moveset[i].pressTime, isLast));
                }
                //Debug.Log(gameObject.name + " has " + listSize + " commands");
                hasRun = true;
            }
        }
    }

    IEnumerator ghostMovement(string key, float delay, float end, bool last) {

        //wait til the right time to start input
        yield return new WaitForSeconds(delay);

        //start given input
        if (key == "W") {
            rb.velocity = new Vector3(-3, 0, rb.velocity.z);
        }

        if (key == "S") {
            rb.velocity = new Vector3(3, 0, rb.velocity.z);
        }

        if (key == "A") {
            rb.velocity = new Vector3(rb.velocity.x, 0, -3);
        }

        if (key == "D") {
            rb.velocity = new Vector3(rb.velocity.x, 0, 3);
        }

        //wait for time input is to be going
        yield return new WaitForSeconds(end);

        //stop given input
        if (key == "W") {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
        }

        if (key == "S") {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
        }

        if (key == "A") {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }

        if (key == "D") {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }

        if (key == "Space") {
            StartCoroutine(doAttack());
        }

        //if last command, kill self after executing
        if (last) {
            Destroy(gameObject);
        }
    }

    IEnumerator doAttack() {
        GameObject attack;
        attack = Instantiate(ghostAttack, ghostWeapon.transform.position, ghostWeapon.transform.rotation) as GameObject;

        attack.transform.parent = gameObject.transform;

        yield return new WaitForSeconds(0.25f);

        Destroy(attack);
    }

}