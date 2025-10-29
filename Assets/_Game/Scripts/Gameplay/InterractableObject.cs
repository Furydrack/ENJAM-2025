using UnityEngine;
using System.Collections.Generic;
using TriInspector;
using System.Linq;

public class InterractableObject : MonoBehaviour
{
    [HideInInspector]
    public Collider2D collider;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Draggable draggable;
    [HideInInspector]
    public Stretchable stretchable;

    [Title("Runtime")]
    [ReadOnly]
    public bool isPlaced = false;
    [ReadOnly]
    public int savedSortingOrder;
    private Vector2 _startPosition;
    private int _startSortingOrder;
    private Color _startColor;
    private bool _willBeRemoved;

    #region Unity
    private void Start()
    {
        draggable = GetComponent<Draggable>();
        stretchable = GetComponent<Stretchable>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        isPlaced = false;

        _startPosition = transform.position;
        _startColor = spriteRenderer.color;
        _startSortingOrder = spriteRenderer.sortingOrder;
        _willBeRemoved = false;
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
        savedSortingOrder = spriteRenderer.sortingOrder;
        if (GameManager.instance.currentPhase == GameManager.GamePhase.ENVIRONMENT)
            GameManager.instance.OnStartEditing(this);
    }

    private void OnMouseUp()
    {
        if (_willBeRemoved)
        {
            RemoveObjectFromBuild();
            return;
        }
    }

    public void ResetSortingOrder() => spriteRenderer.sortingOrder = savedSortingOrder;
    #endregion

    #region Public methods
    public void CheckRemove(bool toRemove)
    {
        _willBeRemoved = toRemove;
        spriteRenderer.color = toRemove ? Color.blue : _startColor;
    }

    public void RemoveObjectFromBuild()
    {
        if(GameManager.instance.placedObjects.Contains(gameObject))
            GameManager.instance.placedObjects.Remove(gameObject);
        GameManager.instance.currentDraggedObject = null;
        transform.position = _startPosition;
        spriteRenderer.sortingOrder = _startSortingOrder;
        isPlaced = false;
        spriteRenderer.color = _startColor;
    }
    #endregion
}
