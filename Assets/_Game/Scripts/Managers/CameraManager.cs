using TriInspector;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Title("Refs")]
    [SerializeField]
    private Camera _globalCam;
    [SerializeField]
    private Camera _zoomedCam;
    [SerializeField]
    FadeInOut _fade;

    private void Start()
    {
        ToggleCams(true);
    }

    [Button]
    public void GoToZoomView() => ChangeView(false);
    [Button]
    public void GoToGlobalView() => ChangeView(true);

    private void ChangeView(bool toGlobalView)
    {
        _fade.onFadeInFinished.RemoveListener(() => ToggleCams(toGlobalView));
        _fade.onFadeInFinished.AddListener(() => ToggleCams(toGlobalView));
        _fade.LaunchFade();
    }

    private void ToggleCams(bool globalIsActive)
    {
        _zoomedCam.gameObject.SetActive(!globalIsActive);
        _globalCam.gameObject.SetActive(globalIsActive);
    }
}
