using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    public int coinCount = 0;
    public int playerHp = 100;
    public bool inLevel = true;

    private Text coinCounttext;
    private Board boardScript;
    private int level = 1;
    private List<Enemy> enemies;
    private bool doingSetup = true;
    private Text levelText;
    private GameObject levelImage;
    [HideInInspector] public List<Transform> wayPointList;



    //METTRE LA BOOL DE PROG COMME DANS COIN ET GAMEMANAGER AVEC BOOL ET SHOPMANAGER


    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<Board>();

        InitGame();
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        enemies.Clear();
        levelImage = GameObject.Find("LevelImage");
        //levelText = GameObject.Find("LevelText").GetComponent<Text>();
       //levelText.text = "Level " + level;
       // levelImage.SetActive(true);
       // Invoke("HideLevelImage", levelStartDelay);
        boardScript.SetupBoard(level);
        SetupAI();
    }
    void SetupAI()
    {
        foreach (GameObject wp in GameObject.FindGameObjectsWithTag("waypoints"))                                     //temporaire
        {
            wayPointList.Add(wp.transform);
        }
        foreach (GameObject em in GameObject.FindGameObjectsWithTag("EnemyManager"))                                     //temporaire
        {
            em.GetComponent<StateController>().SetupAI(true, wayPointList);
        }
    }
    // Update is called once per frame
    void Update () {

      //  coinCounttext.text = ("Coins: " + coinCount); 

    }
}
