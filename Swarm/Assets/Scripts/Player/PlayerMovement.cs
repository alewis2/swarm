using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject ghost;
    public GameObject playerWeapon;
    public GameObject playerAttack;

    Vector3 startPosition;

    float attackCooldown;
    float startTime;
    //float slowTime;
    //float slowCooldown;

    // Use this for initialization
    void Start()
    {
        //slowTime = 0;
        //slowCooldown = 0;
        Debug.Log(GameLogic.Instance.myGlobalVar);
        rb = GetComponent<Rigidbody>();
        //Player Spawn Point
        transform.position = new Vector3(transform.position.x, 0.572f, transform.position.z);
        startPosition = transform.position;
        Physics.IgnoreCollision(ghost.GetComponent<Collider>(), GetComponent<Collider>());
        attackCooldown = 0;
    }


    void Update() {
//       slowCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) ) {
            //    slowTime = Time.time;  && slowCooldown <= 0
            Time.timeScale = .5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) ) {
            Time.timeScale = 1;
            //   slowCooldown = 10; || slowTime < Time.time + 3
        }

        playerMovement();

        //attaking using space or LMB
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && attackCooldown <= Time.time) {
            GameLogic.ghostInput temp;
            attackCooldown = Time.time + 0.7f;
            StartCoroutine(doAttack());
            temp = new GameLogic.ghostInput("Space", 0, Time.time - GameLogic.Instance.startTime);
            GameLogic.Instance.currentGhost.Add(temp);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            transform.position = startPosition;
        }
    }

    void playerMovement() {
        //give velocity on correct axis with instant accel
        int horizontal = 0;
        int vertical = 0;

        //check for movement direction
        if (Input.GetKey(KeyCode.W)) {
            horizontal = -1;
        }

        if (Input.GetKey(KeyCode.S)) {
            horizontal = 1;
        }

        if (Input.GetKey(KeyCode.A)) {
            vertical = -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            vertical = 1;
        }

        //add correct movement direction
        rb.velocity = new Vector3(horizontal * 3, 0, vertical * 3);

    }

    IEnumerator doAttack() {
        GameObject attack;
        attack = Instantiate(playerAttack, playerWeapon.transform.position, playerWeapon.transform.rotation) as GameObject;

        attack.transform.parent = gameObject.transform;

        yield return new WaitForSeconds(0.25f);

        Destroy(attack);
    }
}