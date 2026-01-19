using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour {
    Camera cam;
    bool dragging;
    public Vector3 initialScale;
    public Vector3 scaleFactor = new Vector3(1.05f, 0, 1.05f);
    public GameObject board;

    void Start() {
        initialScale = transform.localScale;
        cam = Camera.main;
    }

    void OnMouseEnter() {
        transform.localScale = Vector3.Scale(scaleFactor, initialScale);
    }

    void OnMouseExit() {
        if (!dragging) {
            transform.localScale = initialScale;
        }
    }

    void OnMouseDown() {
        dragging = true;
    }

    void OnMouseUp() {
        dragging = false;
    }

    void Update() {
        if (!dragging) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            transform.position = hit.point + Vector3.up * 0.02f;
        }
    }
}