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
}
