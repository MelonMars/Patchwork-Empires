using UnityEngine;

public class BoardGrid : MonoBehaviour {
    public float cellSize = 1f;
    public Vector2Int gridSize = new(50, 50);

    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        Vector3 offset = worldPos - transform.position;

        float snapX = Mathf.Round(offset.x / cellSize) * cellSize;
        float snapZ = Mathf.Round(offset.z / cellSize) * cellSize;

        return new Vector3(transform.position.x + snapX, transform.position.y, transform.position.z + snapZ);
    }
}
 