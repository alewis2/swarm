using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMobAI : MonoBehaviour
{

    Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        startPosition = new Vector3(0.97f, 0.572f, -0.658f);
    }

    // Update is called once per frame
    void Update()
    {

        //"Kill" the boss (really just hide under the stage)
        if (GameLogic.Instance.bossHP <= 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -2, gameObject.transform.position.z);
        }

        //"Revive" the boss
        if (GameLogic.Instance.bossHP > 0)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = startPosition;
        }
    }
}