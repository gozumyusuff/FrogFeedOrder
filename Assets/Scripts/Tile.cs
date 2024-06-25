using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public enum TileType
{
    DEFAULT,
    FROG,
    FRUIT
}



public class Tile : MonoBehaviour
{
    bool up = true, down = true, left = true, right = true;

    List<bool> possibleDirections = new List<bool>();

    public TileType CurrentTileType;

    public int width, height;

    [SerializeField] LineRenderer ln;

    public Color currentColor;

    [SerializeField] GameObject frog;
    [SerializeField] GameObject fruit;

    private void Awake()
    {
        possibleDirections.Add(up);
        possibleDirections.Add(down);
        possibleDirections.Add(left);
        possibleDirections.Add(right);

        switch (CurrentTileType)
        {
            case TileType.DEFAULT:
                break;
            case TileType.FROG:
                frog.SetActive(true);
                break;
            case TileType.FRUIT:
                fruit.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetTileType(TileType type, Color color)
    {
        currentColor = color;
        CurrentTileType = type;
        ChangeColor(color, transform.GetChild(0));

        switch (CurrentTileType)
        {
            case TileType.DEFAULT:
                break;
            case TileType.FROG:
                frog.SetActive(true);
                ChangeColor(color, frog.transform.GetChild(0));
                break;
            case TileType.FRUIT:
                fruit.SetActive(true);
                ChangeColor(color, fruit.transform);
                break;
            default:
                break;
        }
    }

    public void SetFrogRotation(float rotation)
    {
        if (CurrentTileType == TileType.FROG)
        {
            frog.transform.localRotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    public void ChangeColor(Color color, Transform tr)
    {
        tr.GetComponent<MeshRenderer>().materials[0].color = color;
    }

    public async void CheckAttack()
    {
        var forwardDir = frog.transform.forward;
        forwardDir = new Vector3(Mathf.RoundToInt(forwardDir.x), Mathf.RoundToInt(forwardDir.y), Mathf.RoundToInt(forwardDir.z));


        BaseTile baseTileToDeactivate = null;

        if (forwardDir.x > 0)
        {
            // soldan saða
            List<BaseTile> fruitTiles = new List<BaseTile>();
            for (int i = 0; i < GridManager.Instance._width; i++)
            {
                var baseTile = GridManager.Instance.GetTileAtPosition(new Vector2(i, height));
                fruitTiles.Add(baseTile);
                if (baseTile.tiles[0].tileType is TileType.FRUIT)
                {
                    ln.enabled = true;
                    Vector3 newPos = new(baseTile.transform.position.x, transform.position.y, -1);

                    ln.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                    ln.SetPosition(1, newPos);
                    await Task.Delay(500);

                    baseTileToDeactivate = baseTile;

                    


                }
            }
            foreach (var item in fruitTiles)
            {
                item.DeactivateAllInstantiatedTiles();
                item.ActivateSecondTiles();
            }

        }
        else if (forwardDir.x < 0)
        {
            // saðdan sola
            List<BaseTile> fruitTiles = new List<BaseTile>();
            for (int i = GridManager.Instance._width - 1; i >= 0; i--)
            {
                var baseTile = GridManager.Instance.GetTileAtPosition(new Vector2(i, height));
                fruitTiles.Add(baseTile);
                if (baseTile.tiles[0].tileType is TileType.FRUIT)
                {
                    ln.enabled = true;
                    Vector3 newPos = new(baseTile.transform.position.x, transform.position.y, -1);

                    ln.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                    ln.SetPosition(1, newPos);
                    await Task.Delay(500);

                    baseTileToDeactivate = baseTile;
                    
                }
            }
            foreach (var item in fruitTiles)
            {
                item.DeactivateAllInstantiatedTiles();
                item.ActivateSecondTiles();
            }
        }
        else if (forwardDir.y > 0)
        {
            // yukarý
            List<BaseTile> fruitTiles = new List<BaseTile>();
            for (int i = 0; i < GridManager.Instance._height; i++)
            {
                var baseTile = GridManager.Instance.GetTileAtPosition(new Vector2(width, i));
                fruitTiles.Add(baseTile);
                if (baseTile.tiles[0].tileType is TileType.FRUIT)
                {
                    ln.enabled = true;
                    Vector3 newPos = new(transform.position.x, baseTile.transform.position.y, -1);

                    ln.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                    ln.SetPosition(1, newPos);
                    await Task.Delay(500);

                    baseTileToDeactivate = baseTile;
                    
                }
            }
            foreach (var item in fruitTiles)
            {
                item.DeactivateAllInstantiatedTiles();
                item.ActivateSecondTiles();
            }
        }
        else if (forwardDir.y < 0)
        {
            List<BaseTile> fruitTiles = new List<BaseTile>();
            // aþaðý
            for (int i = GridManager.Instance._height - 1; i >= 0; i--)
            {
                var baseTile = GridManager.Instance.GetTileAtPosition(new Vector2(width, i));
                fruitTiles.Add(baseTile);
                if (baseTile.tiles[0].tileType is TileType.FRUIT)
                {
                    ln.enabled = true;
                    Vector3 newPos = new(transform.position.x, baseTile.transform.position.y, -1);

                    ln.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                    ln.SetPosition(1, newPos);
                    await Task.Delay(500);

                    baseTileToDeactivate = baseTile;
                   
                }
            }

            foreach (var item in fruitTiles)
            {
                item.DeactivateAllInstantiatedTiles();
                item.ActivateSecondTiles();
            }
        }

        GridManager.Instance.CheckLevelCompletion();


    }
}
