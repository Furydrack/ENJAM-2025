using System.Collections.Generic;
using TriInspector;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private float rotationSpeed = 10f;

    private InterractableObject _object;

    public bool isDragging = false;
    [SerializeField, ReadOnly]
    private List<InterractableObject> _behindObjects;

    private void Start()
    {
        _object = GetComponent<InterractableObject>();
    }

    #region Detection events
    private void OnMouseDown()
    {
        if (GameManager.instance.isInUI) return;

        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.isEditingWithTool)
            return;

        StartDrag();
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.isInUI) return;

        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.isEditingWithTool)
            return;

        Dragging();
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.isInUI) return;

        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.isEditingWithTool)
            return;

        EndDrag();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.isEditingWithTool) 
            return;

        if (collision.transform.TryGetComponent<InterractableObject>(out InterractableObject obj) && isDragging)
            _behindObjects.Add(obj);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.isEditingWithTool)
            return;

        if (collision.transform.TryGetComponent<InterractableObject>(out InterractableObject obj) && _behindObjects.Contains(obj))
            _behindObjects.Remove(obj);
    }
    #endregion

    #region Drag gestion
    Vector3 _startOffet;
    private void StartDrag()
    {
        GameManager.instance.currentDraggedObject = _object;
        _object.collider.enabled = false;
        _object.collider.enabled = true;
        _object.spriteRenderer.sortingOrder = ObjectsManager.instance.highestSortingOrder + 1;
        isDragging = true;

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        _startOffet = transform.position - curPosition;
    }

    private void Dragging()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

        transform.position = curPosition + _startOffet;

        // Rotate
        float scrollDelta = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            // Fait tourner l'objet autour de son axe Z (2D)
            transform.Rotate(Vector3.forward, -scrollDelta * rotationSpeed, Space.Self);
        }
    }

    private void EndDrag()
    {
        isDragging = false;

        // Set new sorting order
        int newSortingOrder = _object.savedSortingOrder;
        if (_behindObjects.Count > 0)
        {
            int highestOrder = 0;
            foreach (var obj in _behindObjects)
                if (obj.GetComponent<SpriteRenderer>().sortingOrder > highestOrder)
                    highestOrder = obj.GetComponent<SpriteRenderer>().sortingOrder;

            newSortingOrder = ++highestOrder;
            if (highestOrder > ObjectsManager.instance.highestSortingOrder)
                ObjectsManager.instance.highestSortingOrder = highestOrder;
        }
        _object.spriteRenderer.sortingOrder = newSortingOrder;
    }
    #endregion
}
