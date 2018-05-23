using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public GameObject playerAttack;
    public GameObject player;
    public GameObject logic;

    bool hasDealtDamage;

	// Use this for initialization
	void Start () {
		hasDealtDamage = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!hasDealtDamage) {
            Collider[] hits = Physics.OverlapBox(playerAttack.transform.position, playerAttack.GetComponent<Renderer>().bounds.size/2, player.transform.rotation);
            foreach (Collider hit in hits) {
                //only hit boss and only once if multiple boss parts tagged
                if (hit.tag == "Boss" && !hasDealtDamage) {
                    GameLogic.Instance.bossTakeDamage(gameObject.transform.parent.tag);
                    hasDealtDamage = true;
                    break;
                }
            }         
        }
    }
}
