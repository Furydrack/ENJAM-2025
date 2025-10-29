using UnityEngine;
using System.Collections.Generic;
using TriInspector;

public class SavesManager : MonoBehaviour
{
    public static SavesManager instance;

    [SerializeField, ReadOnly]
    private CreationLibrary _creationsLibrary;
    public CreationLibrary creationsLibrary => _creationsLibrary;

    private const string CREATIONS_KEY = "Creations";

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void Start()
    {
        if(string.IsNullOrEmpty(PlayerPrefs.GetString(CREATIONS_KEY, string.Empty)))
            _creationsLibrary.library = new List<Creation>();
        else
            _creationsLibrary = JsonUtility.FromJson<CreationLibrary>(PlayerPrefs.GetString(CREATIONS_KEY));
    }

    public void AddCreationToLibrary(Creation creation)
    {
        _creationsLibrary.library.Insert(0, creation);
        PlayerPrefs.SetString(CREATIONS_KEY, JsonUtility.ToJson(_creationsLibrary));
    }

    public GameObject GetRebuildCreationFromLibrary(int index)
    {
        if (index < 0 || index >= _creationsLibrary.library.Count) return null;

        return GetRebuildCreation(_creationsLibrary.library[index]);
    }

    private GameObject GetRebuildCreation(Creation creation)
    {
        var parent = Instantiate(new GameObject());

        foreach (var po in creation.placedObjects)
        {
            var obj = Instantiate(new GameObject());

            // Transform
            obj.transform.position = po.pos;
            obj.transform.rotation = po.rot;
            obj.transform.localScale = po.scale;

            // Sprite Renderer
            obj.AddComponent<SpriteRenderer>();
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.sprite = po.sprite;
            sr.color = po.color;
            sr.sortingOrder = po.sortingOrder;

            // Set to parent
            obj.transform.SetParent(parent.transform);
        }

        return parent;
    }
}
