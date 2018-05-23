using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void GotoMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void GotoTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    public void GotoCredits() {
        SceneManager.LoadScene("Credits");
    }

    public void GotoLevel1() {
        SceneManager.LoadScene("Level 1");
    }

    public void GotoLevel2() {
        SceneManager.LoadScene("Level 2");
    }

	/*
    public void GotoCredits() {
        SceneManager.LoadScene("Credits");
    }
    public void GotoCredits() {
        SceneManager.LoadScene("Credits");
    }
    public void GotoCredits() {
        SceneManager.LoadScene("Credits");
    }
	*/
}
