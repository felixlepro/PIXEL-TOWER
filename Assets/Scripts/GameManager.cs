﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
   // public  GameObject playerPrefab;
    //public  GameObject piggyPrefab;
    public GameObject player;
    public GameObject piggy;
    public float levelStartDelay = 2f;
    public static GameManager instance;
   // public static int coinCount;
    //public int playerHp = 100;
    public bool inLevel = true;
    public AudioSource audio;
    public AudioClip coinSound;
    public GameObject coinPrefab;
    public GameObject key;
    public GameObject[] weapons;
    

    private  Text coinCounttext;
    private Board boardScript;
    BoardBoss boardBoss;
    public int level;
    public int nbrFloorEntreBoss = 4;
    private List<EnemyManager> enemies;
    private bool doingSetup = true;
    private Text levelText;
    private GameObject levelImage;
    [HideInInspector] public List<Vector3> wayPointList = new List<Vector3>();

    //public struct PlayerStats
    //{
    //   public int hp { get;set; }
    //    public int coins { get; set; }
    //    //public int currentWeaponIndex { get; set; }
    //    public GameObject[] weapons { get; set; }

    //    public PlayerStats(int h, int c, GameObject[] wea)
    //    {
    //        hp = h;
    //        coins = c;
    //       // currentWeaponIndex = cw;
    //        weapons = wea;
           
    //    }
    //}
  //  [HideInInspector] public static PlayerStats playerStat;

    //METTRE LA BOOL DE PROG COMME DANS COIN ET GAMEMANAGER AVEC BOOL ET SHOPMANAGER


    // Use this for initialization
    void Awake()
    {
        

       
        if (instance == null)
        {
            instance = this;
            instance.level = 0;
        }
        else if (instance != this)
            Destroy(gameObject);
       
        //player = GameObject.Find("Pilot");
        //piggy = GameObject.Find("Piggy");

        // player.GetComponent<Player>().setPlayerStats(playerStat.hp, playerStat.coins, playerStat.currentWeaponIndex, playerStat.weapons, level == 0);

       
        audio = GetComponent<AudioSource>();

        enemies = new List<EnemyManager>();
        if (instance.level == 0)
        {
            instance.boardScript = GetComponent<Board>();
            instance.boardBoss = GetComponent<BoardBoss>();
            instance.player = Instantiate(player, new Vector3(boardScript.hauteur, boardScript.largeur, 0), Quaternion.identity);
            instance.piggy = Instantiate(piggy, new Vector3(boardScript.hauteur, boardScript.largeur, 0), Quaternion.identity);
        }
        instance.player.transform.position = new Vector3(instance.boardScript.hauteur, instance.boardScript.largeur, 0);
        instance.piggy.transform.position = player.transform.position;

        DamageTextManager.Initialize();
        DropManager.Initialize();
        // DontDestroyOnLoad(player);
        //DontDestroyOnLoad(piggy);
        instance.wayPointList.Clear();
        loadNewLevel();

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(piggy);
       // DontDestroyOnLoad(gameObject.GetComponent<GameManager>());
       // Debug.Log(test);
        
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
    public void Restart()
    {
        //Player p = player.GetComponent<Player>();
       // playerStat = new PlayerStats(p.hp , p.coins , p.weaponObjects());
        //if (GameManager.instance.inLevel)
        //{
        //    SceneManager.LoadScene(2);
        //    inLevel = false;
        //}
        //else
        {
            SceneManager.LoadScene(1);
            inLevel = true;
        }
    }
    void loadNewLevel()
    {
        
        doingSetup = true;
        enemies.Clear();
        levelImage = GameObject.Find("LevelImage");
        instance.level += 1;
        Debug.Log("Floor: " + instance.level);
        if ((instance.level % 4) == 0)
        {
            instance.boardBoss.SetupBoard(instance.level);
            SetupAI();
        }
        else
        {
            instance.boardScript.SetupBoard(instance.level);
            SetupAI();
            ActivateAI(true);
        }

    }


    void SetupAI()
    {
        foreach (GameObject em in GameObject.FindGameObjectsWithTag("EnemyManager"))
        {
            em.GetComponent<EnemyManager>().SetStats(level);
            em.GetComponent<EnemyManager>().SetupAI(instance.wayPointList);
        }
    }
    public void ActivateAI(bool tf)
    {
        foreach (GameObject em in GameObject.FindGameObjectsWithTag("EnemyManager"))
        {
            em.GetComponent<EnemyManager>().ActivateAI(tf);

        }
    }
 
    public int GetCurrentLevel()
    {
        return level;
    }
}
