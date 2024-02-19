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

    [SerializeField] public Transform GardenParentObject;
    [SerializeField] private GameObject _flowerObjectPrefab;
    [SerializeField] private GameObject _HiveSupportPrefab;

    [SerializeField] private int flowers_number;
    [SerializeField] private int hivePosition_number;

    private Dictionary<Vector2, Tile> _tiles;
    private int[,] gardenMatrix;

    private Vector2 OptSolution;

    public Vector2 OptimalSolution
    {
        get { return OptSolution; }
        set { OptSolution = value; }
    }

    public bool wrongGuess;

    void Start()
    {
        GenerateGrid();
        gardenMatrix = new int[_width, _width];

        invokeGardenObjects();
        OptimalSolution = FindOptimalSolution(gardenMatrix);
    }

    private void Update()
    {
        if (wrongGuess)
        {
            clearMatrix();
            DestroyChildrens(GardenParentObject);

            invokeGardenObjects();
            OptimalSolution = FindOptimalSolution(gardenMatrix);
            wrongGuess = false;
        }
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

    public void invokeGardenObjects()
    {
        List<Vector2> randomPositions = GenerateRandomPositions(_width, flowers_number + hivePosition_number);
        Tile tile = new Tile();

        int count = 0;

        foreach (Vector2 position in randomPositions)
        {

            if (count < flowers_number)
            {
                gardenMatrix[(int)position.x, (int)position.y] = 1;

                tile = GetTileAtPosition(position);
                var spawnedTile = Instantiate(_flowerObjectPrefab, new Vector3(tile.transform.position.x, 0.5f, tile.transform.position.z), Quaternion.identity, GardenParentObject);
                spawnedTile.name = $"Flower {tile.name}";
            }
            else
            {
                gardenMatrix[(int)position.x, (int)position.y] = 2;

                tile = GetTileAtPosition(position);
                var spawnedTile = Instantiate(_HiveSupportPrefab, new Vector3(tile.transform.position.x, 0.1f, tile.transform.position.z), Quaternion.identity, GardenParentObject);
                spawnedTile.name = $"BeeHive Support {tile.name}";
                spawnedTile.GetComponent<ObjectPosition>().TilePosition = position;
            }

            count++;
        }
    }

    public Vector2 FindOptimalSolution(int[,] matrix)
    {
        List<Vector2> posicoesPossiveis = new List<Vector2>();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 2) // Verifique se é uma posição possível para a colmeia
                {
                    posicoesPossiveis.Add(new Vector2(i, j));
                }
            }
        }


        float menorDistanciaTotal = float.MaxValue;
        Vector2 posicaoOtima = Vector2.zero;

        // Itera sobre as posições possíveis para a colmeia
        foreach (Vector2 posicaoPossivel in posicoesPossiveis)
        {
            float distanciaTotal = 0;

            // Itera sobre as posições das flores
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 1) // Verifica se é uma flor
                    {
                        // Calcula a distância entre a posição possível da colmeia e a posição da flor
                        float distancia = Vector2.Distance(posicaoPossivel, new Vector2(i, j));
                        distanciaTotal += distancia;
                    }
                }
            }

            // Atualiza a posição ótima se a distância total for menor
            if (distanciaTotal < menorDistanciaTotal)
            {
                menorDistanciaTotal = distanciaTotal;
                posicaoOtima = posicaoPossivel;
            }
        }

        return posicaoOtima;
    }

    public void clearMatrix()
    {
        for (int i = 0; i < gardenMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gardenMatrix.GetLength(1); j++)
            {
                gardenMatrix[i, j] = 0;
            }
        }
    }

    void DestroyChildrens(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
