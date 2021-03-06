﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public enum TileType
    {
        Wall,Floor,Chest
    }
    public int hauteur = 69;
    public int largeur = 69;
    public IntRange numRooms = new IntRange(15, 20);
    public IntRange hautRoom = new IntRange(3, 10);
    public IntRange largRoom = new IntRange(3, 10);
    public IntRange longCorridor = new IntRange(6, 10);
    public GameObject floorTiles;
    public GameObject mur_Nord;
    public GameObject mur_Sud;
    public GameObject mur_Est;
    public GameObject mur_Ouest;
    public GameObject voidTile;
    public GameObject exit;

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    GameObject boardHolder;

    public GameObject[] enemyList;
    public int nbrEnemyBase;
     List<int[]> potentialEn = new List<int[]>();

    public GameObject[] chestList;
    public int nbrChestMax;
   [HideInInspector] public int nbrChest;
    List<int[]> potentialChest = new List<int[]>();
    List<int[]> potentialExit = new List<int[]>();
    GameObject chestHolder;
    int[,] gridToInt;
    //int level;
    public float distanceMinEnemJoueur = 10;
    int modulXn = 3;
    public  void SetupBoard()
    {
        //boardHolder  = Instantiate(boardHolder, Vector3.zero, Quaternion.identity);
       // Debug.Log("setup");
        //Destroy(boardHolder);
        //  Destroy(GameObject.Find("Board Holder"));
        // Destroy(GameObject.Find("Chest Holder"));

        potentialChest.Clear();
        potentialEn.Clear();
        potentialExit.Clear();

        boardHolder = new GameObject("Board Holder");
         chestHolder = new GameObject("Chest Holder");

        SetUpTilesArray();
        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();
        gridToInt = new int[tiles.Length, tiles[0].Length];
        InstantiateTiles();
        //GetComponent<GridManager>().CreateGrid(TyleTypeToInt(tiles));
        GetComponent<GridManager>().CreateGrid(gridToInt);
        InstantiateEnemies(tiles);

    }
    //private int[,] TyleTypeToInt(TileType[][] t)
    //{
    //    //int NodeContour = GetComponent<GridManager>().NodeContour;

    //    int[,] grid = new int[t.Length,t[0].Length];
    //    for (int x = 0; x < t.Length; x++)
    //    {
    //        for (int y = 0; y < t[x].Length; y++)
    //        {
    //            //grid[x,y] = (t[x][y] == TileType.Floor)?1:0;
    //            if (t[x][y] == TileType.Floor)
    //            {
    //                if (t[x][y - 1] != TileType.Floor)
    //                {
    //                    grid[x, y] = 3;
    //                }
    //                else
    //                {
    //                    grid[x, y] = 1;
    //                    nbrTileFloor += 1;
    //                }
    //            }
    //            else if (t[x][y] == TileType.Chest)
    //            {
    //                grid[x, y] = 2;
    //            }
    //            else
    //            {
    //                grid[x, y] = 0;
    //            }
    //        }
    //    }
    //    return grid;
    //}

    private void SetUpTilesArray()
    {

        tiles = new TileType[largeur+1][];
        for (int i = 0;i<tiles.Length;i++)
        {
            tiles[i] = new TileType[hauteur+1];
        }
    }
    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(largRoom, hautRoom, largeur, hauteur);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], longCorridor, largRoom, hautRoom, largeur, hauteur, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(largRoom, hautRoom, largeur, hauteur, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], longCorridor, largRoom, hautRoom, largeur, hauteur, false);
            }

            
        }

    }


    void SetTilesValuesForRooms()
    {
        int nbr = 0;
        SetNbrChest();
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.largRoom; j++)
            {
                int xCoord = currentRoom.posX + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.hautRoom; k++)
                {
                    int yCoord = currentRoom.posY + k;

                    int[] pos = { xCoord, yCoord };
                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    if (j == Mathf.FloorToInt(currentRoom.largRoom/2) && k == Mathf.FloorToInt(currentRoom.hautRoom/2) && currentRoom != rooms[0])
                    {
                        
                        potentialChest.Add(pos);
                    }
                    else if (new Vector2(xCoord - largeur/2, yCoord - hauteur/2).magnitude > distanceMinEnemJoueur && nbr++%5 == 0)
                    {
                        potentialEn.Add(pos);
                    }
                    tiles[xCoord][yCoord] = TileType.Floor;

                }
            }
        }
        SetUpChest();
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.longCorridor; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startX;
                int yCoord = currentCorridor.startY;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.Nord:
                        yCoord += j;
                        break;
                    case Direction.Est:
                        xCoord += j;
                        break;
                    case Direction.Sud:
                        yCoord -= j;
                        break;
                    case Direction.Ouest:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j] == TileType.Floor) //|| tiles[i][j] == TileType.Chest)
                {
                    InstantiateObject(floorTiles, i, j);
                    if (gridToInt[i, j] != 3)  {
                        gridToInt[i, j] = 1;
                    }
                  
                }
                else if (tiles[i][j] == TileType.Chest)
                {
                    InstantiateObject(floorTiles, i, j);
                    gridToInt[i, j] = 2;
                }

                else if (tiles[i][j] == TileType.Wall && j > 0 )
                {
                    if (tiles[i][j-1]==TileType.Floor )
                    {
                        InstantiateObject(mur_Nord, i, j);
                        if (tiles[i+1][j] == TileType.Wall && tiles[i-1][j] == TileType.Wall )
                        {
                            int[] position = { i, j };
                            potentialExit.Add(position);
                        }
                        
                    }
                    

                }
                if (tiles[i][j] == TileType.Wall && i < largeur - 1)
                {
                    
                    if (tiles[i + 1][j] == TileType.Floor)
                    {
                        InstantiateObject(mur_Ouest, i + .35f, j + .72f);
                    }

                }
                if (tiles[i][j] == TileType.Wall && j <hauteur-1)
                {
                    if (tiles[i][j + 1] == TileType.Floor)
                    {
                        InstantiateObject(mur_Sud, i, j+1.1f);
                        InstantiateObject(voidTile, i, j+.72f);
                        gridToInt[i, j+1] = 3;
                    }
                    
                }
                if (tiles[i][j] == TileType.Wall &&  i > 0)
                {
                    if (tiles[i - 1][j] == TileType.Floor)
                    {
                        InstantiateObject(mur_Est, i - .35f, j + .72f);
                    }

                }
                
            }
        }
        // Instantie lexit
        int rndIndex = Random.Range(0, potentialExit.Count);
        
        InstantiateObject(exit, potentialExit[rndIndex][0], potentialExit[rndIndex][1]);
    }

    //void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    //{
    //    // Create a random index for the array.
    //    int randomIndex = Random.Range(0, prefabs.Length);

    //    // The position to be instantiated at is based on the coordinates.
    //    Vector3 position = new Vector3(2f*xCoord,2f* yCoord, 0f);

    //    // Create an instance of the prefab from the random index of the array.
    //    GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

    //    // Set the tile's parent to the board holder.
    //    tileInstance.transform.parent = boardHolder.transform;
    //}
    void InstantiateObject(GameObject prefab,float x,float y)
    {
        Vector3 position = new Vector3(2f * x, 2f * y, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefab, position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
    void InstantiateEnemies(TileType[][] t)
    {
        Vector3 player = GameManager.instance.player.transform.position;
            //GameObject.Find("Pilot").transform.position;
        int nbrEnemy = Mathf.RoundToInt(nbrEnemyBase * ((((float)GameManager.instance.level-1) / 8 + 1) ));

        for (int i = 0; i < nbrEnemy; i++)
        {
            int rng = Random.Range(0, potentialEn.Count-1);
            int xCoord = potentialEn[rng][0];
            int yCoord = potentialEn[rng][1];
            potentialEn.RemoveAt(rng);
            InstantiateAnEnemy(new Vector3(2 * xCoord, 2 * yCoord, 0));
            
        }
        // float enemyDensity = 1f + level * 0.1f;

        //int[,] grid = new int[t.Length, t[0].Length];
        //for (int x = 0; x < t.Length; x++)
        //{
        //    for (int y = 0; y < t[x].Length; y++)
        //    {
        //        if (t[x][y] == TileType.Floor)
        //        {
        //            Vector3 pos = new Vector3(2 * x, 2 * y, 0);

        //            if (Vector3.Distance(pos, player) > 7 && Random.Range(1, nbrTileFloor) < nbrEnemy)
        //            {
        //                InstantiateAnEnemy(pos);
        //            }
        //            //if (Vector3.Distance(pos,player) > 7 && enemyDensity >= Random.Range(0, 100))
        //            //{

        //            //    InstantiateAnEnemy(pos);
        //            // }
        //        }
        //    }
        //}
    }
    void InstantiateAnEnemy(Vector3 position)
    {
        int whatEn = Random.Range(0, enemyList.Length);
        GameObject enemy = Instantiate(enemyList[whatEn], position, Quaternion.identity);
        enemy.transform.parent = GameObject.Find("Enemies").transform;                
    }
    void AddChest(Vector3 position, bool key)
    {
        int whatChest = 0;//Random.Range(0, enemyList.Length);
        GameObject ch = Instantiate(chestList[whatChest], position, Quaternion.identity);
        ch.transform.parent = chestHolder.transform;
        ch.GetComponent<Chest>().hasKey = key;

        modulXn = (23 * modulXn + 7) % 11;
        ch.GetComponent<Chest>().nbCoins = modulXn + 5;
    }
    void SetNbrChest()
    {     
        float rng = Random.value;
        rng = rng * rng * rng;
        for(int i = 1; i <= nbrChestMax; i++)
        {

            if (rng < (float)i/(float)nbrChestMax )
            {
                nbrChest = i;
                break;
            }
        }      
    }
    void SetUpChest()
    {
        int nbrPot = potentialChest.Count / 2;
        if (nbrPot < nbrChest)
        {
            nbrChest = nbrPot;
        }
        int key = Random.Range(0, nbrChest - 1);
        for (int i =0; i<nbrChest; i++)
        {
            int rng = Random.Range(0, nbrPot);
            int xCoord = potentialChest[rng][0];
            int yCoord = potentialChest[rng][1];
            potentialChest.RemoveAt(rng);
            AddChest(new Vector3(2 * xCoord, 2 * yCoord, 0), (key == i));
            tiles[xCoord][yCoord] = TileType.Chest;          
        }

    }
}
