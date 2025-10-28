using UnityEngine;

public class Stretchable : MonoBehaviour
{
    //tailles minimales et maximales des sprites
    public float MinScale = 0.01f;
    public float MaxScale = 3f;

    //Valeurs utilisés lors des stretch et squeeze
    public float ScaleUp = 1f;
    public float ScaleDown = -0.5f;

    private Vector3 Scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    public void Stretch()
    {

        Scale = transform.localScale;



        if (Input.GetKey(KeyCode.E))
        {
            Scale += new Vector3(ScaleUp, ScaleDown, 0);
        }

        if (Input.GetKey(KeyCode.R))
        {
            Scale += new Vector3(ScaleDown, ScaleUp, 0);
        }

        //Clamp la taille possible de l'objet
        Scale.y = Mathf.Clamp(Scale.y, MinScale, MaxScale);
        Scale.x = Mathf.Clamp(Scale.x, MinScale, MaxScale);


        transform.localScale = Scale;

    }
}
