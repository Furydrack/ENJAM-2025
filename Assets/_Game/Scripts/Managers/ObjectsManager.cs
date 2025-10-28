using UnityEngine;
using System.Collections.Generic;
using TriInspector;

public class ObjectsManager : MonoBehaviour
{
    public static ObjectsManager instance;

    [Title("Runtime")]
    [SerializeField, ReadOnly]
    List<GameObject> _interractableObjects = new List<GameObject>();
    [ReadOnly]
    public int highestSortingOrder;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        _interractableObjects.AddRange(GameObject.FindGameObjectsWithTag("InterractableObject"));

        foreach (GameObject obj in _interractableObjects)
            if (obj.GetComponent<SpriteRenderer>().sortingOrder > highestSortingOrder)
                highestSortingOrder = obj.GetComponent<SpriteRenderer>().sortingOrder;
    }
}
