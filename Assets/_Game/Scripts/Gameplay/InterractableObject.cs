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

    #region Unity
    private void Start()
    {
        draggable = GetComponent<Draggable>();
        stretchable = GetComponent<Stretchable>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        isPlaced = false;
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

    public void ResetSortingOrder() => spriteRenderer.sortingOrder = savedSortingOrder;
    #endregion
}
