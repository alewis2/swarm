using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialText : MonoBehaviour {

	public GameObject player; 

	public Text tutorialText;

	private float playerPosX;

	// Use this for initialization
	void Start () {
		playerPosX = player.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		
		//check player position
		playerPosX = player.transform.position.x;

		if (playerPosX > 6) {
			tutorialText.text = "Welcome to SWARM!\n\nUse WASD keys to move around.";
		}

        else if (playerPosX <= 6)
        {
            tutorialText.text = "Use the spacebar to attack enemies.\nMost enemies will try to attack back, but this one is safe to wail on.";
        }
		
        else if (playerPosX <= -4)
        {
            tutorialText.text = "3";
        }

        else if (playerPosX <= -10)
        {
            tutorialText.text = "4";
        }
		
		else {
			tutorialText.text = "";
		}
	}
}
