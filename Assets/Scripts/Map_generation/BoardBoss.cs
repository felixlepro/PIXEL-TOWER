using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBoss : MonoBehaviour
{

    public enum TileType
    {
        Wall, Floor, Chest, Door, Exit
    }
    public int hauteur = 69;
    public int largeur = 69;

    public IntRange hautRoomIni = new IntRange(3, 10);
    public IntRange largRoomIni = new IntRange(3, 10);

    public IntRange hautRoomBoss = new IntRange(3, 10);
    public IntRange largRoomBoss = new IntRange(3, 10);
    public IntRange longCorridor = new IntRange(6, 10);
    public GameObject floorTiles;
    public GameObject mur_Nord;
    public GameObject mur_Sud;
    public GameObject mur_Est;
    public GameObject mur_Ouest;
    public GameObject voidTile;
    public GameObject Door;

    public GameObject exit;
    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    
    
    public GameObject[] enemyList;

    GameObject boardHolder;
    int level;

    int[,] gridToInt;
   [HideInInspector] public int nbrEnemy;

    public void SetupBoard(int lvl)
    {
        //boardHolder = Instantiate(boardHolder, Vector3.zero, Quaternion.identity);
        level = lvl / (GameManager.instance.nbrFloorEntreBoss+1);
        Destroy(GameObject.Find("Board Holder"));
        Destroy(GameObject.Find("Chest Holder"));
        boardHolder = new GameObject("Board Holder");
      //  chestHolder = new GameObject("Chest Holder");
        SetUpTilesArray();

        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();
        gridToInt = new int[tiles.Length, tiles[0].Length];
        InstantiateTiles();
        GetComponent<GridManager>().CreateGrid(gridToInt);
        InstantiateEnemies(tiles);


    }
    //private int[,] TyleTypeToInt(TileType[][] t)
    //{
    //    //int NodeContour = GetComponent<GridManager>().NodeContour;

    //    int[,] grid = new int[t.Length, t[0].Length];
    //    for (int x = 0; x < t.Length; x++)
    //    {
    //        for (int y = 0; y < t[x].Length; y++)
    //        {
    //            //grid[x,y] = (t[x][y] == TileType.Floor)?1:0;
    //            if (t[x][y] == TileType.Floor)
    //            {
    //                grid[x, y] = 1;
    //            }
    //            else if (t[x][y] == TileType.Chest)
    //            {
    //                grid[x, y] = 3;
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
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileType[hauteur+1];
        }
    }
    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[2];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(largRoomIni, hautRoomIni, largeur, hauteur);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], longCorridor, largRoomBoss, hautRoomBoss, largeur, hauteur, true,0);

        rooms[1] = new Room();
        rooms[1].SetupRoom(largRoomBoss, hautRoomBoss, largeur, hauteur, corridors[0]);

        //// If we haven't reached the end of the corridors array...
        //if (i < corridors.Length)
        //{
        //    // ... create a corridor.
        //    corridors[i] = new Corridor();

        //    // Setup the corridor based on the room that was just created.
        //    corridors[i].SetupCorridor(rooms[i], longCorridor, largRoom, hautRoom, largeur, hauteur, false);
        //}


        // }

    }


    void SetTilesValuesForRooms()
    {
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
                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                     tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }

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
                if (j == currentCorridor.longCorridor-1)
                {
                    tiles[xCoord][yCoord] = TileType.Door;
                }
                else tiles[xCoord][yCoord] = TileType.Floor;
                
            }
        }
    }


    void InstantiateTiles()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j] == TileType.Floor || tiles[i][j] == TileType.Chest)
                {
                    InstantiateObject(floorTiles, i, j);
                    if (gridToInt[i, j] != 3)
                    {
                        gridToInt[i, j] = 1;
                    }

                }


                if (tiles[i][j] == TileType.Wall && j > 0)
                {
                    if (tiles[i][j - 1] == TileType.Floor || tiles[i][j-1] == TileType.Door)
                    {
                        InstantiateObject(mur_Nord, i, j);
                        if (corridors[0].startX == i)
                        {
                            InstantiateObject(exit, i, j);
                        }
                    }
                }
                if (tiles[i][j] == TileType.Wall && i < largeur - 1)
                {

                    if (tiles[i + 1][j] == TileType.Floor || tiles[i + 1][j] == TileType.Door)
                    {
                        InstantiateObject(mur_Ouest, i + .35f, j + .72f);
                    }

                }
                if (tiles[i][j] == TileType.Wall && j < hauteur - 1)
                {
                    if (tiles[i][j + 1] == TileType.Floor || tiles[i][j+1] == TileType.Door)
                    {
                        InstantiateObject(mur_Sud, i, j + 1.1f);
                        InstantiateObject(voidTile, i, j + .72f);
                        gridToInt[i, j + 1] = 3;
                    }


                }
                if (tiles[i][j] == TileType.Wall && i > 0)
                {
                    if (tiles[i - 1][j] == TileType.Floor || tiles[i - 1][j] == TileType.Door)
                    {
                        InstantiateObject(mur_Est, i - .35f, j + .72f);
                    }

                }
                if (tiles[i][j] == TileType.Door)
                {

                    InstantiateObject(floorTiles, i, j);
                    InstantiateObject(Door, i, j - 0.5f);
                    gridToInt[i, j] = 1;


                }
            }
        }
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
    void InstantiateObject(GameObject prefab, float x, float y)
    {
        Vector3 position = new Vector3(2f * x, 2f * y, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefab, position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

void InstantiateEnemies(TileType[][] t)
{
        nbrEnemy = level;// Mathf.CeilToInt((float)level / 2f);

        float ecart = rooms[1].largRoom / (nbrEnemy+1);
        for (int i = 1; i <= nbrEnemy; i++)
        {
            float xCoord = rooms[1].posX + i * ecart;
            float yCoord = Mathf.RoundToInt(rooms[1].hautRoom/2) + rooms[1].posY;
            InstantiateAnEnemy(new Vector3(2 * xCoord, 2 * yCoord, 0));

        }
}
void InstantiateAnEnemy(Vector3 position)
{
    int whatEn = Random.Range(0, enemyList.Length);
    GameObject enemy = Instantiate(enemyList[whatEn], position, Quaternion.identity);
    enemy.transform.parent = GameObject.Find("Enemies").transform;
}
//    void AddChest(Vector3 position)
//    {
//        int whatChest = 0;//Random.Range(0, enemyList.Length);
//        GameObject enemy = Instantiate(chestList[whatChest], position, Quaternion.identity);
//        enemy.transform.parent = GameObject.Find("Chest Holder").transform;
//    }
//    void SetNbrChest()
//    {
//        int newNbrChest = 1;
//        float rng = Random.value;
//        rng = rng * rng * rng;
//        for (int i = 1; i <= nbrChestMax; i++)
//        {
//            float cossin = i;
//            float cossin2 = nbrChestMax;

//            if (rng < cossin / cossin2)
//            {
//                newNbrChest = i;
//                break;
//            }
//        }
//        nbrChest = newNbrChest;
//    }
//    void SetUpChest()
//    {
//        int nbrPot = potentialChest.Count / 2;
//        for (int i = 0; i < nbrChest; i++)
//        {
//            int rng = Random.Range(0, nbrPot);
//            int xCoord = potentialChest[rng * 2];
//            int yCoord = potentialChest[rng * 2 + 1];
//            if (tiles[xCoord][yCoord] != TileType.Chest)
//            {
//                AddChest(new Vector3(2 * xCoord, 2 * yCoord, 0));
//                tiles[xCoord][yCoord] = TileType.Chest;
//            }
//            else tiles[xCoord][yCoord] = TileType.Floor;

//        }

//    }
}