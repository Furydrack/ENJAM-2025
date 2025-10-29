using UnityEngine;

[CreateAssetMenu(fileName = "StretchSettings", menuName = "Settings/Stretch")]
public class StretchSettingsSO : ScriptableObject
{
    public float forceCoef = 1f;
    public Vector2 minScaleProportion, maxScaleProportion;
}
