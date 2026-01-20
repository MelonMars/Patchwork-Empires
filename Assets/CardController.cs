using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    Camera cam;
    bool dragging;
    public Vector3 initialScale;
    public Vector3 scaleFactor = new Vector3(1.05f, 0, 1.05f);
    public LayerMask boardMask;
    Vector3 originalPosition;

    bool placed;
    bool isInHand = true;
    BoardGrid board;
    HandManager manager;


    void Start()
    {
        initialScale = transform.localScale;
        cam = Camera.main;
        board = FindObjectOfType<BoardGrid>();
        manager = GetComponentInParent<HandManager>();
    }

    void OnMouseEnter()
    {
        if (isInHand && !placed)
            transform.localScale = Vector3.Scale(scaleFactor, initialScale);
    }

    void OnMouseExit()
    {
        if (!dragging)
        {
            transform.localScale = initialScale;
        }
    }

    void OnMouseDown()
    {
        if (placed) return;
        originalPosition = transform.position;
        dragging = true;
    }

    void OnMouseUp()
    {
        if (!dragging) return;
        dragging = false;

        Vector2 curr = new Vector2(transform.position.x, transform.position.z);
        Vector2 orig = new Vector2(originalPosition.x, originalPosition.z);

        Debug.Log(Vector2.Distance(curr, orig));
        if (Vector2.Distance(curr, orig) < 3f)
        {
            manager.ReturnCard(transform);
            transform.localScale = initialScale;
            return;
        }
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, boardMask))
        {
            transform.position = board.SnapToGrid(hit.point);
            transform.rotation = Quaternion.identity;
            manager.PlaceCard(transform);
            isInHand = false;
            return;
        }
        manager.ReturnCard(transform);
        transform.localScale = initialScale;
    }

    void Update()
    {
        if (!dragging) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 snapped = board.SnapToGrid(hit.point);
            transform.position = snapped + Vector3.up * 0.02f;
        }
    }
}