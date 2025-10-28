using UnityEngine;
using System.Collections.Generic;
using TriInspector;
using System.Linq;

public class DraggableObject : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    [Title("Runtime")]
    [SerializeField, ReadOnly]
    private List<DraggableObject> _behindObjects;
    [SerializeField, ReadOnly]
    private bool _isDragging = false;
    [SerializeField, ReadOnly]
    private int _savedSortingOrder;

    #region Unity
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Detection events
    private void OnMouseEnter()
    {
        //Debug.Log($"Mouse entered {name}");
    }

    private void OnMouseExit()
    {
        //Debug.Log($"Mouse exited {name}");
    }

    private void OnMouseDown()
    {
        //Debug.Log($"Mouse clicked on {name}");
        StartDrag();
    }

    private void OnMouseUp()
    {
        //Debug.Log($"Mouse released click on {name}");
        EndDrag();
    }

    private void OnMouseDrag()
    {
        //Debug.Log($"Mouse is dragging {name}");
        Dragging();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<DraggableObject>(out DraggableObject obj) && _isDragging)
            _behindObjects.Add(obj);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<DraggableObject>(out DraggableObject obj) && _behindObjects.Contains(obj))
            _behindObjects.Remove(obj);
    }
    #endregion

    #region Drag gestion
    private void StartDrag()
    {
        _savedSortingOrder = _spriteRenderer.sortingOrder;
        _spriteRenderer.sortingOrder = ObjectsManager.instance.highestSortingOrder+1;
        _isDragging = true;
    }

    private void Dragging()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + new Vector3(0f, 0f, 10f);
        transform.position = curPosition;
    }

    private void EndDrag()
    {
        _isDragging = false;

        // Set new sorting order
        int newSortingOrder = _savedSortingOrder;
        if(_behindObjects.Count > 0)
        {
            int highestOrder = 0;
            foreach (var obj in _behindObjects)
                if (obj.GetComponent<SpriteRenderer>().sortingOrder > highestOrder)
                    highestOrder = obj.GetComponent<SpriteRenderer>().sortingOrder;

            newSortingOrder = ++highestOrder;
            if(highestOrder > ObjectsManager.instance.highestSortingOrder)
                ObjectsManager.instance.highestSortingOrder = highestOrder;
        }  
        _spriteRenderer.sortingOrder = newSortingOrder;
    }
    #endregion
}
