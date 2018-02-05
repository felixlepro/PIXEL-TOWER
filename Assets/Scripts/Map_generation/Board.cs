using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public enum TileType
    {
        wall,floor
    }
    public int hauteur = 100;
    public int largeur = 100;
    public IntRange numRooms = new IntRange(15, 20);
    public IntRange hautRooms = new IntRange(3, 10);
    public IntRange largRooms = new IntRange(3, 10);
    public IntRange longCorridor = new IntRange(6, 10);
    public GameObject[] floortiles;
    public GameObject mur_Nord;
    public GameObject mur_Sud;
    public GameObject mur_Est;
    public GameObject mur_Ouest;
    public GameObject voidTile;

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridor;
    private GameObject boardHolder;

    private void Start()
    {
        boardHolder = new GameObject("Board Holder");

        SetUpTilesArray();

        CreateRoomsCorridor();


        

    }   
    private void SetUpTilesArray()
    {
        tiles = new TileType[largeur][];
        for (int i = 0;i<tiles.Length;i++)
        {
            tiles[i] = new TileType[hauteur];
        }
    }
    private void CreateRoomsCorridor()
    {

    }
}
