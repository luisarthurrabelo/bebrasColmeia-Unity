using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MapCanvas : MonoBehaviour
{
    private GridManager grid_component;

    public GameObject Map;
    public GameObject prefab;
    public GameObject flower_prefab;
    public GameObject beeHive_prefab;

    public float _tileMapSize = 1.0f;
    private bool mapActive = false;

    private Dictionary<Vector2, GameObject> _tilesMap;

    private void Start()
    {
        grid_component = GameObject.FindObjectOfType<GridManager>();
        GenerateMapGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!mapActive)
            {
                Map.SetActive(true);
                invokeMapSprites();
                mapActive = !mapActive;
            }
            else
            {
                mapActive = !mapActive;
                destroyGardenObjects();
                Map.SetActive(false);
            }
        }
    }

    void GenerateMapGrid()
    {
        _tilesMap = new Dictionary<Vector2, GameObject>();

        float gridWidth = grid_component._width * _tileMapSize;
        float gridHeight = grid_component._height * _tileMapSize;

        float startX = -gridWidth / 2.0f + _tileMapSize / 2.0f;
        float startY = -gridHeight / 2.0f + _tileMapSize / 2.0f;

        for (int i = 0; i < grid_component._width; i++)
        {
            for (int j = 0; j < grid_component._height; j++)
            {
                float x = startX + i * _tileMapSize;
                float y = startY + j * _tileMapSize;

                var spawnedTile = Instantiate(prefab);
                spawnedTile.transform.parent = Map.transform;
                spawnedTile.transform.localPosition = new Vector3(x, y, -0.1f);
                spawnedTile.transform.localRotation = Quaternion.identity;

                spawnedTile.name = $"TileMap {i} {j}";

                _tilesMap[new Vector2(i, j)] = spawnedTile;
            }
        }
    }

    public GameObject GetTileAtPosition(Vector2 pos)
    {
        if (_tilesMap.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }

    public void invokeMapSprites()
    {
        int[,] matrix = grid_component.gardenMatrix;

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int cellValue = matrix[row, col];

                GameObject tileMap = GetTileAtPosition(new Vector2(row, col));

                Vector3 tilePosition = tileMap.transform.localPosition;
                Vector3 position = new Vector3(tilePosition.x, tilePosition.y, -0.8f);

                GameObject newObject;

                switch (cellValue)
                {
                    case 1:

                        newObject = Instantiate(flower_prefab);

                        newObject.transform.parent = Map.transform;

                        newObject.transform.localPosition = position;
                        newObject.transform.localRotation = Quaternion.identity;
                        newObject.transform.localScale = Vector3.one;
                        break;
                    case 2:

                        newObject = Instantiate(beeHive_prefab);

                        newObject.transform.parent = Map.transform;
                        newObject.transform.localPosition = position;
                        newObject.transform.localRotation = Quaternion.identity;
                        newObject.transform.localScale = Vector3.one;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void destroyGardenObjects()
    {
        foreach (Transform child in Map.transform)
        {
            if (child.CompareTag("MapSprite"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}
