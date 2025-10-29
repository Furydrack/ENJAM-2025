using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D BasicCursor;
    [SerializeField] private Texture2D[] CursorHoverArray;
    [SerializeField] private Texture2D[] CursorGrabArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float framerate;

    private int currentFrame;
    private float frameTimer;

    private bool isHovering;
    private bool isGrabing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

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

        if(isGrabing)
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

        if (!isHovering & !isGrabing) {
            Cursor.SetCursor(BasicCursor, new Vector2(10, 10), CursorMode.Auto);
        }


    }
}
