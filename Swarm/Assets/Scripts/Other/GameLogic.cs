using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
/// <summary>
public class GameLogic : Singleton<GameLogic> {
    protected GameLogic() { } // guarantee this will be always a singleton only - can't use the constructor!
    
    public string myGlobalVar = "Game Logic is on!";
}

/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public GameObject boss;
    public GameObject ghostPrefab;
    public GameObject player;
    public GameObject attackPrefab;

    //floats for recording movement
    float WEnd;
    float AEnd;
    float SEnd;
    float DEnd;
    float WStart;
    float AStart;
    float SStart;
    float DStart;
    float attackStart;
    float lastAttackTime;
    public bool isDead;

    //boss stats
    public int bossHP;
    int maxHP;

    public class ghostInput {

        public string keyPress;
        public float pressTime;
        public float pressStart;

        public ghostInput(string key, float time, float start) {
            keyPress = key;
            pressTime = time;
            pressStart = start;
        }

        public ghostInput(GameLogic.ghostInput toCopy) {
            keyPress = toCopy.keyPress;
            pressTime = toCopy.pressTime;
            pressStart = toCopy.pressStart;
        }
    }

    //list of ghosts
    List<List<GameLogic.ghostInput>> ghostLists;

    //time round started
    public float startTime;

    //current run, saved as the next ghost
    public List<GameLogic.ghostInput> currentGhost;

    private static T _instance;

    private static object _lock = new object();

    public static T Instance {
        get {
            if (applicationIsQuitting) {
                return null;
            }

            lock (_lock) {
                if (_instance == null) {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1) {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null) {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy() {
        applicationIsQuitting = true;
    }

    void Start() {
        maxHP = 10;
        bossHP = maxHP;
        currentGhost = new List<GameLogic.ghostInput>();
        ghostLists = new List<List<GameLogic.ghostInput>>();
        startTime = Time.time;
        lastAttackTime = startTime;
        Time.timeScale = 0;
    }

    void Update() {

        //starts the game if the player makes an input
        if (Input.anyKeyDown && Time.timeScale == 0 && isDead == false) {
            Time.timeScale = 1;
        }

        //pause button
        if (Input.GetKeyDown(KeyCode.P)) {
            Time.timeScale = 0;
        }

        //reset and add new ghost
        if (Input.GetKeyDown(KeyCode.R)) {
            resetGame();
        }

        recordInput();
    }

    void resetGame() {
        //stops the game asap on reset
        Time.timeScale = 0;
        GameObject newGhost;

        //respawn boss
        bossHP = maxHP;


        //destroy all bullets on screen
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var shot in bullets) {
            Destroy(shot);
        }

        //delete all current ghosts
        var ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (var ghost in ghosts) {
            Destroy(ghost);
        }

        //add new complete ghost and reset currentGhost
        List<GameLogic.ghostInput> input = new List<GameLogic.ghostInput>(currentGhost);
        ghostLists.Add(input);

        //make all new ghosts
        for (int i = 0; i < ghostLists.Count; i++) {
            newGhost = Instantiate(ghostPrefab);
            newGhost.name = "Ghost " + i;
            newGhost.GetComponent<GhostRepeat>().moveset = new List<GameLogic.ghostInput>(ghostLists[i]);
        }
        //reset currentGhost
        currentGhost.Clear();


        //restart the time and game
        startTime = Time.time;
        isDead = false;
    }

    void recordInput() {

        GameLogic.ghostInput temp;

        //Get time at start of key press
        if (Input.GetKeyDown(KeyCode.W)) {
            WStart = Time.time - startTime;
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            AStart = Time.time - startTime;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            SStart = Time.time - startTime;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            DStart = Time.time - startTime;
        }


        //get total key press time and enter to list
        if (Input.GetKeyUp(KeyCode.W)) {
            WEnd = Time.time - WStart;
            temp = new GameLogic.ghostInput("W", WEnd, WStart);
            currentGhost.Add(temp);
        }

        if (Input.GetKeyUp(KeyCode.A)) {
            AEnd = Time.time - AStart;
            temp = new GameLogic.ghostInput("A", AEnd, AStart);
            currentGhost.Add(temp);
        }

        if (Input.GetKeyUp(KeyCode.S)) {
            SEnd = Time.time - SStart;
            temp = new GameLogic.ghostInput("S", SEnd, SStart);
            currentGhost.Add(temp);
        }

        if (Input.GetKeyUp(KeyCode.D)) {
            DEnd = Time.time - DStart;
            temp = new GameLogic.ghostInput("D", DEnd, DStart);
            currentGhost.Add(temp);
        }

/*
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && lastAttackTime >= Time.time -0.3f) {
            lastAttackTime = Time.time;
            attackStart = Time.time - startTime;
            temp = new GameLogic.ghostInput("Space", 0, attackStart);
            currentGhost.Add(temp);
        }
        */
    }

    public void bossTakeDamage(string attackerType) {
        if (attackerType == "Player") {
            bossHP -= 2;
        }

        if (attackerType == "Ghost") {
            bossHP--;
        }

        Debug.Log("Boss HP remaining: " + bossHP);
    }

}