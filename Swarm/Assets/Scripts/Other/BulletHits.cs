using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHits : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        
        //dont trigger if hits self
        if (other.gameObject.name != "Boss") {
        
            //if it hits a target
            if (other.gameObject.name == "HitBox") {

                if (other.transform.parent.name == "Player") {
                    Time.timeScale = 0;
                    GameLogic.Instance.isDead = true;
                    Debug.Log("You have DIED!");
                }
            }
            if (other.gameObject.tag == "Ghost") {
                Destroy(other.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
