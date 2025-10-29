using System.Collections.Generic;
using System;
using UnityEngine;
using TriInspector;

[Serializable]
public struct CreationLibrary
{
    public List<Creation> library;

    public CreationLibrary(List<Creation> library)
    {
        this.library = library;
    }
}

[Serializable]
public struct Creation
{
    [ReadOnly]
    public string creationName;
    [TextArea, ReadOnly]
    public string creationDescription;
    [ReadOnly]
    public List<PlacedObject> placedObjects;
    [ReadOnly]
    public float price;

    public Creation(List<GameObject> obj)
    {
        creationName = "";
        creationDescription = "";
        price = 0;
        placedObjects = new List<PlacedObject>();

        foreach (GameObject go in obj)
            placedObjects.Add(new PlacedObject(go));
    }
}

[Serializable]
public struct PlacedObject
{
    // Transform
    public Vector3 pos, scale;
    public Quaternion rot;
    // SpriteRenderer
    public Sprite sprite;
    public Color color;
    public int sortingOrder;

    public PlacedObject(GameObject go)
    {
        pos = go.transform.position;
        scale = go.transform.localScale;
        rot = go.transform.rotation;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sprite = sr.sprite;
        color = sr.color;
        sortingOrder = sr.sortingOrder;
    }
}
