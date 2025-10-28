using UnityEngine;

public class Stretchable : MonoBehaviour
{
    private Vector2 _lastMousePos;
    private StretchSettingsSO _settings;

    private Vector2Int _mouseSign;

    private void Start()
    {
        _settings = ToolsManager.instance.stretchSettings;
    }

    #region Detection events
    private void OnMouseDown()
    {
        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.STRETCH)
            return;

        StartStretch();
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.STRETCH)
            return;

        Stretch();
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.STRETCH)
            return;

        EndStretch();
    }
    #endregion

    #region Stretch gestion
    private void StartStretch()
    {
        _lastMousePos = Input.mousePosition;
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseSign = new Vector2Int((int)Mathf.Sign(worldMousePos.x - transform.position.x), (int)Mathf.Sign(worldMousePos.y - transform.position.y));
    }

    private void Stretch()
    {
        Vector2 mousePos = (Vector2)Input.mousePosition;
        Vector2 mouseDelta = mousePos - _lastMousePos;
        _lastMousePos = mousePos;

        float strecthForce = _settings.forceCoef;

        bool horStretch = Mathf.Abs(mouseDelta.x) >= Mathf.Abs(mouseDelta.y);
        Vector2 scale = transform.localScale;


        if(horStretch)
        {
            strecthForce *= mouseDelta.x * _mouseSign.x;
            scale.x += strecthForce;
            scale.y -= strecthForce;
            transform.localScale = scale;
        }
        else
        {
            // Vertical stretch
            strecthForce *= mouseDelta.y * _mouseSign.y;
            scale.x -= strecthForce;
            scale.y += strecthForce;
        }

        scale.x = Mathf.Clamp(scale.x, _settings.minScale.x, _settings.maxScale.x);
        scale.y = Mathf.Clamp(scale.y, _settings.minScale.y, _settings.maxScale.y);

        transform.localScale = scale;
    }

    private void EndStretch()
    {
        _lastMousePos = Vector2.zero;
    }
    #endregion
}
