using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Board : MonoBehaviour {
    public enum TileType
    {
        Wall,Floor
    }
    public int hauteur = 100;
    public int largeur = 100;
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

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    private GameObject boardHolder;

    public void SetupBoard()
    {
        
        boardHolder = new GameObject("Board Holder");

        SetUpTilesArray();

        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        AstarPath.active.Scan();

    }

    private void SetUpTilesArray()
    {
        tiles = new TileType[largeur][];
        for (int i = 0;i<tiles.Length;i++)
        {
            tiles[i] = new TileType[hauteur];
        }
        hauteur--;
        largeur--;
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
                }
                

                if (tiles[i][j] == TileType.Wall && j > 0 )
                {
                    if (tiles[i][j-1]==TileType.Floor )
                    {
                        InstantiateObject(mur_Nord, i, j);
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
}
