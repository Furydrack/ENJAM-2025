using UnityEngine;

[CreateAssetMenu(fileName = "ColoringSettings", menuName = "Settings/Coloring")]
public class ColorSettingsSO : ScriptableObject
{
    [SerializeField]
    private Vector2 _randomFork = new Vector2(-1f,1f);
    public Vector2 randomFork => _randomFork * forceCoef;
    public float forceCoef;
}
