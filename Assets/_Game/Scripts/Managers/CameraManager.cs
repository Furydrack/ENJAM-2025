using TriInspector;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Title("Refs")]
    [SerializeField]
    private Camera _globalCam;
    [SerializeField]
    private Camera _zoomedCam;
    [SerializeField]
    private Camera _endCamera;
    public FadeInOut fade;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

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
        fade.onFadeInFinished.RemoveListener(() => ToggleCams(toGlobalView));
        fade.onFadeInFinished.AddListener(() => ToggleCams(toGlobalView));
        fade.LaunchFade();
    }

    private void ToggleCams(bool globalIsActive)
    {
        _zoomedCam.gameObject.SetActive(!globalIsActive);
        _globalCam.gameObject.SetActive(globalIsActive);
    }

    public void GoToEndCam()
    {
        fade.onFadeInFinished.RemoveListener(ToggleCams);
        fade.onFadeInFinished.AddListener(ToggleCams);
        fade.LaunchFade();
    }

    private void ToggleCams()
    {
        _globalCam.gameObject.SetActive(false);
        _endCamera.gameObject.SetActive(true);
    }
}
