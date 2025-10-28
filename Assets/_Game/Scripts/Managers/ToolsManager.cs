using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ToolsManager : MonoBehaviour
{
    public static ToolsManager instance;

    [Title("Refs")]
    [Header("UI"), SerializeField]
    private GameObject _uiContainer;
    [SerializeField]
    private SpriteRenderer _focusBackgroundRenderer;
    [SerializeField]
    private ToggleGroup _toggleGroup;
    [SerializeField, ReadOnly]
    private List<Toggle> _toolsToggle; // Order important

    public enum CurrentToolUsed { NONE = 0, STRETCH = 1 }
    [Title("Runtime"), ReadOnly]
    public CurrentToolUsed currentToolUsed = CurrentToolUsed.NONE;
    public bool isEditingWithTool => currentToolUsed != CurrentToolUsed.NONE;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        for(int i=0; i < _toggleGroup.transform.childCount; i++)
        {
            int id = i+1;
            Toggle toggle = _toggleGroup.transform.GetChild(i).GetComponent<Toggle>();
            _toolsToggle.Add(toggle);
            toggle.onValueChanged.AddListener((value) => OnToolEnabled(value, id));
        }
    }

    public void InitTools()
    {
        _uiContainer.SetActive(true);
        _toggleGroup.SetAllTogglesOff();
    }

    public void EndEditionPhase()
    {
        currentToolUsed = CurrentToolUsed.NONE;
        ToggleFocus(false);
        HideTools();
    }

    public void HideTools()
    {
        _uiContainer.SetActive(false);
    }

    private void OnToolEnabled(bool enabled, int tool)
    {
        if(!enabled)
        {
            if(!_toggleGroup.AnyTogglesOn())
            {
                currentToolUsed = CurrentToolUsed.NONE;
                ToggleFocus(false);
            }

            return;
        }

        if (!isEditingWithTool)
            ToggleFocus(true);

        currentToolUsed = (CurrentToolUsed)tool;
        Debug.Log($"{currentToolUsed} is used");
    }

    private void ToggleFocus(bool enable)
    {
        if(enable)
        {
            _focusBackgroundRenderer.sortingOrder = ObjectsManager.instance.highestSortingOrder + 1;
            GameManager.instance.currentDraggedObject.spriteRenderer.sortingOrder = ObjectsManager.instance.highestSortingOrder + 2;
            _focusBackgroundRenderer.gameObject.SetActive(true);
        }
        else
        {
            _focusBackgroundRenderer.gameObject.SetActive(false);
            GameManager.instance.currentDraggedObject?.ResetSortingOrder();
        }
    }
}
