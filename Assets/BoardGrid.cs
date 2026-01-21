using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    public float cellSize = 1f;
    public Vector2Int gridSize = new(50, 50);
    public GameObject highlightPrefab;
    private GameObject[,] highlights;
    private bool[,] validTiles;

    void Awake()
    {
        highlights = new GameObject[gridSize.x, gridSize.y];
        validTiles = new bool[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 originOffset = new Vector3(gridSize.x * cellSize / 2f, 0, gridSize.y * cellSize / 2f);
                Vector3 worldPos = transform.position - originOffset + new Vector3(x * cellSize, 0, y * cellSize);
                GameObject highlight = Instantiate(highlightPrefab, worldPos, Quaternion.identity, transform);
                highlight.transform.localScale = new Vector3(0.1f / transform.localScale.x, 1, 0.1f / transform.localScale.z);
                highlight.SetActive(false);
                highlights[x, y] = highlight;
            }
        }
        int centerX = gridSize.x / 2;
        int centerY = gridSize.y / 2;

        validTiles[centerX, centerY] = true;
    }

    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector3 offset = worldPos - transform.position;

        float snapX = Mathf.Round(offset.x / cellSize) * cellSize;
        float snapZ = Mathf.Round(offset.z / cellSize) * cellSize;

        return new Vector3(transform.position.x + snapX, transform.position.y, transform.position.z + snapZ);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 originOffset = new Vector3(gridSize.x * cellSize / 2f, 0, gridSize.y * cellSize / 2f);
        Vector3 offset = worldPos - (transform.position - originOffset);

        int x = Mathf.FloorToInt(offset.x / cellSize);
        int y = Mathf.FloorToInt(offset.z / cellSize);
        return new Vector2Int(x, y);
    }

    public bool IsValidTile(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        if (gridPos.x < 0 || gridPos.x >= gridSize.x || gridPos.y < 0 || gridPos.y >= gridSize.y)
            return false;
        return validTiles[gridPos.x, gridPos.y];
    }

    public void ShowHighlights()
    {
        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                highlights[x, y].SetActive(validTiles[x, y]);
    }

    public void HideHighlights()
    {
        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
                highlights[x, y].SetActive(false);
    }

    public void PlaceCardAt(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        if (gridPos.x < 0 || gridPos.x >= gridSize.x || gridPos.y < 0 || gridPos.y >= gridSize.y)
            return;
        validTiles[gridPos.x, gridPos.y] = false; 

        Vector2Int[] neighbors = new Vector2Int[]
        {
            new Vector2Int(gridPos.x + 1, gridPos.y),
            new Vector2Int(gridPos.x - 1, gridPos.y),
            new Vector2Int(gridPos.x, gridPos.y + 1),
            new Vector2Int(gridPos.x, gridPos.y - 1)
        };

        foreach (var neighbor in neighbors)
        {
            if (neighbor.x >= 0 && neighbor.x < gridSize.x && neighbor.y >= 0 && neighbor.y < gridSize.y)
            {
                validTiles[neighbor.x, neighbor.y] = true;
            }
        }
    }
}