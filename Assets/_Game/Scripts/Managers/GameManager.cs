using TriInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Title("Refs")]
    [SerializeField]
    private Transform _zoomedObjectPlacementRef;
    [Header("UI"), SerializeField]
    private Button _finishButton;
    [SerializeField, PropertySpace]
    private GameObject _confirmPopup;
    [SerializeField]
    private Button _confirmFinishButton;
    [SerializeField]
    private Button _returnToEditionButton;
    [SerializeField, PropertySpace]
    private GameObject _endScreen;

    [Title("Runtime")]
    [SerializeField, ReadOnly]
    private DraggableObject _currentDraggedObject;
    public enum GamePhase { ENVIRONMENT, EDITION }
    [ReadOnly]
    public GamePhase currentPhase = GamePhase.ENVIRONMENT;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // Bind events
        _confirmPopup.SetActive(false);
        _endScreen.SetActive(false);
        _finishButton.onClick.AddListener(OnFinishPhase);
        _returnToEditionButton.onClick.AddListener(() => _confirmPopup.SetActive(false));
        _confirmFinishButton.onClick.AddListener(FinishCreation);

        InitEnvironmentPhase();
    }

    public void OnStartEditing(DraggableObject draggedObject)
    {
        _currentDraggedObject = draggedObject;
        CameraManager.instance.fade.onFadeInFinished.AddListener(InitEditionPhase);
        CameraManager.instance.GoToZoomView();
    }

    private void InitEditionPhase()
    {
        CameraManager.instance.fade.onFadeInFinished.RemoveListener(InitEditionPhase);
        currentPhase = GamePhase.EDITION;
        if(_currentDraggedObject != null && !_currentDraggedObject.isPlaced)
            _currentDraggedObject.transform.position = _zoomedObjectPlacementRef.transform.position;
    }

    private void InitEnvironmentPhase()
    {
        CameraManager.instance.fade.onFadeInFinished.RemoveListener(InitEnvironmentPhase);
        currentPhase = GamePhase.ENVIRONMENT;
    }

    private void OnFinishPhase()
    {
        if (currentPhase == GamePhase.ENVIRONMENT)
            _confirmPopup.SetActive(true);
        else if(currentPhase == GamePhase.EDITION)
            OnEndZoomPhase();
    }

    public void OnEndZoomPhase()
    {
        if(_currentDraggedObject != null)
            _currentDraggedObject.isPlaced = true;
        _currentDraggedObject = null;
        CameraManager.instance.fade.onFadeInFinished.AddListener(InitEnvironmentPhase);
        CameraManager.instance.GoToGlobalView();
    }

    private void FinishCreation()
    {
        _endScreen.SetActive(true);
    }
}
