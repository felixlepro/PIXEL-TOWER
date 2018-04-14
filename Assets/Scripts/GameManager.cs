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
    public AudioSource audio;
    public AudioClip coinSound;
    public GameObject[] weapons;

    private Text coinCounttext;
    private Board boardScript;
    BoardBoss boardBoss;
    private int level = 0;
    public int nbrFloorEntreBoss = 4;
    private List<EnemyManager> enemies;
    private bool doingSetup = true;
    private Text levelText;
    private GameObject levelImage;
    [HideInInspector] public List<Vector3> wayPointList;

    //METTRE LA BOOL DE PROG COMME DANS COIN ET GAMEMANAGER AVEC BOOL ET SHOPMANAGER


    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<EnemyManager>();
        boardScript = GetComponent<Board>();
        boardBoss = GetComponent<BoardBoss>();
        loadNewLevel();
        //InitGame();
        DamageTextManager.Initialize();
        audio = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
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
        //instance.level++;
        //instance.InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        enemies.Clear();
        levelImage = GameObject.Find("LevelImage");
        boardScript.SetupBoard(level);
        
        SetupAI();
        //levelText = GameObject.Find("LevelText").GetComponent<Text>();
        //levelText.text = "Level " + level;
        // levelImage.SetActive(true);
        // Invoke("HideLevelImage", levelStartDelay);

    }
    void loadNewLevel()
    {
        doingSetup = true;
        enemies.Clear();
        levelImage = GameObject.Find("LevelImage");
        level += 1;
        Debug.Log(level);
        if ((level % 4) == 0)
        {
            boardBoss.SetupBoard(level);
        }
        else boardScript.SetupBoard(level);
        SetupAI();
    }
   

    void SetupAI()
    {
        foreach (GameObject em in GameObject.FindGameObjectsWithTag("EnemyManager"))                                     //temporaire
        {
            em.GetComponent<EnemyManager>().SetStats(level);
            em.GetComponent<EnemyManager>().SetupAI(wayPointList);
        }
    }
    // Update is called once per frame
    void Update () {

      //  coinCounttext.text = ("Coins: " + coinCount); 

    }
}
