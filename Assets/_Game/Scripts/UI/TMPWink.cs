using UnityEngine;
using TMPro;

public class TMPBlink : MonoBehaviour
{
    [Header("Réglages")]
    [Tooltip("Vitesse du clignotement (secondes pour un aller-retour)")]
    public float blinkDuration = 1f;

    private TMP_Text textMesh;
    private Color baseColor;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
        if (textMesh == null)
        {
            Debug.LogError("TMPBlink : Aucun TMP_Text trouvé sur " + gameObject.name);
            enabled = false;
            return;
        }

        baseColor = textMesh.color;
    }

    private void Update()
    {
        // PingPong retourne une valeur entre 0 et 1 en boucle
        float t = Mathf.PingPong(Time.time / blinkDuration, 1f);

        // Interpolation entre alpha 0 et alpha 1
        Color newColor = baseColor;
        newColor.a = Mathf.Lerp(0f, baseColor.a, t);

        textMesh.color = newColor;
    }
}
