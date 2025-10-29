using UnityEngine;

public class Stretchable : MonoBehaviour
{
    private Vector2 _lastMousePos;
    private StretchSettingsSO _settings;

    private Vector2Int _mouseSign;

    private Vector2 _minScale, _maxScale;

    private void Start()
    {
        _settings = ToolsManager.instance.stretchSettings;
        _minScale = new Vector2(transform.localScale.x * (1f-_settings.minScaleProportion.x), transform.localScale.y * (1f-_settings.minScaleProportion.y));
        _maxScale = new Vector2(transform.localScale.x * (1f+_settings.maxScaleProportion.x), transform.localScale.y * (1f+_settings.maxScaleProportion.y));
    }

    #region Detection events
    private void OnMouseDown()
    {
        if (GameManager.instance.isInUI) return;

        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.STRETCH)
            return;

        StartStretch();
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.isInUI) return;

        if (GameManager.instance.currentPhase != GameManager.GamePhase.EDITION || ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.STRETCH)
            return;

        Stretch();
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.isInUI) return;

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

        scale.x = Mathf.Clamp(scale.x, _minScale.x, _maxScale.x);
        scale.y = Mathf.Clamp(scale.y, _minScale.y, _maxScale.y);

        transform.localScale = scale;
    }

    private void EndStretch()
    {
        _lastMousePos = Vector2.zero;
    }
    #endregion
}
