using UnityEngine;

public class ObjectToBuild : MonoBehaviour
{
    public void OnMouseDown()
    {
        if(GameManager.instance.currentPhase == GameManager.GamePhase.ENVIRONMENT)
            GameManager.instance.OnStartEditing(null);
    }
}
