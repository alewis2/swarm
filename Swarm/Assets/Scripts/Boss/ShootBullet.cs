using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour
{
    //Drag in the Bullet Emitter from the Component Inspector.
    public GameObject Bullet_Emitter;

    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Bullet;

    //Enter the Speed of the Bullet from the Component Inspector.
    public float Bullet_Forward_Force;

    //delay between shots
    int aoeCooldown;
    float burstDelay;
    float delay;
    float burstReset;
    float startDelay;
    float resetDelay;

    // Use this for initialization
    void Start() {
        aoeCooldown = 0;
        burstDelay = 3f;
        resetDelay = 0.1f;
        burstReset = burstDelay;
        startDelay = resetDelay;
        delay = resetDelay;
        
    }

    // Update is called once per frame
    void Update() {

        //stop shooting if boss is "dead"
        if (gameObject.transform.position.y > 0) {

            //reset counters on restart
            if (Input.GetKeyDown(KeyCode.R)) {
                aoeCooldown = 0;
                burstDelay = burstReset;
                delay = resetDelay;
            }


            delay -= Time.deltaTime;
            burstDelay -= Time.deltaTime;

            if (burstDelay <= 1) {
                if (delay <= 0) {
                    //The Bullet instantiation happens here.
                    GameObject bullet;

                    bullet = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
                    bullet.transform.Rotate(Vector3.left * 90);

                    //Retrieve the Rigidbody component from the instantiated Bullet and control it.
                    Rigidbody Temporary_RigidBody;
                    Temporary_RigidBody = bullet.GetComponent<Rigidbody>();

                    //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
                    Temporary_RigidBody.AddForce(transform.forward * Bullet_Forward_Force);

                    //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
                    Destroy(bullet, 10.0f);
                    delay = resetDelay;
                }
            }

            //reset burst cooldown
            if (burstDelay < 0) {
                aoeCooldown++;
                burstDelay = burstReset;
            }

            if (aoeCooldown == 3) {
                aoeCooldown = 0;
                for (int i = 0; i < 360; i = i + 10) {
                    aoeBurst(i);
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                delay = startDelay;
                aoeCooldown = 0;
            }
        }
    }

    void aoeBurst(int offsetAngle) {

        GameObject bullet;

        bullet = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

        bullet.transform.Rotate(new Vector3(-90,0,0 + offsetAngle));

        Rigidbody Temporary_RigidBody;
        Temporary_RigidBody = bullet.GetComponent<Rigidbody>();

        Temporary_RigidBody.AddForce(bullet.transform.rotation * bullet.transform.forward * Bullet_Forward_Force/2);

        Destroy(bullet, 5.0f);
    }


}
