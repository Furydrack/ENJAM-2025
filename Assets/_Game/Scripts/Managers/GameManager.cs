using System;
using System.Collections.Generic;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Title("Refs")]
    //[Header("Environment")]
    //[SerializeField]
    //private Transform _creationParent;
    [SerializeField]
    private Transform _zoomedObjectPlacementRef;
    [Header("UI"), SerializeField]
    private Button _finishButton;
    [SerializeField, PropertySpace]
    private GameObject _confirmPopup;
    [SerializeField]
    private Button _confirmFinishButton;
    [SerializeField]
    private Button _goToCreationButton;
    [SerializeField]
    private Button _returnToEditionButton;
    [SerializeField, PropertySpace]
    private GameObject _endScreen;
    [SerializeField]
    private GameObject _startScreen;
    [SerializeField, PropertySpace]
    private Button _startButton;

    public bool isInUI = true;

    [Title("Sounds")]
    [SerializeField]
    AudioClip _clickSFX, _stick1SFX, _stick2SFX;
    [SerializeField]
    AudioClip _ost;
    [SerializeField]
    AudioSource _sfxAudioSource;

    AudioSource _audioSource;


    [Title("Runtime")]
    [ReadOnly]
    public InterractableObject currentDraggedObject;
    public enum GamePhase { ENVIRONMENT, EDITION, END }
    [ReadOnly]
    public GamePhase currentPhase = GamePhase.ENVIRONMENT;
    [SerializeField, ReadOnly]
    private Creation _creation;
    [ReadOnly]
    public List<GameObject> placedObjects;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        isInUI = true;

        // Bind events
        _confirmPopup.SetActive(false);
        _endScreen.SetActive(false);
        _finishButton.onClick.AddListener(OnFinishPhase);
        _returnToEditionButton.onClick.AddListener(ClosePopup);
        _confirmFinishButton.onClick.AddListener(FinishCreation);
        _goToCreationButton.onClick.AddListener(GoToCreation);
        _startButton.onClick.AddListener(StartGame);
        ToolsManager.instance.HideTools();

        // Init game
        _creation = new Creation();
        placedObjects = new List<GameObject>();
        _finishButton.gameObject.SetActive(false);
        _goToCreationButton.gameObject.SetActive(false);

        _startScreen.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        isInUI = false;
        _startScreen.gameObject.SetActive(false);
        _finishButton.gameObject.SetActive(true);
        InitEnvironmentPhase();
        CursorManager.instance.SetCustomCursor();

        _audioSource.clip = _ost;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void OnStartEditing(InterractableObject draggedObject)
    {
        _sfxAudioSource.clip = _clickSFX;
        _sfxAudioSource.Play();

        currentDraggedObject = draggedObject;
        CameraManager.instance.fade.onFadeInFinished.AddListener(InitEditionPhase);
        CameraManager.instance.GoToZoomView();
    }

    private void InitEditionPhase()
    {
        CameraManager.instance.fade.onFadeInFinished.RemoveListener(InitEditionPhase);
        currentPhase = GamePhase.EDITION;

        if(currentDraggedObject != null && !currentDraggedObject.isPlaced)
            currentDraggedObject.transform.position = _zoomedObjectPlacementRef.transform.position;

        _goToCreationButton.gameObject.SetActive(false);
        // Init Tools
        ToolsManager.instance.InitTools();
    }

    private void InitEnvironmentPhase()
    {
        CameraManager.instance.fade.onFadeInFinished.RemoveListener(InitEnvironmentPhase);
        currentPhase = GamePhase.ENVIRONMENT;

        _goToCreationButton.gameObject.SetActive(true);
        ToolsManager.instance.HideTools();
    }

    private void OnFinishPhase()
    {
        if (currentPhase == GamePhase.ENVIRONMENT)
            EnablePopup();
        else if(currentPhase == GamePhase.EDITION)
            OnEndEditionPhase();
    }

    private void EnablePopup()
    {
        _confirmPopup.SetActive(true);
        isInUI = true;
    }

    private void ClosePopup()
    {
        _confirmPopup.SetActive(false);
        isInUI = false;
    }

    public void OnEndEditionPhase()
    {
        if(currentDraggedObject != null)
        {
            currentDraggedObject.isPlaced = true;
            placedObjects.Add(currentDraggedObject.gameObject);
        }
        currentDraggedObject = null;
        CameraManager.instance.fade.onFadeInFinished.AddListener(InitEnvironmentPhase);
        CameraManager.instance.GoToGlobalView();
        ToolsManager.instance.EndEditionPhase();
    }

    private void GoToCreation()
    {
        if (GameManager.instance.currentPhase == GameManager.GamePhase.ENVIRONMENT)
            GameManager.instance.OnStartEditing(null);
    }

    private void FinishCreation()
    {
        currentPhase = GamePhase.END;

        _confirmPopup.SetActive(false);

        // @todo move this code to the real finish creation after editing
        _creation = new Creation(placedObjects);

        // Move to view
        CameraManager.instance.GoToEndCam();
        CameraManager.instance.fade.onFadeInFinished.AddListener(InitEndView);

        //SavesManager.instance.AddCreationToLibrary(_creation);
    }

    private void InitEndView()
    {
        isInUI = true;
        _goToCreationButton.gameObject.SetActive(false);
        CameraManager.instance.fade.onFadeInFinished.RemoveListener(InitEndView);
        _endScreen.SetActive(true);
        _finishButton.GetComponentInChildren<TMP_Text>().text = "Retry";
        _finishButton.onClick.RemoveAllListeners();
        _finishButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
