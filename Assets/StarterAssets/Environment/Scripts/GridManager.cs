using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private float _tileSize = 1.0f;
    [SerializeField] private Vector3 _centerPosition;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameObject _flowerObjectPrefab;
    [SerializeField] private GameObject _HiveSupportPrefab;

    [SerializeField] private int flowers_number;
    [SerializeField] private int hivePosition_number;

    private Dictionary<Vector2, Tile> _tiles;
    private int[,] gardenMatrix;

    void Start()
    {
        GenerateGrid();
        gardenMatrix = new int[_width, _width];

        List<Vector2> randomPositions = GenerateRandomPositions(_width, flowers_number + hivePosition_number);
        Tile tile = new Tile();

        //foreach (Vector2 position in randomPositions)
        //{
        //    Debug.Log(position);
        //}

        int count = 0;

        foreach (Vector2 position in randomPositions)
        {

            if (count < flowers_number)
            {
                gardenMatrix[(int)position.x, (int)position.y] = 1;

                tile = GetTileAtPosition(position);
                var spawnedTile = Instantiate(_flowerObjectPrefab, new Vector3(tile.transform.position.x, 0.5f, tile.transform.position.z), Quaternion.identity);
                spawnedTile.name = $"Flower {tile.name}";
            }
            else
            {
                gardenMatrix[(int)position.x, (int)position.y] = 2;

                tile = GetTileAtPosition(position);
                var spawnedTile = Instantiate(_HiveSupportPrefab, new Vector3(tile.transform.position.x, 0.1f, tile.transform.position.z), Quaternion.identity);
                spawnedTile.name = $"BeeHive Support {tile.name}";
                spawnedTile.GetComponent<ObjectPosition>().TilePosition = position;
            }
       
            count++;
        }

        //for (int i = 0; i < gardenMatrix.GetLength(0); i++)
        //{
        //    for (int j = 0; j < gardenMatrix.GetLength(1); j++)
        //    {
        //        Debug.Log("Elemento na posição (" + i + "," + j + "): " + gardenMatrix[i, j]);
        //    }
        //}
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        float gridWidth = _width * _tileSize;
        float gridHeight = _height * _tileSize;

        float startX = _centerPosition.x - gridWidth / 2.0f;
        float startZ = _centerPosition.z - gridHeight / 2.0f;

        for (int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                float x = startX + i * _tileSize;
                float y = startZ + j * _tileSize;

                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, 0.01f, y), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";

                var isOffset = ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0));
                spawnedTile.init(isOffset);

                _tiles[new Vector2(i, j)] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }

    List<Vector2> GenerateRandomPositions(int size, int count)
    {
        List<Vector2> positions = new List<Vector2>();
        System.Random rand = new System.Random();

        while (positions.Count < count)
        {
            int x = rand.Next(size);
            int y = rand.Next(size);
            Vector2 newPosition = new Vector2(x, y);

            bool positionExists = false;
            foreach (Vector2 existingPosition in positions)
            {
                if (existingPosition == newPosition)
                {
                    positionExists = true;
                    break;
                }
            }

            if (!positionExists)
            {
                positions.Add(newPosition);
            }
        }

        return positions;
    }

    //public void addHiveOnHiveSupMatrix(Vector2 position, int value)
    //{

    //    if (position.x >= 0 && position.x < _width && position.y >= 0 && position.y < _width)
    //    {
    //        int posX = (int)position.x;
    //        int posY = (int)position.y;

    //        gardenMatrix[posX, posY] = value;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Posição fora dos limites da matriz.");
    //    }

    //    //for (int i = 0; i < gardenMatrix.GetLength(0); i++)
    //    //{
    //    //    for (int j = 0; j < gardenMatrix.GetLength(1); j++)
    //    //    {
    //    //        Debug.Log("Elemento na posição (" + i + "," + j + "): " + gardenMatrix[i, j]);
    //    //    }
    //    //}
    //}
}
