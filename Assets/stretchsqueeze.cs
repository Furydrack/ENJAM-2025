using UnityEditor.UI;
using UnityEngine;

public class stretchsqueeze : MonoBehaviour
{
    public float scaleSpeed = 10000000f;
    public float MinScale = 0.01f;
    public float MaxScale = 3f;

    public float ScaleUp = 1f;
    public float ScaleDown = -0.5f;

    private Vector3 Scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Scale = transform.localScale;

        if (Input.GetKey(KeyCode.E))
        {
            Scale += new Vector3(ScaleUp, ScaleDown, 0) ;
                }

        if (Input.GetKey(KeyCode.R)) 
        {
            Scale += new Vector3(ScaleDown, ScaleUp, 0) ;
        }

        Scale.y = Mathf.Clamp(Scale.y, MinScale, MaxScale);
        Scale.x = Mathf.Clamp(Scale.x, MinScale, MaxScale);


        transform.localScale = Scale;

    }




}
