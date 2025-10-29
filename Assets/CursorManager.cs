using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [SerializeField] private Texture2D BasicCursor;
    [SerializeField] private Texture2D[] CursorHoverArray;
    [SerializeField] private Texture2D[] CursorGrabArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float framerate;

    private int currentFrame;
    private float frameTimer;

    public bool isHovering;
    public bool isGrabing;

    private bool isCustom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void SetCustomCursor()
    {
        isCustom = true;
        Cursor.SetCursor(BasicCursor, new Vector2(10, 10), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCustom) return;      

        if (isHovering)
        {
            if (frameTimer <= 0f)
            {
                frameTimer += framerate;
                currentFrame = (currentFrame + 1) % CursorHoverArray.Length;
                Cursor.SetCursor(CursorHoverArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
                print(currentFrame);
            }
            frameTimer -= Time.deltaTime;

        }

        else if(isGrabing)
        {
            if (frameTimer <= 0f)
            {
                frameTimer += framerate;
                currentFrame = (currentFrame + 1) % CursorHoverArray.Length;
                Cursor.SetCursor(CursorGrabArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
                print(currentFrame);
            }
            frameTimer -= Time.deltaTime;

        }

        else if (!isHovering && !isGrabing) {
            Cursor.SetCursor(BasicCursor, new Vector2(10, 10), CursorMode.Auto);
        }


    }
}
