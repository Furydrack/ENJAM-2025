using UnityEngine;

public class Colorable : MonoBehaviour
{
    private ColorSettingsSO _settings;
    private InterractableObject _object;

    private Vector2 _lastMousePos;

    private void Start()
    {
        _settings = ToolsManager.instance.colorSettings;
        _object = GetComponent<InterractableObject>();
    }

    #region Detection events
    private void OnMouseDown()
    {
        if (ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.COLOR)
            return;

        StartColoring();
    }

    private void OnMouseDrag()
    {
        if (ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.COLOR)
            return;

        Coloring();
    }

    private void OnMouseUp()
    {
        if (ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.COLOR)
            return;

        EndColoring();
    }
    #endregion

    #region Color gestion
    private void StartColoring()
    {
        Color randomColor = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        _object.spriteRenderer.color = randomColor;

        _lastMousePos = Input.mousePosition;
    }

    private void Coloring()
    {
        //Color newColor = _object.spriteRenderer.color;

        //// New color by drag
        //newColor.r += Random.Range(_settings.randomFork.x, _settings.randomFork.y);
        //newColor.g += Random.Range(_settings.randomFork.x, _settings.randomFork.y);
        //newColor.b += Random.Range(_settings.randomFork.x, _settings.randomFork.y);

        //// Cycle colors
        //if(newColor.r < 0f) newColor.r = 1f;
        //else if(newColor.r > 1f) newColor.r = 0f;
        //if(newColor.g < 0f) newColor.g = 1f;
        //else if(newColor.g > 1f) newColor.g = 0f;
        //if(newColor.b < 0f) newColor.b = 1f;
        //else if(newColor.b > 1f) newColor.b = 0f;

        Vector2 mousePos = (Vector2)Input.mousePosition;
        Vector2 mouseDelta = mousePos - _lastMousePos;
        _lastMousePos = mousePos;

        Color.RGBToHSV(_object.spriteRenderer.color, out float H, out float S, out float V);
        H = Mathf.Clamp01(H + _settings.forceCoef * mouseDelta.x);
        S = Mathf.Clamp01(S + _settings.forceCoef * mouseDelta.y);

        _object.spriteRenderer.color = Color.HSVToRGB(H, S, V);
    }

    private void EndColoring()
    {

    }
    #endregion
}
