using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int _width, _height;
    [SerializeField] private BaseTile _tileBasePrefab;
    [SerializeField] private GameObject[] levels;
     List<Level> _levels = new();

    [SerializeField] private Transform _cam;
    [SerializeField] private TextMeshProUGUI moveCounterText;

    public GameObject _tilePrefab;
    public Dictionary<Vector2, BaseTile> _tiles = new();
    public static GridManager Instance;

    private int currentLevelIndex = 0;

    public int moveCounter = 10;

    private void Awake()
    {
        Instance = this;
        UpdateMoveCounterText();
        InstantiateLevel();
    }

    private void InstantiateLevel()
    {
        var instantiatedLevel =  Instantiate(levels[currentLevelIndex]).GetComponent<Level>();
        _levels.Add(instantiatedLevel);
    }




    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && moveCounter > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.TryGetComponent<Tile>(out Tile hittedTile))
                {
                    if (hittedTile.CurrentTileType is TileType.FROG)
                    {
                        //Debug.Log(hit.transform.name);
                        hittedTile.CheckAttack();
                        DecreaseMoveCounter();

                    }
                }

            }
        }
    }
    public BaseTile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void DecreaseMoveCounter()
    {
        moveCounter--;
        UpdateMoveCounterText();
    }

    private void UpdateMoveCounterText()
    {
        moveCounterText.text = "Moves Left: " + moveCounter;
    }

    public void CheckLevelCompletion()
    {

        bool allListCleared = _levels[0].baseTiles.All(a => a.InstantiatedTiles.Count == 0);

        if (allListCleared)
        {
            //level finishewd
            currentLevelIndex++;
            if (currentLevelIndex < levels.Length)
            {
                ResetMoveCounter();
                _tiles = new();
                _levels.ForEach(a => Destroy(a.gameObject));
                _levels.Clear();
                InstantiateLevel();
            }
            else
            {
                Debug.Log("All levels completed!");
            }
        }


    }

    private void ResetMoveCounter()
    {
        moveCounter = 10;
        UpdateMoveCounterText();
    }

}
