using System.Collections;
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
    public float distanceMinEnemJoueur = 10;
    int modulXn = 3;
    public  void SetupBoard()
    {
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
        GetComponent<GridManager>().CreateGrid(gridToInt);
        InstantiateEnemies(tiles);

    }
    

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
        rooms = new Room[numRooms.Random];
        
        corridors = new Corridor[rooms.Length - 1];
        
        rooms[0] = new Room();
        corridors[0] = new Corridor();
        
        rooms[0].SetupRoom(largRoom, hautRoom, largeur, hauteur);
        corridors[0].SetupCorridor(rooms[0], longCorridor, largRoom, hautRoom, largeur, hauteur, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room();
            
            rooms[i].SetupRoom(largRoom, hautRoom, largeur, hauteur, corridors[i - 1]);
            
            if (i < corridors.Length)
            {
                corridors[i] = new Corridor();
                corridors[i].SetupCorridor(rooms[i], longCorridor, largRoom, hautRoom, largeur, hauteur, false);
            }

            
        }

    }


    void SetTilesValuesForRooms()
    {
        int nbr = 0;
        SetNbrChest();
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            
            for (int j = 0; j < currentRoom.largRoom; j++)
            {
                int xCoord = currentRoom.posX + j;
                
                for (int k = 0; k < currentRoom.hautRoom; k++)
                {
                    int yCoord = currentRoom.posY + k;

                    int[] pos = { xCoord, yCoord };
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
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];
            
            for (int j = 0; j < currentCorridor.longCorridor; j++)
            {
                int xCoord = currentCorridor.startX;
                int yCoord = currentCorridor.startY;
                
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
                if (tiles[i][j] == TileType.Floor)
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

    
    void InstantiateObject(GameObject prefab,float x,float y)
    {
        Vector3 position = new Vector3(2f * x, 2f * y, 0f);
        GameObject tileInstance = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        tileInstance.transform.parent = boardHolder.transform;
    }
    void InstantiateEnemies(TileType[][] t)
    {
        Vector3 player = GameManager.instance.player.transform.position;
        int nbrEnemy = Mathf.RoundToInt(nbrEnemyBase * ((((float)GameManager.instance.level-1) / 8 + 1) ));

        for (int i = 0; i < nbrEnemy; i++)
        {
            int rng = Random.Range(0, potentialEn.Count-1);
            int xCoord = potentialEn[rng][0];
            int yCoord = potentialEn[rng][1];
            potentialEn.RemoveAt(rng);
            InstantiateAnEnemy(new Vector3(2 * xCoord, 2 * yCoord, 0));
            
        }
        
    }
    void InstantiateAnEnemy(Vector3 position)
    {
        int whatEn = Random.Range(0, enemyList.Length);
        GameObject enemy = Instantiate(enemyList[whatEn], position, Quaternion.identity);
        enemy.transform.parent = GameObject.Find("Enemies").transform;                
    }
    void AddChest(Vector3 position, bool key)
    {
        int whatChest = 0;
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
