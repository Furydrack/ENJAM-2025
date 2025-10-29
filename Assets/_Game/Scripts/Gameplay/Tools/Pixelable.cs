using UnityEngine;

public class Pixelable : MonoBehaviour
{
    private InterractableObject _object;
    private PixelSettingsSO _settings;
    private Vector2 _lastMousePos;

    private void Start()
    {
        _object = GetComponent<InterractableObject>();
        _settings = ToolsManager.instance.pixelSettings;
    }

    #region Detection events
    private void OnMouseDown()
    {
        if (GameManager.instance.isInUI) return;

        if (ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.PIXEL)
            return;

        StartPixelasing();
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.isInUI) return;

        if (ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.PIXEL)
            return;

        Pixelasing();
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.isInUI) return;

        if (ToolsManager.instance.currentToolUsed != ToolsManager.CurrentToolUsed.PIXEL)
            return;

        EndPixelasing();
    }
    #endregion

    #region Pixel gestion
    private void StartPixelasing()
    {
        _lastMousePos = Input.mousePosition;
    }

    private void Pixelasing()
    {
        Vector2 mousePos = (Vector2)Input.mousePosition;
        Vector2 mouseDelta = mousePos - _lastMousePos;
        _lastMousePos = mousePos;

        Material mat = _object.spriteRenderer.material;

        float force = mouseDelta.x > mouseDelta.y ? mouseDelta.x : mouseDelta.y;
        float pixelSize = Mathf.Clamp(mat.GetFloat("_PixelSize") * (1f + force * _settings.forceCoef), _settings.limits.x, _settings.limits.y);

        mat.SetFloat("_PixelSize", pixelSize);
    }

    private void EndPixelasing()
    {

    }
    #endregion
}

