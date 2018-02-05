﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] floortiles;
    public GameObject mur_Nord;
    public GameObject mur_Sud;
    public GameObject mur_Est;
    public GameObject mur_Ouest;
    public GameObject voidTile;

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    private GameObject boardHolder;

    private void Start()
    {
        boardHolder = new GameObject("Board Holder");

        SetUpTilesArray();

        CreateRoomsAndCorridors();





    }   
    private void SetUpTilesArray()
    {
        tiles = new TileType[largeur][];
        for (int i = 0;i<tiles.Length;i++)
        {
            tiles[i] = new TileType[hauteur];
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


    //void InstantiateTiles()
    //{
    //    // Go through all the tiles in the jagged array...
    //    for (int i = 0; i < tiles.Length; i++)
    //    {
    //        for (int j = 0; j < tiles[i].Length; j++)
    //        {
    //            // ... and instantiate a floor tile for it.
    //            InstantiateFromArray(floorTiles, i, j);

    //            // If the tile type is Wall...
    //            if (tiles[i][j] == TileType.Wall)
    //            {
    //                // ... instantiate a wall over the top.
    //                InstantiateFromArray(wallTiles, i, j);
    //            }
    //        }
    //    }
    //}
    
    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
}
