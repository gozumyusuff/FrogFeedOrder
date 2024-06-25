using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileData
{
    public TileType tileType;
    public Color color;
    public float frogRotation;
}

public class BaseTile : MonoBehaviour
{
    public int width, height;

    public List<TileData> tiles;

    public List<Tile> InstantiatedTiles = new();

    private void Awake()
    {
        transform.parent.GetComponent<Level>().baseTiles.Add(this);

    }

    private void Start()
    {
        GridManager.Instance._tiles.Add(new Vector2(width, height), this);
        if (tiles.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            Tile instantiatedTile = Instantiate(GridManager.Instance._tilePrefab, transform).GetComponent<Tile>();
            InstantiatedTiles.Add(instantiatedTile);
            instantiatedTile.SetTileType(tiles[i].tileType, tiles[i].color);
            instantiatedTile.width = width;
            instantiatedTile.height = height;


            if (tiles[i].tileType == TileType.FROG)
            {
                instantiatedTile.SetFrogRotation(tiles[i].frogRotation);
            }
        }

        foreach (Tile tile in InstantiatedTiles)
        {
            tile.gameObject.SetActive(false);
        }

        InstantiatedTiles[0].gameObject.SetActive(true);

    }

    public void DeactivateAllInstantiatedTiles()
    {
        Destroy(InstantiatedTiles[0].gameObject);
        InstantiatedTiles.RemoveAt(0);


    }

    public void ActivateSecondTiles()
    {
        if (InstantiatedTiles.Count > 0)
        {

            InstantiatedTiles[0].gameObject.SetActive(true);
        }

    }
}
